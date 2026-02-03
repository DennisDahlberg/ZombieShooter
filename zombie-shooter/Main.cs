using Godot;
using System;

namespace ZombieShooter;
public partial class Main : Node2D
{
	private BulletManager _bulletManager;
	private Player _player;
	private CanvasLayer _gui;
	
	public override void _Ready()
	{
		_bulletManager = GetNode<BulletManager>("BulletManager");
		_player = GetNode<Player>("Player");
		_gui = GetNode<CanvasLayer>("GUI");

		_player.PlayerFiredBullet += _bulletManager.HandleBulletSpawned;
	}

	public override void _Process(double delta)
	{
	}
}
