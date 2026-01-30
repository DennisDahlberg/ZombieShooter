using Godot;
using System;   

namespace ZombieShooter;
public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 400.0f;
    [Export] public PackedScene Bullet;

    [Signal]
    public delegate void PlayerFiredBulletEventHandler(Bullet bulletInstance);
    
    private Marker2D _endOfGun;

    public override void _Ready()
    {
        _endOfGun = GetNode<Marker2D>("EndOfGun");
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
        var bullet = (Bullet)Bullet.Instantiate();
        AddChild(bullet);
        bullet.GlobalPosition = _endOfGun.GlobalPosition;
        Vector2 target = GetGlobalMousePosition();
        Vector2 directionToMouse = bullet.GlobalPosition.DirectionTo(target).Normalized();
        bullet.SetDirection(directionToMouse);
        EmitSignalPlayerFiredBullet(bullet);
        GD.Print("Pew!"); 
    }
}