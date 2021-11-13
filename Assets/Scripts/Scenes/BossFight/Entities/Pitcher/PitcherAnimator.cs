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

		public void Lunge (Vector3 target) => Trigger(lungeHash, target);

		protected override void OnAnimationEvent (AnimationEvent evt) {
			if (evt.stringParameter == "Pitch Ball")
				onPitchBall?.Invoke(spawnBallLocation.transform.position);
		}
	}
}