using Godot;
using System;

namespace ZombieShooter;
public partial class Zombie : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	
	private int _health = 100;
	private Node2D _player;

	public override void _Ready()
	{
		var players = GetTree().GetNodesInGroup("Player");
		_player = (Node2D)players[0];
	}
	
	

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
