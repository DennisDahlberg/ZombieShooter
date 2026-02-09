using Godot;
using System;
using System.Collections.Generic;
using ZombieShooter;

namespace ZombieShooter;
public partial class PerkManager : Node2D
{
	public static PerkManager Instance { get; private set; }
	
	private Player _player;
	private List<string> _activePerks = [];

	public override void _Ready()
	{
		Instance = this;
		_player = GetParent<Player>();
	}

	public void AddPerk(string perkName)
	{
		if (_activePerks.Contains(perkName))
			return;

		if (_activePerks.Count >= 4)
			return;
		
		ApplyPerkEffect(perkName);
		_activePerks.Add(perkName);
		foreach (var perk in _activePerks)
			GD.Print(perk);
	}

	private void ApplyPerkEffect(string perkName)
	{
		switch (perkName)
		{
			case "Revive":
				break;
			case "Juggernog":
				break;
			case "StaminaUp":
				_player.Speed *= 1.20f;
				break;
		}
	}
}
