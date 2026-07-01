using System.Diagnostics;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace PaelCharacter.PaelCharacterCode.Nodes;

[GlobalClass]
public partial class PaelCreatureVisuals : NCreatureVisuals
{
	private Node2D? _goldNode;
	private float _targetGoldScale = 1f;
	private decimal _lastGold = -1;

	private Node2D? _characterNode;
	private double _totalElapsedTime = 0.0;

	private List<Node2D> _dripSpawnPoints = new();
	private Sprite2D? _dripTexture;
	private double _lastDripTime = 0.0;
	private double _timeToNextDrip = 0.0;
	private Vector2 _dripOrigin;
	
	public override void _Ready()
	{
		base._Ready();

		_goldNode = GetNodeOrNull<Node2D>("%Gold");
		_characterNode = GetNodeOrNull<Node2D>("%Visuals");
		
		_dripSpawnPoints.Add(GetNodeOrNull<Node2D>("%DripSpawnPoint0"));
		_dripSpawnPoints.Add(GetNodeOrNull<Node2D>("%DripSpawnPoint1"));
		_dripSpawnPoints.Add(GetNodeOrNull<Node2D>("%DripSpawnPoint2"));
		
		_dripTexture = GetNodeOrNull<Sprite2D>("%DripTexture");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		_totalElapsedTime += delta;

		if (_totalElapsedTime - _lastDripTime >= _timeToNextDrip)
		{
			NewDrip();
		}
		
		UpdateGoldAmountFromPlayer();
		AnimateGoldPile(delta);
		AnimateCharacterScale();
		AnimateDrip();
	}

	private void UpdateGoldAmountFromPlayer()
	{
		Player? player = TryGetPlayer();

		if (player == null) return;

		decimal currentGold = player.Gold;

		if (currentGold == _lastGold) return;

		_lastGold = currentGold;
		_targetGoldScale = GetGoldPileScale(currentGold);
	}

	private void AnimateGoldPile(double delta)
	{
		if (_goldNode == null) return;

		var currentScale = _goldNode.Scale.X;
		var nextScale = Mathf.Lerp(currentScale, _targetGoldScale, (float)delta * 8f);

		_goldNode.Scale = new Vector2(nextScale, nextScale);
	}

	private void AnimateCharacterScale()
	{
		if (_characterNode == null) return;

		const float originalScale = 0.235F;
		const float breathPeriod = 5.0F;
		const float breathAmplitude = 0.03F;
		
		var newScaleX = originalScale * (1.0F + ((breathAmplitude / 2F) * MathF.Sin((float)_totalElapsedTime / breathPeriod)));
		var newScaleY = originalScale * (1.0F + (breathAmplitude * MathF.Sin((float)_totalElapsedTime / breathPeriod)));
		
		_characterNode.Scale = new Vector2(newScaleX, newScaleY);
	}

	private Player? TryGetPlayer()
	{
		if (GetParent() is not NCreature nCreature) return null;

		if (!nCreature.Entity.IsPlayer) return null;

		return nCreature.Entity.Player;
	}

	private static float GetGoldPileScale(decimal gold)
	{
		const float minScale = 0.35f;
		const float maxScale = 5f;
		const float goldForMaxScale = 900f;

		var t = Math.Clamp((float)gold / goldForMaxScale, 0f, 1f);
		t = MathF.Sqrt(t);

		return Mathf.Lerp(minScale, maxScale, t);
	}

	private void NewDrip()
	{
		if (_dripTexture == null) return;
		
		const double minDripInterval = 6.0;
		const double maxDripInterval = 15.0;
		
		_lastDripTime = _totalElapsedTime;
		_timeToNextDrip = minDripInterval + (maxDripInterval - minDripInterval) * Random.Shared.NextDouble();
		
		var dripSpawnPointIndex = (int)Math.Floor(Random.Shared.NextDouble() * _dripSpawnPoints.Count);
		_dripOrigin = _dripSpawnPoints[dripSpawnPointIndex].GlobalPosition;

		_dripTexture.GlobalPosition = _dripOrigin;
		_dripTexture.Rotation = 0.1F * (float)(Random.Shared.NextDouble() * 2 - 1);
		_dripTexture.Visible = true;
		
	}

	private void AnimateDrip()
	{
		if (_dripTexture == null || _characterNode == null) return;

		const double dropletFormationTime = 1.0;

		if (_totalElapsedTime - _lastDripTime <= dropletFormationTime)
		{
			_dripTexture.Offset = new Vector2(-18, 0);
			_dripTexture.GlobalPosition = _dripOrigin;
			_dripTexture.Scale = new Vector2(1, (float)((_totalElapsedTime - _lastDripTime) / dropletFormationTime));

			return;
		}
		
		float fallingTime = (float)(_totalElapsedTime - _lastDripTime - dropletFormationTime);
		float dripOpacity = Math.Clamp(1F - (fallingTime - 2F), 0F, 1F);
		
		_dripTexture.Modulate = new Color(1F, 1F, 1F, dripOpacity);

		if (dripOpacity <= 0F) return;
		
		const float dripAcceleration = 300.0F;
		float groundHeight = _characterNode.GlobalPosition.Y - 30F;
		
		var dripOffsetY = dripAcceleration * fallingTime * fallingTime;
		
		var newPosition = _dripOrigin + new Vector2(0, dripOffsetY);
		
		if (newPosition.Y > groundHeight)
		{
			_dripTexture.Offset = new Vector2(-18, -110);
			var squashFactor = (float)Math.Pow(newPosition.Y - groundHeight, 0.1F);
			_dripTexture.Scale = new Vector2(1 + squashFactor, 1 / (1 + squashFactor));
			newPosition.Y = groundHeight + (0.235F * 110);
		}
		else
		{
			_dripTexture.Offset = new Vector2(-18, 0);
			_dripTexture.Scale = Vector2.One;
		}

		_dripTexture.GlobalPosition = newPosition;
	}
}
