using Godot;
using System;

namespace ZombieShooter;
public partial class BulletManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleBulletSpawned(Bullet bullet, Vector2 position, Vector2 direction)
	{
		GD.Print(bullet);
		GD.Print("bullet");
		AddChild(bullet);
		bullet.GlobalPosition = position;
		bullet.SetDirection(direction);
	}
}
