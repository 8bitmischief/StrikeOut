using System;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHurtbox : Hurtbox
	{
		[Header("Areas")]
		[SerializeField] private RelativeBatterArea _area;
		[SerializeField] private RelativeBatterArea _destinationArea;
		private IBatterHurtable _hurtableEntity;
		private IBatterPredictedHurtable _predictedHurtableEntity;

		public BatterArea area
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_area)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}
		public BatterArea destinationArea
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_destinationArea)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}

		public event Action<EnemyHitRecord> onHurt;
		public event Action<EnemyHitRecord, int> onPredictedHurt;

		private void Awake()
		{
			if (entity is IBatterHurtable)
				_hurtableEntity = entity as IBatterHurtable;
			if (entity is IBatterPredictedHurtable)
				_predictedHurtableEntity = entity as IBatterPredictedHurtable;
		}

		public bool IsHurtBy(BatterArea area)
		{
			return this.area == area;
		}

		public bool WillBeHurtBy(BatterArea area, int frames)
		{
			return IsHurtBy(area) && WillBeActive(frames);
		}

		public void OnHurt(EnemyHitRecord hit)
		{
			if (_hurtableEntity != null)
				_hurtableEntity.OnHurt(hit);
			onHurt?.Invoke(hit);
		}

		public void OnPredictedHurt(EnemyHitRecord hit, int frames)
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