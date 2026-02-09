using Godot;
using System;

namespace ZombieShooter;
public partial class PerkMachine : StaticBody2D
{
	[Export] public int Cost = 500;
	[Export] public string PerkName = "StaminaUp";

	private Area2D _buyArea;
	private bool _playerInRange;
	private bool _isBought;

	public override void _Ready()
	{
		_buyArea = GetNode<Area2D>("BuyArea");
		
		_buyArea.BodyEntered += OnBodyEntered;
		_buyArea.BodyExited += OnBodyExited;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player") && !_isBought)
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

	public override void _Input(InputEvent @event)
	{
		if (_playerInRange && @event.IsActionPressed("buy") && !_isBought)
		{
			TryPurchase();
		}
	}

	private void TryPurchase()
	{
		if (GameManager.Instance.SpendMoney(Cost))
		{
			GameManager.Instance.UpdateActionLabel("");
			PerkManager.Instance.AddPerk(PerkName);
			_isBought = true;
		}
		else
		{
			GD.Print("Unable to buy perk!");
		}
	}
}
