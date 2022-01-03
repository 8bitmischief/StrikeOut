using UnityEngine;

namespace StrikeOut.BossFight
{
	public interface IBatterHittable
	{
		void OnHit(BatterHitRecord hit);
	}
}