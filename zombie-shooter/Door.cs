using Godot;
using System;

namespace ZombieShooter;
public partial class Door : StaticBody2D
{
	[Signal] public delegate void BuyDoorEventHandler(int doorCost);
	
	[Export] public int Cost = 1000;
	private Area2D _buyArea;
	private bool _playerInRange = false;

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
			GD.Print("Player in range. Press F to buy for " + Cost);
			_playerInRange = true;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Player left range.");
			_playerInRange = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (_playerInRange && @event.IsActionPressed("buy")) 
		{
			TryPurchase();
		}
	}

	private void TryPurchase()
	{
		if (GameManager.Instance.SpendMoney(Cost))
		{
			GD.Print("Door opened!!!");
			QueueFree();
		}
		else
		{
			GD.Print("Door unable to purchase money.");
		}
	}
}
