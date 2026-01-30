using Godot;
using System;   

namespace ZombieShooter;
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 400.0f;
    [Export] public PackedScene Bullet;

    [Signal]
    public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance, Vector2 position,  Vector2 direction);
    
    private Marker2D _endOfGun;
    private Marker2D _gunDirection;
    private Timer _attackCooldown;
    private AnimationPlayer _animation;
    
    public override void _Ready()
    {
        _endOfGun = GetNode<Marker2D>("EndOfGun");
        _gunDirection = GetNode<Marker2D>("GunDirection");
        _attackCooldown = GetNode<Timer>("AttackCooldown");
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        
        _animation.Stop();
        GetNode<Sprite2D>("MuzzleFlash").Hide();
    }

    public override void _PhysicsProcess(double delta)
    {
        LookAt(GetGlobalMousePosition());

        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = direction * Speed;

        MoveAndSlide();

        if (Input.IsActionJustPressed("shoot"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_attackCooldown.IsStopped())
        {
            return;
        }
        var bullet = (Bullet)Bullet.Instantiate();
        var target = GetGlobalMousePosition();
        var directionToMouse = _gunDirection.GlobalPosition - _endOfGun.GlobalPosition;
        EmitSignalPlayerFiredBullet(bullet, _endOfGun.GlobalPosition, directionToMouse);
        _attackCooldown.Start();
        _animation.Play("muzzle_flash");
    }
}