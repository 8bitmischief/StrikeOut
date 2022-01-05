using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHurtbox : Hurtbox
	{
		[Header("Single Strike Zone")]
		[SerializeField] private StrikeZone _strikeZone;
		[SerializeField] private BatterHitResult _result;
		[Header("Multi Strike Zone")]
		[SerializeField] private BatterHitResult _north;
		[SerializeField] private BatterHitResult _east;
		[SerializeField] private BatterHitResult _south;
		[SerializeField] private BatterHitResult _west;
		private IEnemyHurtable _hurtableEntity;
		private IEnemyPredictedHurtable _predictedHurtableEntity;

		public StrikeZone strikeZone { get => _strikeZone; set => _strikeZone = value; }

		public event Action<BatterHitRecord> onHurt;
		public event Action<BatterHitRecord, int> onPredictedHurt;

		private void Awake()
		{
			if (entity is IEnemyHurtable)
				_hurtableEntity = entity as IEnemyHurtable;
			if (entity is IEnemyPredictedHurtable)
				_predictedHurtableEntity = entity as IEnemyPredictedHurtable;
		}

		public bool IsHurtBy(StrikeZone strikeZone)
		{
			BatterHitResult result = GetHitResult(strikeZone);
			return result == BatterHitResult.Damage || result == BatterHitResult.Ball || result == BatterHitResult.Parry;
		}

		public bool WillBeHurtBy(StrikeZone strikeZone, int startFrame, int endFrame = -1)
		{
			return IsHurtBy(strikeZone) && WillBeActive(startFrame, endFrame);
		}

		public BatterHitResult GetHitResult(StrikeZone strikeZone)
		{
			StrikeZone singleStrikeZone;
			if (entity.transform.localScale.x < 0 && _strikeZone == StrikeZone.East)
				singleStrikeZone = StrikeZone.West;
			else if (entity.transform.localScale.x < 0 && _strikeZone == StrikeZone.West)
				singleStrikeZone = StrikeZone.East;
			else
				singleStrikeZone = _strikeZone;
			if (singleStrikeZone != StrikeZone.None && strikeZone == singleStrikeZone)
			{
				return _result;
			}
			else
			{
				switch (strikeZone)
				{
					case StrikeZone.North: return _north;
					case StrikeZone.East: return entity.transform.localScale.x >= 0f ? _east : _west;
					case StrikeZone.South: return _south;
					case StrikeZone.West: return entity.transform.localScale.x >= 0f ? _west : _east;
					default: return BatterHitResult.None;
				}
			}
		}

		public void OnHurt(BatterHitRecord hit)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(hit);
			onHurt?.Invoke(hit);
		}

		public void OnPredictedHurt(BatterHitRecord hit, int frames)
		{
			if (_predictedHurtableEntity != null)
				_predictedHurtableEntity.OnPredictedHurt(hit, frames);
			onPredictedHurt?.Invoke(hit, frames);
		}

		protected override void Register()
		{
			Scene.I.hitDetectionManager.RegisterHurtbox(this);
		}

		protected override void Unregister()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHurtbox(this);
		}
	}
}