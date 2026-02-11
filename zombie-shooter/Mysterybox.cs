using Godot;
using System;
using System.Text;
using System.Threading.Tasks;
using Godot.Collections;
using ZombieShooter.Gun;

namespace ZombieShooter;
public partial class Mysterybox : StaticBody2D
{
	[Export] public Array<WeaponData> WeaponPool = [];
	[Export] public float SpinDuration = 3.0f;
	[Export] public int Cost = 950;

	private AnimatedSprite2D _sprite;
	private Marker2D _spawnPoint;
	private Timer _spinTimer;
	private Timer _spinCooldownTimer;
	private Sprite2D _gunSprite;
	private Area2D _buyArea;

	private bool _isSpinning = false;
	private bool _isPlayerInRange = false;
	private bool _isOpen = false;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_spawnPoint = GetNode<Marker2D>("WeaponSpawnPoint");
		_spinTimer = GetNode<Timer>("SpinTimer");
		_spinCooldownTimer = GetNode<Timer>("SpinCooldown");
		_buyArea = GetNode<Area2D>("BuyArea");

		_gunSprite = new Sprite2D();
		_gunSprite.ZIndex = 1000;
		_gunSprite.Scale = new Vector2(2.0f, 2.0f);
		_spawnPoint.AddChild(_gunSprite);
		_gunSprite.Hide();
		
		_sprite.Play("idle");
		
		_buyArea.BodyEntered += OnBodyEntered;
		_buyArea.BodyExited += OnBodyExited;
		_spinTimer.Timeout +=  OnTimerTimeout;
	}

	public override void _Process(double delta)
	{
		if (!_isSpinning || !_spinCooldownTimer.IsStopped())
			return;
		
		_spinCooldownTimer.Start(0.2f);
		int randomIndex = (int)(GD.Randi() % WeaponPool.Count);
		_gunSprite.Texture = WeaponPool[randomIndex].Icon;
	}

	public override void _Input(InputEvent @event)
	{
		if (_isPlayerInRange && !_isSpinning && @event.IsActionPressed("buy"))
		{
			StartSpin();
		}
	}

	private void StartSpin()
	{
		_isSpinning = true;
		_isOpen = true;
		_sprite.Play("open");
		_gunSprite.Show();
		_spinTimer.Start(SpinDuration);
	}

	public void OnTimerTimeout()
	{
		StopSpin();
	}

	private void StopSpin()
	{
		_isSpinning = false;
		
		GetTree().CreateTimer(4.0f).Timeout += () => {
			_gunSprite.Hide();
			_sprite.Play("close");
		};
		_isOpen = false;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is not Player)
			return;
		
		_isPlayerInRange = true;
		if (!_isOpen)
		{
			GameManager.Instance.UpdateActionLabel($"Press F to open Mystery box [{Cost}]");
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			_isPlayerInRange = false;
			GameManager.Instance.UpdateActionLabel("");
		}
	}
}
