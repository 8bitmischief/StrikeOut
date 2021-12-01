using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class PitcherAnimator : EntityAnimator<Pitcher, Pitcher.Animation>
	{
		private static readonly int StartTravelingHash = Animator.StringToHash("Start Traveling");
		private static readonly int StopTravelingHash = Animator.StringToHash("Stop Traveling");
		private static readonly int PitchHash = Animator.StringToHash("Pitch");
		private static readonly int LungeHash = Animator.StringToHash("Lunge");
		private static readonly int ThrowBoomerangHash = Animator.StringToHash("Throw Boomerang");

		[Header("Children")]
		[SerializeField] private Transform _spawnLocation;
		private Vector3 _targetPosition;

		public event Action<Vector3> onSpawnBall;
		public event Action<Vector3> onSpawnBoomerang;

		public void Pitch() => Trigger(PitchHash);

		public void Lunge(Vector3 target) => Trigger(LungeHash, target);

		public void ThrowBoomerang() => Trigger(ThrowBoomerangHash);

		public void TravelTo(Vector3 targetPosition)
		{
			_targetPosition = targetPosition;
			float distance = Vector3.Distance(transform.position, targetPosition);
		}

		protected override void OnStartAnimation(Pitcher.Animation animation)
		{
			
		}

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