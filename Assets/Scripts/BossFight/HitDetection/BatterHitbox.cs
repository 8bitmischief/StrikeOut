using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHitbox : Hitbox
	{
		private static BatterHitRecord ReusedHitRecord = new BatterHitRecord();

		[SerializeField] private StrikeZone _strikeZone;
		private IBatterHittable _hittableEntity;
		private IBatterPredictedHittable _predictedHittableEntity;

		public event Action<BatterHitRecord> onHit;
		public event Action<BatterHitRecord, int> onPredictedHit;

		private void Start()
		{
			if (entity is IBatterHittable)
				_hittableEntity = entity as IBatterHittable;
			if (entity is IBatterPredictedHittable)
				_predictedHittableEntity = entity as IBatterPredictedHittable;
		}

		public bool DoesHit(StrikeZone strikeZone)
		{
			return strikeZone == GetHitStrikeZone();
		}

		public bool WillHit(StrikeZone strikeZone, int startFrame, int endFrame = -1)
		{
			return DoesHit(strikeZone) && WillBeActive(startFrame, endFrame);
		}

		public BatterHitRecord CheckForHit(EnemyHurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox))
			{
				StrikeZone hitStrikeZone = GetHitStrikeZone();
				BatterHitResult result = hurtbox.GetHitResult(hitStrikeZone);
				if (result != BatterHitResult.None && result != BatterHitResult.Miss)
				{
					ReusedHitRecord.hitter = entity;
					ReusedHitRecord.hurtee = hurtbox.entity;
					ReusedHitRecord.hitbox = this;
					ReusedHitRecord.hurtbox = hurtbox;
					ReusedHitRecord.strikeZone = hitStrikeZone;
					ReusedHitRecord.result = result;
					return ReusedHitRecord;
				}
				else
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		public void OnHit(BatterHitRecord hit)
		{
			base.OnHit(hit.hurtbox);
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hit);
			onHit?.Invoke(hit);
		}

		public void OnPredictedHit(BatterHitRecord hit, int frames)
		{
			if (_predictedHittableEntity != null)
				_predictedHittableEntity.OnPredictedHit(hit, frames);
			onPredictedHit?.Invoke(hit, frames);
		}

		protected override void Register()
		{
			base.Register();
			Scene.I.hitDetectionManager.RegisterHitbox(this);
		}

		protected override void Unregister()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHitbox(this);
		}

		private StrikeZone GetHitStrikeZone()
		{
			if (_strikeZone == StrikeZone.West && entity.transform.localScale.x < 0f)
				return StrikeZone.East;
			else if (_strikeZone == StrikeZone.East && entity.transform.localScale.x < 0f)
				return StrikeZone.West;
			else
				return _strikeZone;
		}
	}
}