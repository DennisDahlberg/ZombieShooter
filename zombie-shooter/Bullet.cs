using Godot;
using System;
using ZombieShooter.Gun;

namespace ZombieShooter;
public partial class Bullet : Area2D
{
	[Export] public float Speed = 600f;
	
	private Vector2 _direction = Vector2.Zero;
	private Timer _killTimer;

	public override void _Ready()
	{
		_killTimer = GetNode<Timer>("KillTimer");
		_killTimer.Start();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_direction != Vector2.Zero)
		{
			GlobalPosition += _direction * Speed * (float)delta;
		}
	}

	public void SetDirection(Vector2 direction)
	{
		_direction = direction.Normalized();
		Rotation += direction.Angle();
	}

	public void OnKillTimerTimeout()
	{
		_killTimer.Stop();
		QueueFree();
	}

	public void OnBulletBodyEntered(Node body)
	{
		if (body.HasMethod("HandleHitByBullet"))
			body.Call("HandleHitByBullet", WeaponManager.Instance.GetEquippedWeaponDamage());
		
		QueueFree();
	}
}
