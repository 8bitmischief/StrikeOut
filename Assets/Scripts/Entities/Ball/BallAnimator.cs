using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class BallAnimator : EntityAnimator<Ball, Ball.State> {}
}