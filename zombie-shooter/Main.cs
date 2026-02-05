using Godot;
using System;

namespace ZombieShooter;
public partial class Main : Node2D
{
	private BulletManager _bulletManager;
	private Player _player;
	private Weapon _weapon;
	private Gui _gui;
	
	public override void _Ready()
	{
		_bulletManager = GetNode<BulletManager>("BulletManager");
		_player = GetNode<Player>("Player");
		_gui = GetNode<Gui>("GUI");
		_weapon = GetNode<Weapon>("Player/Weapon");

		_weapon.AmmoAmountChanged += _gui.SetCurrentAmmo;
		_weapon.MaxAmmoAmountChanged += _gui.SetMaxAmmo;
		_weapon.PlayerFiredBullet += _bulletManager.HandleBulletSpawned;
		
		_player.PlayerHealthChanged += _gui.SetHealth;

		GameManager.Instance.MoneyChanged += _gui.SetCurrentMoneyAmount;
		GameManager.Instance.ActionLabelChanged += _gui.SetActionLabel;
		
		_gui.SetCurrentAmmo(_weapon.GetCurrentAmmo());
		_gui.SetMaxAmmo(_weapon.GetMaxAmmo());
	}

	public override void _Process(double delta)
	{
	}
}
