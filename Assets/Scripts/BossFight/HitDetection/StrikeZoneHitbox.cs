using UnityEngine;

namespace StrikeOut.BossFight
{
	public class StrikeZoneHitbox : Hitbox
	{
		[Header("Strike Zones")]
		[SerializeField] private bool _hitsNorth;
		[SerializeField] private bool _hitsEast;
		[SerializeField] private bool _hitsSouth;
		[SerializeField] private bool _hitsWest;

		public bool hitsNorth => _hitsNorth;
		public bool hitsEast => entity.transform.localScale.x >= 0f ? _hitsEast : _hitsWest;
		public bool hitsSouth => _hitsSouth;
		public bool hitsWest => entity.transform.localScale.x >= 0f ? _hitsWest : _hitsEast;

		public override bool IsHitting(Hurtbox hurtbox)
		{
			if (base.IsHitting(hurtbox) && hurtbox is StrikeZoneHurtbox)
			{
				StrikeZoneHurtbox strikeZoneHurtbox = hurtbox as StrikeZoneHurtbox;
				return hitsNorth && strikeZoneHurtbox.hitByNorth ||
					hitsEast && strikeZoneHurtbox.hitByEast ||
					hitsSouth && strikeZoneHurtbox.hitBySouth ||
					hitsWest && strikeZoneHurtbox.hitByWest;
			}
			else
			{
				return false;
			}
		}

	}
}