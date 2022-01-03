using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IEnemyHittable
	{
		void OnHit(Entity entity, EnemyHitbox hitbox, BatterHurtbox hurtbox);
	}
}