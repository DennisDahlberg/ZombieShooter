using Godot;
using System;

namespace ZombieShooter;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 250f;

	public override void _PhysicsProcess(double delta)
	{
		LookAt(GetGlobalMousePosition());
		Vector2 inputDirection = Input
			.GetVector("move_left", "move_right", "move_up", "move_down");
		
		Velocity = inputDirection * Speed;
		MoveAndSlide();
	}
}
