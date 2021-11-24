using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities {
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.State> {
		private static readonly int pitchHash = Animator.StringToHash("Pitch");
		private static readonly int lungeHash = Animator.StringToHash("Lunge");
		private static readonly int throwBoomerangHash = Animator.StringToHash("Throw Boomerang");

		public Action<Vector3> onSpawnBall;
		public Action<Vector3> onSpawnBoomerang;

		[Header("Children")]
		[SerializeField] private Transform spawnLocation;

		public void Pitch () => Trigger(pitchHash);

		public void Lunge (Vector3 target) => Trigger(lungeHash, target);

		public void ThrowBoomerang () => Trigger(throwBoomerangHash);

		protected override void OnAnimationEvent (AnimationEvent evt) {
			switch (evt.stringParameter) {
				case "Pitch Ball":
					onSpawnBall?.Invoke(spawnLocation.transform.position);
					break;
				case "Throw Boomerang":
					onSpawnBoomerang?.Invoke(spawnLocation.transform.position);
					break;
			}
		}
	}
}