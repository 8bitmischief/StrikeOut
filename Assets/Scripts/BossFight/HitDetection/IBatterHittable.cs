using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IBatterHittable
	{
		void OnHit(Entity entity, BatterHitbox hitbox, EnemyHurtbox hurtbox);
	}
}