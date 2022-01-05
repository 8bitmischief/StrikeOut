using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IEnemyPredictedHittable
	{
		void OnPredictedHit(EnemyHitRecord hit, int frames);
	}
}