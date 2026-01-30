using Godot;
using System;

namespace ZombieShooter;
public partial class Bullet : Area2D
{
	[Export] public float Speed = 15f;
	
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
			var velocity = _direction * Speed;
			
			GlobalPosition += velocity;
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
		if (!body.HasMethod("HandleHitByBullet"))
			return;
		body.Call("HandleHitByBullet");
		QueueFree();
	}
}
