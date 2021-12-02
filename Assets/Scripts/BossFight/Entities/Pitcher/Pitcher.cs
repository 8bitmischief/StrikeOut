using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using SharedUnityMischief.Pool;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<PitcherAnimator, Pitcher.Animation>
	{
		[Header("Prefab Pools")]
		[SerializeField] private PrefabPool<Ball> _ballPool;
		[SerializeField] private PrefabPool<Boomerang> _boomerangPool;

		public bool isIdle => animation == Animation.Idle;

		public void TravelTo(Vector3 targetPosition)
		{
			animator.TravelTo(targetPosition, 10f);
		}

		private void Start()
		{
			_ballPool.Prewarm();
			_boomerangPool.Prewarm();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			animator.onSpawnBall += SpawnBall;
			animator.onSpawnBoomerang += SpawnBoomerang;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			animator.onSpawnBall -= SpawnBall;
			animator.onSpawnBoomerang -= SpawnBoomerang;
		}

		private void OnDestroy()
		{
			_ballPool.Dispose();
			_boomerangPool.Dispose();
		}

		public void Pitch()
		{
			animator.Pitch();
		}

		public void LungeLeft()
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
			animator.Lunge(new Vector3(-2.6f, 0f, 3f));
		}

		public void LungeRight()
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
			animator.Lunge(new Vector3(2.6f, 0f, 3f));
		}

		public void ThrowBoomerang()
		{
			animator.ThrowBoomerang();
		}

		protected override void OnStartAnimation(Animation animation)
		{
			switch (animation)
			{
				case Animation.BackOff:
					transform.localScale = new Vector3(1f, 1f, 1f);
					animator.SetRootMotion(new Vector3(0f, 0f, 25f));
					break;
			}
		}

		private void SpawnBall(Vector3 spawnPosition)
		{
			Ball ball = BossFightScene.I.entityManager.SpawnEntityFromPool(_ballPool, spawnPosition);
			switch (Random.Range(1, 5))
			{
				case 1:
					ball.Pitch(PitchType.Curveball, StrikeZone.North);
					break;
				case 2:
					ball.Pitch(PitchType.Curveball, StrikeZone.East);
					break;
				case 3:
					ball.Pitch(PitchType.Curveball, StrikeZone.South);
					break;
				case 4:
					ball.Pitch(PitchType.Curveball, StrikeZone.West);
					break;
			}
		}

		private void SpawnBoomerang(Vector3 spawnPosition)
		{
			Boomerang boomerang = BossFightScene.I.entityManager.SpawnEntityFromPool(_boomerangPool, spawnPosition);
			boomerang.Throw();
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Pitch = 2,
			Lunge = 3,
			BackOff = 4,
			ThrowBoomerang = 5,
			TravelStart = 6,
			Travel = 7,
			TravelBrake = 8,
			TravelEnd = 9
		}
	}
}