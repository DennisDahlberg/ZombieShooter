using Godot;
using System;   

namespace ZombieShooter;
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 400.0f;
    [Export] public PackedScene Bullet;
    
    [Signal] public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
    [Signal] public delegate void PlayerHealthChangedEventHandler(int newHealth);
    [Signal] public delegate void PlayerMaxHealthChangedEventHandler(int newHealth);
    
    private Marker2D _endOfGun;
    private Marker2D _gunDirection;
    private Timer _attackCooldown;
    private AnimationPlayer _animation;
    private Vector2 _knockbackVelocity = Vector2.Zero;
    
    public Weapon Weapon;
    
    private int _currentHealth = 60;
    
    public override void _Ready()
    {
        Weapon = GetNode<Weapon>("Weapon");
        
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
            Weapon.Shoot();
        
        if (Input.IsActionJustPressed("reload"))
            Weapon.Reload();
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

    public void SetMaxHealth(int newMaxHealth)
    {
        _currentHealth = newMaxHealth;
        EmitSignalPlayerMaxHealthChanged(_currentHealth);
    }

    private void Die()
    {
        GD.Print("Game Over!");
        GetTree().ReloadCurrentScene();
    }
}