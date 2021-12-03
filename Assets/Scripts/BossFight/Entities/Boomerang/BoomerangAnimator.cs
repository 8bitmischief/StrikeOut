using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BoomerangAnimator : EntityAnimator<Boomerang, Boomerang.Animation>
	{
		private static readonly int ThrowHash = Animator.StringToHash("Throw");

		public void Throw(Vector3 targetPosition)
		{
			Trigger(ThrowHash, targetPosition);
		}
	}
}