using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, string>
	{
		public void Move(Vector3 targetPosition) => Trigger("Move", targetPosition);

		public void Pitch() => Trigger("Pitch");

		public void Chop() => Trigger("Chop");

		public void ThrowBoomerang(Vector3 targetPosition, bool leanRight)
		{
			Flip(!leanRight);
			Trigger("Throw Boomerang", targetPosition);
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