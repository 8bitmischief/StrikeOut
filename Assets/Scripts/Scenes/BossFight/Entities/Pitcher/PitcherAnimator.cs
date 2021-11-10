using System;
using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");
		private static readonly int lungeHash = Animator.StringToHash("Lunge");

		public Action<Vector3> onPitchBall;

		[Header("Children")]
		[SerializeField] private Transform spawnBallLocation;

		public void Pitch () => Trigger(pitchHash);

		public void Lunge (Vector3 targetPosition) => Trigger(lungeHash, targetPosition, true);

		protected override void OnAnimationEvent (AnimationEvent evt) {
			if (evt.stringParameter == "Pitch Ball")
				onPitchBall?.Invoke(spawnBallLocation.transform.position);
		}
	}
}