using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class SwordsmanPitcherAnimator : EntityAnimator<SwordsmanPitcher, string>
	{
		[Header("Pitcher Config")]
		[SerializeField] private int _cancelAnimationLevel = 0;

		public void Move(Vector3 targetPosition) => Trigger("Move", targetPosition, false);

		public void Slash() => Trigger("Slash");

		public void EnterRaisedSwordStance() => Trigger("Raised Sword Stance");

		public void MeleeDownwardSlash() => Trigger("Melee Downward Slash");

		public void Pitch() => Trigger("Pitch");

		public bool CanCancelAnimation(int cancelLevel = 1)
		{
			return animation == "Idle" || _cancelAnimationLevel >= cancelLevel;
		}

		private void Flip(bool flipped)
		{
			transform.localScale = new Vector3(
				(flipped ? -1f : 1f) * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z);
		}
	}
}