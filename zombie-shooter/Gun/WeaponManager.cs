using Godot;
using System;
using System.Collections.Generic;

namespace ZombieShooter.Gun;
public partial class WeaponManager : Node2D
{
	public static WeaponManager Instance { get; private set; }

	public class WeaponSlot
	{
		public WeaponData Data;
		public int CurrentAmmo;

		public WeaponSlot(WeaponData data)
		{
			Data = data;
			CurrentAmmo = data.MaxAmmo;
		}
	}
	
	[Export] public WeaponData PrimaryWeapon;
	[Export] public WeaponData SecondaryWeapon;

	private List<WeaponSlot> _inventorySlots = [];
	private Weapon _weaponNode;
	private bool _usingPrimary = true;
	private int _weaponIndex = 0;

	public override void _Ready()
	{
		Instance = this;
		_weaponNode = GetParent().GetNode<Weapon>("Weapon"); 
		
		_inventorySlots.Add(new WeaponSlot(PrimaryWeapon));
		_inventorySlots.Add(new WeaponSlot(SecondaryWeapon));
        
		if (PrimaryWeapon != null)
		{
			_weaponNode.Initialize(PrimaryWeapon, PrimaryWeapon.MaxAmmo);
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
		_inventorySlots[_weaponIndex].CurrentAmmo = _weaponNode.GetCurrentAmmo();

		_weaponIndex = _weaponIndex == 0 ? 1 : 0;
		var weapon = _inventorySlots[_weaponIndex];
		
		_weaponNode.Initialize(weapon.Data, weapon.CurrentAmmo);
	}
}
