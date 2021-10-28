using System;
using UnityEngine;
using SharedUnityMischief.Animation;

namespace StrikeOut {
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EnumStateMachineAnimator<Pitcher.State> {
		private static readonly int PITCH_HASH = Animator.StringToHash("Pitch");

		public Action<Vector3> onSpawnBall;

		[Header("Children")]
		[SerializeField] private Transform spawnBallLocation;

		public void Pitch () => Trigger(PITCH_HASH);

		private void AnimationEvent_SpawnBall () => onSpawnBall?.Invoke(spawnBallLocation.transform.position);
	}
}