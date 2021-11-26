using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using SharedUnityMischief.Pool;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<Pitcher.State, PitcherAnimator>
	{
		[Header("Prefab Pools")]
		[SerializeField] private PrefabPool<Ball> _ballPool;
		[SerializeField] private PrefabPool<Boomerang> _boomerangPool;

		private void Start()
		{
			_ballPool.Prewarm();
			_boomerangPool.Prewarm();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			_animator.onSpawnBall += SpawnBall;
			_animator.onSpawnBoomerang += SpawnBoomerang;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_animator.onSpawnBall -= SpawnBall;
			_animator.onSpawnBoomerang -= SpawnBoomerang;
		}

		private void OnDestroy()
		{
			_ballPool.Dispose();
			_boomerangPool.Dispose();
		}

		public bool IsIdle() => _animator.state == State.Idle;

		public void Pitch()
		{
			_animator.Pitch();
		}

		public void LungeLeft()
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
			_animator.Lunge(new Vector3(-2.6f, 0f, 3f));
		}

		public void LungeRight()
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
			_animator.Lunge(new Vector3(2.6f, 0f, 3f));
		}

		public void ThrowBoomerang()
		{
			_animator.ThrowBoomerang();
		}

		protected override void OnEnterState(State state)
		{
			switch (state)
			{
				case State.BackOff:
					transform.localScale = new Vector3(1f, 1f, 1f);
					_animator.SetRootMotion(new Vector3(0f, 0f, 25f));
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

		public enum State
		{
			None = 0,
			Idle = 1,
			Pitch = 2,
			Lunge = 3,
			BackOff = 4,
			ThrowBoomerang = 5
		}
	}
}