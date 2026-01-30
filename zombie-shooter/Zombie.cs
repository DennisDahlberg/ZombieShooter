using Godot;
using System;

namespace ZombieShooter;
public partial class Zombie : CharacterBody2D
{
	private int _health = 100;
	
	public void HandleHitByBullet()
	{
		_health -= 20;
		if (_health <= 0)
		{
			QueueFree();
			GD.Print("OUCH!!!!!!");
		}
	}
}
