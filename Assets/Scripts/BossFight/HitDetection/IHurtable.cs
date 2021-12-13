using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public interface IHurtable
	{
		void OnHurt(Entity entity, Hitbox hitbox, Hurtbox hurtbox);
	}
}
