using Godot;
using System;

namespace ZombieShooter;
public partial class Weapon : Node2D
{
	[Signal]
	public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
	
	[Export] public PackedScene Bullet;
	
	private Marker2D _endOfGun;
	private Marker2D _gunDirection;
	private Timer _attackCooldown;
	private AnimationPlayer _animation;
	
	public override void _Ready()
	{
		_endOfGun = GetNode<Marker2D>("EndOfGun");
		_gunDirection = GetNode<Marker2D>("GunDirection");
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
        
		_animation.Stop();
		GetNode<Sprite2D>("MuzzleFlash").Hide();
	}
	
	public void Shoot()
	{
		if (!_attackCooldown.IsStopped() || Bullet == null)
		{
			return;
		}
		var bullet = (Bullet)Bullet.Instantiate();
		var target = GetGlobalMousePosition();
		var directionToMouse = _gunDirection.GlobalPosition - _endOfGun.GlobalPosition;
		EmitSignalPlayerFiredBullet(bullet, _endOfGun.GlobalPosition, directionToMouse);
		_attackCooldown.Start();
		_animation.Play("muzzle_flash");
	}
	
}
