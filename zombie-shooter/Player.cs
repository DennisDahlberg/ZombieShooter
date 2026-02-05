using Godot;
using System;   

namespace ZombieShooter;
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 400.0f;
    [Export] public PackedScene Bullet;
    
    [Signal] public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
    [Signal] public delegate void PlayerHealthChangedEventHandler(int newHealth);
    
    
    
    private Marker2D _endOfGun;
    private Marker2D _gunDirection;
    private Timer _attackCooldown;
    private AnimationPlayer _animation;
    private Vector2 _knockbackVelocity = Vector2.Zero;
    private Weapon _weapon;
    
    private int _currentHealth = 100;
    
    public override void _Ready()
    {
        _weapon = GetNode<Weapon>("Weapon");
        
        _animation.Stop();
        GetNode<Sprite2D>("MuzzleFlash").Hide();
    }

    public override void _PhysicsProcess(double delta)
    {
        LookAt(GetGlobalMousePosition());

        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = (direction * Speed) + _knockbackVelocity;;
        
        _knockbackVelocity = _knockbackVelocity.Lerp(Vector2.Zero, 0.1f);

        MoveAndSlide();

        if (Input.IsActionJustPressed("shoot"))
            _weapon.Shoot();
        
        if (Input.IsActionJustPressed("reload"))
            _weapon.Reload();
    }

    public void ApplyKnockback(Vector2 direction, float strength)
    {
        _knockbackVelocity = direction * strength;
    }
    
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        GD.Print($"Player Health: {_currentHealth}");
        
        EmitSignalPlayerHealthChanged(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GD.Print("Game Over!");
        GetTree().ReloadCurrentScene();
    }
}