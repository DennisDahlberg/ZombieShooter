using Godot;
using System;
using System.Collections.Generic;

namespace ZombieShooter;
public partial class ZombieManager : Node2D
{
	[Export] public PackedScene ZombieScene;
	[Export] public float BaseSpawnDelay = 3.0f;
    
	private Timer _spawnTimer;
	private Node2D _spawnPointsContainer;
	private List<Marker2D> _spawnPoints = [];
    
	private int _currentWave = 1;
	private float _difficultyMultiplier = 1.0f;

	public override void _Ready()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnPointsContainer = GetNode<Node2D>("../SpawnPoints"); 
        GD.Print(_spawnPointsContainer);
		foreach (Node child in _spawnPointsContainer.GetChildren())
		{
			if (child is Marker2D marker) _spawnPoints.Add(marker);
		}

		_spawnTimer.WaitTime = BaseSpawnDelay;
		_spawnTimer.Start();
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
	}

	private void OnSpawnTimerTimeout()
	{
		SpawnZombie();
	}

	private void SpawnZombie()
	{
		if (_spawnPoints.Count == 0) return;

		var random = new Random();
		int index = random.Next(_spawnPoints.Count);
		Vector2 spawnPos = _spawnPoints[index].GlobalPosition;

		var zombie = (Zombie)ZombieScene.Instantiate();
		zombie.GlobalPosition = spawnPos;
        
		zombie.Speed += (_currentWave * 5);


		AddChild(zombie);
	}
    
	public void NextWave()
	{
		_currentWave++;
		_difficultyMultiplier += 0.2f;
		_spawnTimer.WaitTime = Math.Max(0.5f, BaseSpawnDelay - (_currentWave * 0.1f));
	}
}
