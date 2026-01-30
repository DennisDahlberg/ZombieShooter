using Godot;
using System;

namespace ZombieShooter;
public partial class Bullet : Area2D
{
	[Export] public float Speed = 10f;
	
	private Vector2 _direction = Vector2.Zero;
	
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
	}
}
