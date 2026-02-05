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

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("MarginContainer/Rows/BottomRow/CenterContainer/BottomLeftSection/Top/HealthBar");
		_currentAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/AmmoSection/CurrentAmmo");
		_maxAmmo = GetNode<Label>("MarginContainer/Rows/BottomRow/AmmoSection/MaxAmmo");
		_playerMoney = GetNode<Label>("MarginContainer/Rows/BottomRow/CenterContainer/BottomLeftSection/Bottom/PlayerMoney");
		_actionLabel = GetNode<Label>("MarginContainer/Rows/MiddleRow/MiddleTop/CenterContainer/ActionLabel");

		_healthBar.Value = 100;
		_playerMoney.Text = "500";
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
}
