using Godot;
using System;

namespace ZombieShooter;
public partial class Weapon : Node2D
{
	[Signal]
	public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
	
	[Signal]
	public delegate void AmmoAmountChangedEventHandler(int newAmmo);
	
	[Signal]
	public delegate void MaxAmmoAmountChangedEventHandler(int newAmmo);
	
	[Export] public PackedScene Bullet;
	[Export] public int MaxAmmo = 5;

	private int _currentAmmo;
	
	private Marker2D _endOfGun;
	private Marker2D _gunDirection;
	private Timer _attackCooldown;
	private AnimationPlayer _animation;
	
	public int GetCurrentAmmo() => _currentAmmo;
	public int GetMaxAmmo() => MaxAmmo;
	
	public override void _Ready()
	{
		_endOfGun = GetNode<Marker2D>("EndOfGun");
		_gunDirection = GetNode<Marker2D>("GunDirection");
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		
		_currentAmmo = MaxAmmo;
		
		_animation.Stop();
		GetNode<Sprite2D>("MuzzleFlash").Hide();
	}
	
	public void Shoot()
	{
		if (!_attackCooldown.IsStopped() || Bullet == null || _currentAmmo <= 0)
		{
			return;
		}

		if (_animation.IsPlaying())
			return;
		
		var bullet = (Bullet)Bullet.Instantiate();
		var target = GetGlobalMousePosition();
		var directionToMouse = _gunDirection.GlobalPosition - _endOfGun.GlobalPosition;
		EmitSignalPlayerFiredBullet(bullet, _endOfGun.GlobalPosition, directionToMouse);
		_attackCooldown.Start();
		_animation.Play("muzzle_flash");
		_currentAmmo--;
		EmitSignalAmmoAmountChanged(_currentAmmo);
		GD.Print(_currentAmmo);
	}

	public void Reload()
	{
		_animation.Play("reload");
	}

	public void StopReload()
	{
		_currentAmmo = MaxAmmo;
		GD.Print(_currentAmmo);
	}

}
