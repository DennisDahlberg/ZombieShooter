using Godot;
using System;
using System.Collections.Generic;
using ZombieShooter;

namespace ZombieShooter;
public partial class PerkManager : Node2D
{
	private Player _player;
	private List<string> _activePerks = [];

	public override void _Ready()
	{
		_player = GetParent<Player>();
	}

	public void AddPerk(string perkName)
	{
		if (_activePerks.Contains(perkName))
			return;

		if (_activePerks.Count >= 4)
			return;
		
		_activePerks.Add(perkName);
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
				break;
		}
	}
}
