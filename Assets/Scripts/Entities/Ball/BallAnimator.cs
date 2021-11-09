using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, Ball.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");

		public void Pitch () => Trigger(pitchHash);
	}
}