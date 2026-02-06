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
		
		if (Velocity.Length() > 0)
		{
			Rotation = Velocity.Angle(); 
		}
		
		MoveAndSlide();
		
		if (_attackCooldown.IsStopped())
		{
			CheckForBite();
		}
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
	
	private void CheckForBite()
	{
		var bodies = _hitbox.GetOverlappingBodies();
		foreach (Node2D body in bodies)
		{
			if (body.IsInGroup("Player"))
			{
				PerformAttack(body);
				break;
			}
		}
	}
	
	private void PerformAttack(Node2D body)
	{
		GD.Print("Biting the player!");
		body.Call("TakeDamage", 10);
    
		Vector2 pushDirection = GlobalPosition.DirectionTo(body.GlobalPosition);
		body.Call("ApplyKnockback", pushDirection, 500.0f);
    
		_attackCooldown.Start();
	}
	
	private void OnHitboxBodyEntered(Node2D body)
	{
		if (_attackCooldown.IsStopped() && body.IsInGroup("Player"))
		{
			PerformAttack(body);
		}
	}
}
