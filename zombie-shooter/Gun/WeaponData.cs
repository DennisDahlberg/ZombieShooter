using Godot;

namespace ZombieShooter.Gun;

[GlobalClass]
public partial class WeaponData : Resource
{
    [Export] public string Name;
    [Export] public PackedScene Bullet;
    [Export] public int MaxAmmo = 30;
    [Export] public int CurrentAmmo = 30;
    [Export] public float AttackCooldown = 0.2f;
    [Export] public float ReloadCooldown = 1.0f;
    [Export] public string ShootAnimation = "muzzle_flash";
    [Export] public Texture2D Icon;
}