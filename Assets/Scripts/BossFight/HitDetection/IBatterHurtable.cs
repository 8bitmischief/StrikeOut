using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IBatterHurtable
	{
		void OnHurt(Entity entity, EnemyHitbox hitbox, BatterHurtbox hurtbox);
	}
}