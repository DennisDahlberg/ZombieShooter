using Godot;
using System;

namespace ZombieShooter;
public partial class PerkMachine : StaticBody2D
{
	[Export] public int Cost = 500;
	[Export] public string PerkName = "Revive";

	private Area2D _buyArea;
	private bool _playerInRange;

	public override void _Ready()
	{
		_buyArea = GetNode<Area2D>("BuyArea");
		
		_buyArea.BodyEntered += OnBodyEntered;
		_buyArea.BodyExited += OnBodyExited;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GameManager.Instance.UpdateActionLabel("Press F to buy perk [" + Cost + "]");
			_playerInRange = true;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GameManager.Instance.UpdateActionLabel("");
			_playerInRange = false;
		}
	}
}
