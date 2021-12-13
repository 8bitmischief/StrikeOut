using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterAreaHitbox : Hitbox
	{
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
		private bool _isOnRightSide;

		public bool hitsFarLeft => (entity.transform.localScale.x >= 0f ? _hitsFarLeft : _hitsFarRight) || (_isOnRightSide ? _hitsFarOppositeSide : _hitsFarSameSide);
		public bool hitsLeft => (entity.transform.localScale.x >= 0f ? _hitsLeft : _hitsRight) || (_isOnRightSide ? _hitsOppositeSide : _hitsSameSide);
		public bool hitsCenter => _hitsCenter;
		public bool hitsRight => (entity.transform.localScale.x >= 0f ? _hitsRight : _hitsLeft) || (_isOnRightSide ? _hitsSameSide : _hitsOppositeSide);
		public bool hitsFarRight => (entity.transform.localScale.x >= 0f ? _hitsFarRight : _hitsFarLeft) || (_isOnRightSide ? _hitsFarSameSide : _hitsFarOppositeSide);

		protected override void OnEnable()
		{
			base.OnEnable();
			_isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
		}

		public override bool IsHitting(Hurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox) && hurtbox is BatterAreaHurtbox)
			{
				BatterAreaHurtbox batterAreaHurtbox = hurtbox as BatterAreaHurtbox;
				return DoesHitArea(batterAreaHurtbox.area) &&
					(batterAreaHurtbox.destinationArea == BatterArea.None || DoesHitArea(batterAreaHurtbox.destinationArea));
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
				case BatterArea.FarLeft: return hitsFarLeft;
				case BatterArea.Left: return hitsLeft;
				case BatterArea.Center: return hitsCenter;
				case BatterArea.Right: return hitsRight;
				case BatterArea.FarRight: return hitsFarRight;
				default: return false;
			}
		}
	}
}