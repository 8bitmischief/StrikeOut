using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, Ball.State>
	{
		private static readonly int pitchTypeHash = Animator.StringToHash("Pitch Type");
		private static readonly int pitchHash = Animator.StringToHash("Pitch");
		private static readonly int hitHash = Animator.StringToHash("Hit");
		private static readonly int passesThroughStrikeZoneHash = Animator.StringToHash("Passes Through Strike Zone");

		public void Pitch(PitchType pitch, Vector3 target, bool passesThroughStrikeZone)
		{
			animator.SetInteger(pitchTypeHash, (int) pitch);
			animator.SetBool(passesThroughStrikeZoneHash, passesThroughStrikeZone);
			Trigger(pitchHash, target);
		}

		public void Hit(Vector3 target)
		{
			Trigger(hitHash, target);
		}
	}
}