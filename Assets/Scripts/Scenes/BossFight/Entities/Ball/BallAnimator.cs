using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, Ball.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");
		private static readonly int throwHash = Animator.StringToHash("Throw");
		private static readonly int passesThroughStrikeZoneHash = Animator.StringToHash("Passes Through Strike Zone");

		public void Throw (Ball.Pitch pitch, Vector3 targetPosition, bool passesThroughStrikeZone) {
			animator.SetInteger(pitchHash, (int) pitch);
			animator.SetBool(passesThroughStrikeZoneHash, passesThroughStrikeZone);
			Trigger(throwHash, targetPosition, true);
		}
	}
}