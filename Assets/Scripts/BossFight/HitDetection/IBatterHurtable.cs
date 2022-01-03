using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IBatterHurtable
	{
		void OnHurt(EnemyHitRecord hit);
	}
}