using UnityEngine;
using SharedUnityMischief.Animation;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EnumStateMachineAnimator<Ball.State> {}
}