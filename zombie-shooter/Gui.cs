using Godot;
using System;

namespace ZombieShooter;
public partial class Gui : CanvasLayer
{
	private ProgressBar _healthBar;
	private Label _currentAmmo;
	private Label _maxAmmo;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("MarginContainer/Rows/BottomRow/HealthSection/HealthBar");
		_currentAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/AmmoSection/CurrentAmmo");
		_maxAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/AmmoSection/MaxAmmo");
	}

	public void SetHealth(int newHealth)
	{
		
	}

	public void SetMaxAmmo(int newMaxAmmo)
	{
		
	}

	public void SetCurrentAmmo(int newCurrentAmmo)
	{
		
	}
}
