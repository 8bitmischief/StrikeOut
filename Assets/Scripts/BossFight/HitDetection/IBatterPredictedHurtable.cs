using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IBatterPredictedHurtable
	{
		void OnPredictedHurt(EnemyHitRecord hit, int frames);
	}
}