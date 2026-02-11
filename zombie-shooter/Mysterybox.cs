using Godot;
using System;
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
	private Sprite2D _gunSprite;
	private Area2D _buyArea;
	
	private bool _isSpinning = false;
	private bool _isPlayerInRange = false;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_spawnPoint = GetNode<Marker2D>("WeaponSpawnPoint");
		_spinTimer = GetNode<Timer>("SpinTimer");
		_buyArea = GetNode<Area2D>("BuyArea");

		_gunSprite = new Sprite2D();
		_spawnPoint.AddChild(_gunSprite);
		_gunSprite.Hide();
		
		_sprite.Play("idle");
		
		_buyArea.BodyEntered += OnBodyEntered;
		_buyArea.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_isSpinning)
		{
			int randomIndex = (int)(GD.Randi() % WeaponPool.Count);
			_gunSprite.Texture = WeaponPool[randomIndex].Icon;
		}
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			_isPlayerInRange = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			_isPlayerInRange = false;
		}
	}
}
