using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IEnemyHurtable
	{
		void OnHurt(Entity entity, BatterHitbox hitbox, EnemyHurtbox hurtbox);
	}
}