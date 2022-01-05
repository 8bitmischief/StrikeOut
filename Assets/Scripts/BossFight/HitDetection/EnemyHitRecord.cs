using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHitRecord : HitRecord
	{
		public EnemyHitbox hitbox;
		public BatterHurtbox hurtbox;
		public BatterArea area;
	}
}