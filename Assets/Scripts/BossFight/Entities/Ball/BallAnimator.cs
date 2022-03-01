using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, string>
	{
		private static readonly int PitchTypeHash = Animator.StringToHash("Pitch Type");
		private static readonly int PitchHash = Animator.StringToHash("Pitch");
		private static readonly int HitHash = Animator.StringToHash("Hit");
		private static readonly int PassesThroughStrikeZoneHash = Animator.StringToHash("Passes Through Strike Zone");

		public void Pitch(PitchType pitch, Vector3 targetPosition, bool passesThroughStrikeZone)
		{
			animator.SetInteger(PitchTypeHash, (int) pitch);
			animator.SetBool(PassesThroughStrikeZoneHash, passesThroughStrikeZone);
			Trigger(PitchHash, targetPosition, false);
		}

		public void Hit(Vector3 targetPosition)
		{
			Trigger(HitHash, targetPosition, false);
		}
	}
}