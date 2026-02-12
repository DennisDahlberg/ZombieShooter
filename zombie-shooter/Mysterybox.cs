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

	private WeaponData _mysteryWeapon;
	private bool _isSpinning = false;
	private bool _isPlayerInRange = false;
	private bool _isOpen = false;
	private bool _canPickup = false;

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
		UpdateGunSprite(WeaponPool[randomIndex]);
	}

	public override void _Input(InputEvent @event)
	{
		if (_isPlayerInRange && !_isOpen && !_isSpinning && @event.IsActionPressed("buy"))
		{
			TryPurchase();
		}
		else if (_isPlayerInRange && _canPickup && @event.IsActionPressed("buy"))
		{
			PickupWeapon();
		}
	}

	private void StartSpin()
	{
		_isSpinning = true;
		_isOpen = true;
		_sprite.Play("open");
		_gunSprite.Show();

		_gunSprite.Scale = Vector2.Zero;
		_gunSprite.Position = new Vector2(0f, 10f);

		var tween = CreateTween()
			.SetTrans(Tween.TransitionType.Back)
			.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(_gunSprite, "scale", new Vector2(2.0f, 2.0f), 0.3f);
		tween.Parallel().TweenProperty(_gunSprite, "position", Vector2.Zero, 0.3f);
		
		_spinTimer.Start(SpinDuration);
		UpdateLabel();
	}

	public void OnTimerTimeout()
	{
		StopSpin();
	}

	private void StopSpin()
	{
		_isSpinning = false;
		
		int finalIndex = (int)(GD.Randi() % WeaponPool.Count);
		_mysteryWeapon = WeaponPool[finalIndex];
		UpdateGunSprite(WeaponPool[finalIndex]);

		var floatTween = CreateTween().SetLoops();
		floatTween.TweenProperty(_gunSprite, "position:y", -5.0f, 0.8f)
			.AsRelative()
			.SetTrans(Tween.TransitionType.Sine);
		floatTween.TweenProperty(_gunSprite, "position:y", 5.0f, 0.8f)
			.AsRelative()
			.SetTrans(Tween.TransitionType.Sine);
		
		
		_canPickup = true;
		UpdateLabel();
		
		GetTree().CreateTimer(4.0f).Timeout += () => {
			if (_mysteryWeapon is not null)
			{
				_gunSprite.Hide();
				_sprite.Play("close");
				_isOpen = false;
				_canPickup = false;
				_mysteryWeapon = null;
				UpdateLabel();	
			}
			
		};
	}

	private void PickupWeapon()
	{
		WeaponManager.Instance.AddWeaponToInventory(_mysteryWeapon);
		_gunSprite.Hide();
		_sprite.Play("close");
		_isOpen = false;
		_canPickup = false;
		_mysteryWeapon = null;
		UpdateLabel();
	}

	private void TryPurchase()
	{
		if (GameManager.Instance.SpendMoney(Cost))
		{
			StartSpin();
		}
	}

	private void UpdateLabel()
	{
		GameManager.Instance.UpdateActionLabel("");
		
		if (!_isPlayerInRange)
			return;
		
		if (!_isOpen)
		{
			GameManager.Instance.UpdateActionLabel($"Press F to open Mystery box [{Cost}]");
		}
		if (_canPickup && _isOpen)
		{
			GameManager.Instance.UpdateActionLabel("Press F to pickup gun");
		}
	}
	
	private void UpdateGunSprite(WeaponData data)
	{
		if (data == null) return;

		var fullSheet = GD.Load<Texture2D>("res://main_assets/gun/more_guns.png");

		AtlasTexture atlas = new AtlasTexture();
		atlas.Atlas = fullSheet;
		atlas.Region = data.IconRegion; 

		_gunSprite.Texture = atlas;
		_gunSprite.TextureFilter = TextureFilterEnum.Nearest; 
	}

	private void OnBodyEntered(Node body)
	{
		if (body is not Player)
			return;
		
		_isPlayerInRange = true;
		UpdateLabel();
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			_isPlayerInRange = false;
			UpdateLabel();
		}
	}
}
