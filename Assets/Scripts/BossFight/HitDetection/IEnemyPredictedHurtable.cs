using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IEnemyPredictedHurtable
	{
		void OnPredictedHurt(BatterHitRecord hit, int frames);
	}
}