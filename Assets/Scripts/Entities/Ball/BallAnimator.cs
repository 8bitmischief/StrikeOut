using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, Ball.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");

		public void Pitch (Vector3 targetPosition) => Trigger(pitchHash, targetPosition, true);
	}
}