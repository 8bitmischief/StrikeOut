using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHitRecord : HitRecord
	{
		public BatterHitbox hitbox;
		public EnemyHurtbox hurtbox;
		public StrikeZone strikeZone;
		public BatterHitResult result;
	}
}