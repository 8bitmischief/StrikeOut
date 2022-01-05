using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHitbox : Hitbox
	{
		private static EnemyHitRecord ReusedHitRecord = new EnemyHitRecord();

		[Header("Areas")]
		[SerializeField] private bool _hitsFarLeft;
		[SerializeField] private bool _hitsLeft;
		[SerializeField] private bool _hitsCenter;
		[SerializeField] private bool _hitsRight;
		[SerializeField] private bool _hitsFarRight;
		[Header("Relative Areas")]
		[SerializeField] private bool _hitsFarSameSide;
		[SerializeField] private bool _hitsSameSide;
		[SerializeField] private bool _hitsOppositeSide;
		[SerializeField] private bool _hitsFarOppositeSide;
		private IEnemyHittable _hittableEntity;
		private IEnemyPredictedHittable _predictedHittableEntity;
		private bool _isOnRightSide;

		public bool hitsFarLeft => (entity.transform.localScale.x >= 0f ? _hitsFarLeft : _hitsFarRight) || (_isOnRightSide ? _hitsFarOppositeSide : _hitsFarSameSide);
		public bool hitsLeft => (entity.transform.localScale.x >= 0f ? _hitsLeft : _hitsRight) || (_isOnRightSide ? _hitsOppositeSide : _hitsSameSide);
		public bool hitsCenter => _hitsCenter;
		public bool hitsRight => (entity.transform.localScale.x >= 0f ? _hitsRight : _hitsLeft) || (_isOnRightSide ? _hitsSameSide : _hitsOppositeSide);
		public bool hitsFarRight => (entity.transform.localScale.x >= 0f ? _hitsFarRight : _hitsFarLeft) || (_isOnRightSide ? _hitsFarSameSide : _hitsFarOppositeSide);

		public event Action<EnemyHitRecord> onHit;
		public event Action<EnemyHitRecord, int> onPredictedHit;

		private void Start()
		{
			if (entity is IEnemyHittable)
				_hittableEntity = entity as IEnemyHittable;
			if (entity is IEnemyPredictedHittable)
				_predictedHittableEntity = entity as IEnemyPredictedHittable;
		}

		public bool DoesHit(BatterArea area)
		{
			switch (area)
			{
				case BatterArea.FarLeft: return hitsFarLeft;
				case BatterArea.Left: return hitsLeft;
				case BatterArea.Center: return hitsCenter;
				case BatterArea.Right: return hitsRight;
				case BatterArea.FarRight: return hitsFarRight;
				default: return false;
			}
		}

		public bool WillHit(BatterArea area, int startFrame, int endFrame = -1)
		{
			return DoesHit(area) && WillBeActive(startFrame, endFrame);
		}

		public EnemyHitRecord CheckForHit(BatterHurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox))
			{
				if (DoesHit(hurtbox.area) &&
					(hurtbox.destinationArea == BatterArea.None || DoesHit(hurtbox.destinationArea)))
				{
					ReusedHitRecord.hitter = entity;
					ReusedHitRecord.hurtee = hurtbox.entity;
					ReusedHitRecord.hitbox = this;
					ReusedHitRecord.hurtbox = hurtbox;
					ReusedHitRecord.area = hurtbox.area;
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

		public void OnHit(EnemyHitRecord hit)
		{
			base.OnHit(hit.hurtbox);
			if (_hittableEntity != null)
				_hittableEntity.OnHit(hit);
			onHit?.Invoke(hit);
		}

		public void OnPredictedHit(EnemyHitRecord hit, int frames)
		{
			if (_predictedHittableEntity != null)
				_predictedHittableEntity.OnPredictedHit(hit, frames);
			onPredictedHit?.Invoke(hit, frames);
		}

		protected override void Register()
		{
			base.Register();
			_isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
			Scene.I.hitDetectionManager.RegisterHitbox(this);
		}

		protected override void Unregister()
		{
			if (Scene.hasInstance)
				Scene.I.hitDetectionManager.UnregisterHitbox(this);
		}
	}
}