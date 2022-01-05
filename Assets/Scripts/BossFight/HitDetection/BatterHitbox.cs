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

		public event Action<BatterHitRecord> onHit;

		private void Start()
		{
			if (entity is IBatterHittable)
				_hittableEntity = entity as IBatterHittable;
		}

		public BatterHitRecord CheckForHit(EnemyHurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox))
			{
				StrikeZone strikeZone;
				if (_strikeZone == StrikeZone.West && entity.transform.localScale.x < 0f)
					strikeZone = StrikeZone.East;
				else if (_strikeZone == StrikeZone.East && entity.transform.localScale.x < 0f)
					strikeZone = StrikeZone.West;
				else
					strikeZone = _strikeZone;
				BatterHitResult result = hurtbox.GetHitResult(strikeZone);
				if (result != BatterHitResult.None && result != BatterHitResult.Miss)
				{
					ReusedHitRecord.hitter = entity;
					ReusedHitRecord.hurtee = hurtbox.entity;
					ReusedHitRecord.hitbox = this;
					ReusedHitRecord.hurtbox = hurtbox;
					ReusedHitRecord.strikeZone = strikeZone;
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

		protected override void OnActivated()
		{
			base.OnActivated();
			Scene.I.hitDetectionManager.RegisterHitbox(this);
		}

		protected override void OnDeactivated()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHitbox(this);
		}
	}
}