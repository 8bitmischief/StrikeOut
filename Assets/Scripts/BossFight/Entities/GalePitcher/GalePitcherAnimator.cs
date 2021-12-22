using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class GalePitcherAnimator : EntityAnimator<GalePitcher, string>
	{
		public void Move(Vector3 targetPosition) => Trigger("Move", targetPosition, false);

		public void Pitch(bool includeWindup, bool pitchToTheRight)
		{
			animator.SetBool("Include Windup", includeWindup);
			Flip(pitchToTheRight);
			Trigger("Pitch");
		}

		public void Chop() => Trigger("Chop");

		public void ThrowBoomerang(Vector3 targetPosition, bool leanRight)
		{
			Flip(!leanRight);
			Trigger("Throw Boomerang", targetPosition, false);
		}

		public void Slash(bool toTheRight)
		{
			Flip(!toTheRight);
			Trigger("Slash");
		}

		public void Parry() => Trigger("Parry");

		private void Flip(bool flipped)
		{
			transform.localScale = new Vector3(
				(flipped ? -1f : 1f) * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z);
		}
	}
}