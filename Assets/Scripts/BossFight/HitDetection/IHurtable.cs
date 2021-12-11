using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public interface IHurtable
	{
		void OnHurt(Entity entity, Hitbox hitbox, Hurtbox hurtbox);
	}
}
