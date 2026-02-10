using Godot;
using System;

namespace ZombieShooter;
public partial class Gui : CanvasLayer
{
	private ProgressBar _healthBar;
	private Label _currentAmmo;
	private Label _maxAmmo;
	private Label _playerMoney;
	private Label _actionLabel;
	private HBoxContainer _perkContainer;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("MarginContainer/Rows/BottomRow/CenterContainer/BottomLeftSection/Top/HealthBar");
		_currentAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/VBoxContainer/AmmoSection/CurrentAmmo");
		_maxAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/VBoxContainer/AmmoSection/MaxAmmo");
		_playerMoney = GetNode<Label>("MarginContainer/Rows/BottomRow/CenterContainer/BottomLeftSection/Bottom/PlayerMoney");
		_actionLabel = GetNode<Label>("MarginContainer/Rows/MiddleRow/MiddleTop/CenterContainer/ActionLabel");
		_perkContainer = GetNode<HBoxContainer>("MarginContainer/Rows/BottomRow/PerkSection");

		_healthBar.MaxValue = 60;
		_healthBar.Value = 60;
		_playerMoney.Text = "500";
	}

	public void SetMaxHealth(int newMaxHealth)
	{
		_healthBar.MaxValue = newMaxHealth;
		SetHealth(newMaxHealth);
	}

	public void ResetHealth()
	{
		SetHealth((int)_healthBar.MaxValue);
	}

	public void SetHealth(int newHealth)
	{
		Color originalHealthColor = Color.FromHtml("#5c1c1c");
		Color highlightColor = Color.FromHtml("#ff7e7e");
		float shakeIntensity = 5.0f;
		Vector2 originalPos = _healthBar.Position;
		
		var barStyle = _healthBar.GetThemeStylebox("fill") as StyleBoxFlat;
		
		var tween = CreateTween();
		tween.SetParallel(true);
		
		tween.TweenProperty(barStyle, "bg_color", highlightColor, 0.1f);
		tween.Chain().TweenProperty(barStyle, "bg_color", originalHealthColor, 0.3f);
		
		tween.TweenProperty(_healthBar, "value", newHealth, 0.4f);
		
		var shakeTween = CreateTween();
		for (int i = 0; i < 6; i++)
		{
			var offset = new Vector2(
				(float)GD.RandRange(-shakeIntensity, shakeIntensity),
				(float)GD.RandRange(-shakeIntensity, shakeIntensity)
			);
			shakeTween.TweenProperty(_healthBar, "position", originalPos + offset, 0.05f);
		}
		shakeTween.TweenProperty(_healthBar, "position",  originalPos, 0.05f);
	}

	public void SetMaxAmmo(int newMaxAmmo)
	{
		_maxAmmo.Text = newMaxAmmo.ToString();
	}

	public void SetCurrentAmmo(int newCurrentAmmo)
	{
		_currentAmmo.Text = newCurrentAmmo.ToString();
	}

	public void SetCurrentMoneyAmount(int newMoneyAmount)
	{
		_playerMoney.Text = newMoneyAmount.ToString();
	}
	
	public void SetActionLabel(string newActionLabel)
	{
		_actionLabel.Text = newActionLabel;
	}

	public void AddPerkIcon(string perkName)
	{
		TextureRect icon = new TextureRect();
    
		var fullSheet = GD.Load<Texture2D>("res://main_assets/perks/perk_icons_tp.png");
    
		AtlasTexture atlas = new AtlasTexture();
		atlas.Atlas = fullSheet;
    
		Rect2 region = GetRegionForPerk(perkName); 
		atlas.Region = region;

		icon.Texture = atlas;
		icon.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		icon.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
		icon.CustomMinimumSize = new Vector2(64, 64);
    
		_perkContainer.AddChild(icon);
	}
	
	private Rect2 GetRegionForPerk(string perkId)
	{
		return perkId switch
		{
			"StaminaUp" => new Rect2(1028, 1017, 471, 484),
			"QuickRevive" => new Rect2(194, 1016, 477, 487),
			"DoubleTap" => new Rect2(1026, 203, 477, 477),
			"Juggernog" => new Rect2(194, 203, 477, 477),
			"SpeedCola" => new Rect2(1860, 203, 477, 477),
			"ElectricCherry" => new Rect2(1857, 1015, 480, 487),
			_ => new Rect2(0, 0, 64, 64)
		};
	}

	public void ClearPerkIcons()
	{
		foreach (Node child in _perkContainer.GetChildren())
		{
			child.QueueFree();
		}
	}
	
}
