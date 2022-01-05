using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IBatterPredictedHittable
	{
		void OnPredictedHit(BatterHitRecord hit, int frames);
	}
}