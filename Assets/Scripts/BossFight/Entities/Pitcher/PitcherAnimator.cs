using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.Animation>
	{
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;

		public event Action<Vector3> onSpawnBoomerang;

		public void ThrowBoomerang(Vector3 targetPosition, bool leanRight)
		{
			SetFlipped(!leanRight);
			Trigger(ThrowBoomerangHash, targetPosition);
		}

		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Throw Boomerang":
					onSpawnBoomerang?.Invoke(_spawnLocation.transform.position);
					break;
			}
		}

		private void SetFlipped(bool flipped)
		{
			transform.localScale = new Vector3(
				(flipped ? -1f : 1f) * Mathf.Abs(transform.localScale.x),
				transform.localScale.y,
				transform.localScale.z);
		}
	}
}