using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHitbox : Hitbox
	{
		[SerializeField] private bool _hitsFarSameSide = false;
		[SerializeField] private bool _hitsSameSide = true;
		[SerializeField] private bool _hitsCenter = false;
		[SerializeField] private bool _hitsOppositeSide = false;
		[SerializeField] private bool _hitsFarOppositeSide = false;
		private bool _isOnRightSide;

		protected override void OnEnable()
		{
			base.OnEnable();
			_isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
		}

		public override bool CanHit(Hurtbox hurtbox)
		{
			if (base.CanHit(hurtbox))
			{
				if (hurtbox.entity == Scene.I.entityManager.batter)
				{
					return DoesHitArea(Scene.I.entityManager.batter.area) &&
						(Scene.I.entityManager.batter.destinationArea == BatterArea.None || DoesHitArea(Scene.I.entityManager.batter.destinationArea));
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		private bool DoesHitArea(BatterArea area)
		{
			switch (area)
			{
				case BatterArea.FarLeft: return (_isOnRightSide && _hitsFarOppositeSide) || (!_isOnRightSide && _hitsFarSameSide);
				case BatterArea.FarRight: return (!_isOnRightSide && _hitsFarOppositeSide) || (_isOnRightSide && _hitsFarSameSide);
				case BatterArea.Left: return (_isOnRightSide && _hitsOppositeSide) || (!_isOnRightSide && _hitsSameSide);
				case BatterArea.Right: return (!_isOnRightSide && _hitsOppositeSide) || (_isOnRightSide && _hitsSameSide);
				case BatterArea.Center: return _hitsCenter;
				default: return false;
			}
		}
	}
}