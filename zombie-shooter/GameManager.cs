using Godot;
using System;

namespace ZombieShooter;
public partial class GameManager : Node2D
{
	public static GameManager Instance { get; private set; }

	[Signal] public delegate void MoneyChangedEventHandler(int currentMoney);
	[Signal] public delegate void ActionLabelChangedEventHandler(string label);

	public int Money { get; private set; } = 500; 

	public override void _Ready()
	{
		Instance = this;
	}
	
	public void AddMoney(int amount)
	{
		Money += amount;
		EmitSignalMoneyChanged(Money);
	}
	
	public bool SpendMoney(int amount)
	{
		if (Money >= amount)
		{
			Money -= amount;
			EmitSignalMoneyChanged(Money);
			return true;
		}
		return false;
	}

	public void UpdateActionLabel(string label)
	{
		EmitSignalActionLabelChanged(label);
	}
}
