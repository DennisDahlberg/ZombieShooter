using Godot;
using System;
using ZombieShooter.Gun;

namespace ZombieShooter;
public partial class Weapon : Node2D
{
	[Signal] public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
	[Signal] public delegate void AmmoAmountChangedEventHandler(int newAmmo);
	[Signal] public delegate void MaxAmmoAmountChangedEventHandler(int newAmmo);
	[Signal]public delegate void WeaponChangedEventHandler(Rect2 weaponRegion);

	private WeaponData _currentWeaponData;
	private int _currentAmmo;

	private int _ammoPrimaryWeapon;
	private int _ammoSecondaryWeapon;
	
	private Marker2D _endOfGun;
	private Marker2D _gunDirection;
	private Timer _attackCooldown;
	private AnimationPlayer _animation;
	
	public int GetCurrentAmmo() => _currentAmmo;
	public int GetMaxAmmo() => _currentWeaponData.MaxAmmo;
	
	public override void _Ready()
	{
		_endOfGun = GetNode<Marker2D>("EndOfGun");
		_gunDirection = GetNode<Marker2D>("GunDirection");
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		
		_currentAmmo = _currentWeaponData.MaxAmmo;
		
		_animation.Stop();
		GetNode<Sprite2D>("MuzzleFlash").Hide();
	}
	
	public void Initialize(WeaponData data, int saveAmmo)
	{
		_currentWeaponData = data;
		_currentAmmo = saveAmmo;
        
		_attackCooldown.WaitTime = data.AttackCooldown;
		_animation.SpeedScale = data.ReloadCooldown;
		
		
        
		EmitSignalMaxAmmoAmountChanged(data.MaxAmmo);
		EmitSignalAmmoAmountChanged(_currentAmmo);
		EmitSignalWeaponChanged(data.IconRegion);
        
		_animation.Stop();
		GetNode<Sprite2D>("MuzzleFlash").Hide();
	}
	
	public void Shoot()
	{
		if (!_attackCooldown.IsStopped() || _currentWeaponData == null)
			return;
		
		if (_animation.IsPlaying())
			return;
		
		if (_currentAmmo <= 0)
		{
			Reload();
			return;
		}
		
		var bullet = (Bullet)_currentWeaponData.Bullet.Instantiate();
		var target = GetGlobalMousePosition();
		var directionToMouse = _gunDirection.GlobalPosition - _endOfGun.GlobalPosition;
		EmitSignalPlayerFiredBullet(bullet, _endOfGun.GlobalPosition, directionToMouse);
		_attackCooldown.Start();
		_animation.Play("muzzle_flash");
		_currentAmmo--;
		EmitSignalAmmoAmountChanged(_currentAmmo);
	}

	public void Reload()
	{
		_animation.Play("reload");
	}

	public void StopReload()
	{
		_currentAmmo = _currentWeaponData.MaxAmmo;
		EmitSignalAmmoAmountChanged(_currentAmmo);
	}

	public void ApplyFireRateMultiplier(float increase)
	{
		_attackCooldown.WaitTime = _currentWeaponData.AttackCooldown * increase; 		
	}

	public void ApplyReloadSpeedMultiplayer(float increase)
	{
		_animation.SpeedScale = increase;
	}

	public void ResetFireRateMultiplier()
	{
		_attackCooldown.WaitTime = _currentWeaponData.AttackCooldown;
	}

}
