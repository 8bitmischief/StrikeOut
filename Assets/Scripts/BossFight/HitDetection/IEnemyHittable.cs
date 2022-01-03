using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IEnemyHittable
	{
		void OnHit(EnemyHitRecord hit);
	}
}