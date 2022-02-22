using UnityEngine;

namespace StrikeOut.BossFight
{
	public enum BatterHitResult
	{
		None = 0,
		Miss = 1,
		Damage = 2,
		CriticalDamage = 3,
		Ball = 4,
		Parry = 5,
		Armor = 6
	}
}