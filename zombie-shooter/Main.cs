using Godot;
using System;

namespace ZombieShooter;
public partial class Main : Node2D
{
	private BulletManager _bulletManager;
	private Player _player;
	
	public override void _Ready()
	{
		_bulletManager = GetNode<BulletManager>("BulletManager");
		_player = GetNode<Player>("Player");

		_player.PlayerFiredBullet += _bulletManager.HandleBulletSpawned;
	}

	public override void _Process(double delta)
	{
	}
}
