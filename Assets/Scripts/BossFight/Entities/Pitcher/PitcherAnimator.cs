using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.State>
	{
		private static readonly int PitchHash = Animator.StringToHash("Pitch");
		private static readonly int LungeHash = Animator.StringToHash("Lunge");
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;

		public event Action<Vector3> onSpawnBall;
		public event Action<Vector3> onSpawnBoomerang;

		public void Pitch() => Trigger(PitchHash);

		public void Lunge(Vector3 target) => Trigger(LungeHash, target);

		public void ThrowBoomerang() => Trigger(ThrowBoomerangHash);

		protected override void OnAnimationEvent(AnimationEvent evt)
		{
			switch (evt.stringParameter)
			{
				case "Pitch Ball":
					onSpawnBall?.Invoke(_spawnLocation.transform.position);
					break;
				case "Throw Boomerang":
					onSpawnBoomerang?.Invoke(_spawnLocation.transform.position);
					break;
			}
		}
	}
}