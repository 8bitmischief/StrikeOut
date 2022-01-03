using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IEnemyHurtable
	{
		void OnHurt(BatterHitRecord hit);
	}
}