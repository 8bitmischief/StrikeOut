using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class EnemyHitRecord
	{
		public Entity hitter;
		public Entity hurtee;
		public EnemyHitbox hitbox;
		public BatterHurtbox hurtbox;
		public BatterArea area;
	}
}