using Godot;
using System;

namespace ZombieShooter;
public partial class Zombie : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	
	private NavigationAgent2D _navAgent;
	private int _health = 100;
	private Node2D _player;
	private Timer _attackCooldown;
	private Area2D _hitbox;

	public override void _Ready()
	{
		var players = GetTree().GetNodesInGroup("Player");
		_player = (Node2D)players[0];
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		
		_hitbox = GetNode<Area2D>("Hitbox");
		_hitbox.BodyEntered += OnHitboxBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player is null)
			return;
		
		_navAgent.TargetPosition = _player.GlobalPosition;
		if (_navAgent.IsTargetReached()) return;
		Vector2 nextPathPosition = _navAgent.GetNextPathPosition();
		
		Vector2 direction = GlobalPosition.DirectionTo(nextPathPosition);
		Velocity = direction * Speed;
		
		LookAt(_player.GlobalPosition);
		MoveAndSlide();
		
		// Vector2 direction = GlobalPosition.DirectionTo(_player.GlobalPosition);
		// Velocity = direction * Speed;
		// LookAt(_player.GlobalPosition);
		//
		// MoveAndSlide();
	}

	public void HandleHitByBullet()
	{
		_health -= 20;
		if (_health <= 0)
		{
			GameManager.Instance.AddMoney(50);
			QueueFree();
			GD.Print("OUCH!!!!!!");
		}
		GameManager.Instance.AddMoney(10);
	}
	
	private void OnHitboxBodyEntered(Node2D body)
	{
		GD.Print("Hitbox entered");
		if (!_attackCooldown.IsStopped())
			return;
		if (body.IsInGroup("Player"))
		{
			body.Call("TakeDamage", 10);
			
			Vector2 pushDirection = GlobalPosition.DirectionTo(_player.GlobalPosition);
			body.Call("ApplyKnockback", pushDirection, 500.0f);
			_attackCooldown.Start();
		}
	}
}
