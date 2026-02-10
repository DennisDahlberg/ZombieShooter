using Godot;
using System;

namespace ZombieShooter.Gun;
public partial class WeaponManager : Node2D
{
	[Export] public WeaponData PrimaryWeapon;
	[Export] public WeaponData SecondaryWeapon;

	private Weapon _weaponNode;
	private bool _usingPrimary = true;

	public override void _Ready()
	{
		_weaponNode = GetParent().GetNode<Weapon>("Weapon"); 
        
		if (PrimaryWeapon != null)
		{
			_weaponNode.Initialize(PrimaryWeapon);
		}
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("toggle_weapon"))
		{
			ToggleWeapon();
		}
	}

	private void ToggleWeapon()
	{
		_usingPrimary = !_usingPrimary;
        
		WeaponData selectedWeapon = _usingPrimary ? PrimaryWeapon : SecondaryWeapon;

		if (selectedWeapon != null)
		{
			_weaponNode.Initialize(selectedWeapon);
			GD.Print($"Swapped to: {selectedWeapon.Name}");
		}
	}
}
