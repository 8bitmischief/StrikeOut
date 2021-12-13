using UnityEngine;

namespace StrikeOut.BossFight
{
	public class StrikeZoneHurtbox : Hurtbox
	{
		[Header("Strike Zones")]
		[SerializeField] private bool _hitByNorth;
		[SerializeField] private bool _hitByEast;
		[SerializeField] private bool _hitBySouth;
		[SerializeField] private bool _hitByWest;

		public bool hitByNorth => _hitByNorth;
		public bool hitByEast => entity.transform.localScale.x >= 0f ? _hitByEast : _hitByWest;
		public bool hitBySouth => _hitBySouth;
		public bool hitByWest => entity.transform.localScale.x >= 0f ? _hitByWest : _hitByEast;
	}
}