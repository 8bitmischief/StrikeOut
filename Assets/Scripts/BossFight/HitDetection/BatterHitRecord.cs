using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterHitRecord
	{
		public Entity hitter;
		public Entity hurtee;
		public BatterHitbox hitbox;
		public EnemyHurtbox hurtbox;
		public StrikeZone strikeZone;
		public BatterHitResult result;
	}
}