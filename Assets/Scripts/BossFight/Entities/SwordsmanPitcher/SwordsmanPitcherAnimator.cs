using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class SwordsmanPitcherAnimator : EntityAnimator<SwordsmanPitcher, string>
	{
		public void Slash() => Trigger("Slash");

		private void Flip(bool flipped)
		{
			transform.localScale = new Vector3(
				(flipped ? -1f : 1f) * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z);
		}
	}
}