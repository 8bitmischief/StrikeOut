using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public interface IHittable
	{
		void OnHit(Entity entity, Hitbox hitbox, Hurtbox hurtbox);
	}
}
