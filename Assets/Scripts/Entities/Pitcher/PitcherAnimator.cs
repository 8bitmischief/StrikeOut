using System;
using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");

		public Action<Vector3> onSpawnBall;

		[Header("Children")]
		[SerializeField] private Transform spawnBallLocation;

		public void Pitch () => Trigger(pitchHash);

		protected override void OnAnimationEvent (AnimationEvent evt) {
			if (evt.stringParameter == "Spawn Ball")
				onSpawnBall?.Invoke(spawnBallLocation.transform.position);
		}
	}
}