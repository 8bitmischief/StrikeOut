using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.Animation>
	{
		private static readonly int MoveHash = Animator.StringToHash("Move");
		private static readonly int PitchHash = Animator.StringToHash("Pitch");
		private static readonly int ChopHash = Animator.StringToHash("Chop");
		private static readonly int SlashHash = Animator.StringToHash("Slash");
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");
		private static readonly int ParryHash = Animator.StringToHash("Parry");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;

		public void Move(Vector3 targetPosition) => Trigger(MoveHash, targetPosition);

		public void Pitch() => Trigger(PitchHash);

		public void Chop() => Trigger(ChopHash);

		public void ThrowBoomerang(Vector3 targetPosition, bool leanRight)
		{
			Flip(!leanRight);
			Trigger(ThrowBoomerangHash, targetPosition);
		}

		public void Slash(bool toTheRight)
		{
			Flip(!toTheRight);
			Trigger(SlashHash);
		}

		public void Parry() => Trigger(ParryHash);

		private void Flip(bool flipped)
		{
			transform.localScale = new Vector3(
				(flipped ? -1f : 1f) * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z);
		}
	}
}