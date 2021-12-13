using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IHittable
	{
		void OnHit(Entity entity, Hitbox hitbox, Hurtbox hurtbox);
	}
}
