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
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

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

		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Spawn Ball":
					entity.SpawnBall(_spawnLocation.transform.position);
					break;
				case "Spawn Boomerang":
					entity.SpawnBoomerang(_spawnLocation.transform.position);
					break;
			}
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