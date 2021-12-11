using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.Animation>
	{
		private static readonly int MoveHash = Animator.StringToHash("Move");
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;

		public void Move(Vector3 targetPosition)
		{
			Trigger(MoveHash, targetPosition);
		}

		public void ThrowBoomerang(Vector3 targetPosition, bool leanRight)
		{
			Flip(!leanRight);
			Trigger(ThrowBoomerangHash, targetPosition);
		}

		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Throw Boomerang":
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