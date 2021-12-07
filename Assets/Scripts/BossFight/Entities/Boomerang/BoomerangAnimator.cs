using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BoomerangAnimator : EntityAnimator<Boomerang, Boomerang.Animation>
	{
		private static readonly int ThrowHash = Animator.StringToHash("Throw");
		private static readonly int HitHash = Animator.StringToHash("Hit");

		public void Throw() => Trigger(ThrowHash);

		public void Hit() => Trigger(HitHash);
	}
}