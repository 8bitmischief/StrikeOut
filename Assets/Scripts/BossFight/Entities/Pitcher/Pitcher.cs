using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using SharedUnityMischief.Pool;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<PitcherAnimator, Pitcher.Animation>
	{
		[Header("Prefab Pools")]
		[SerializeField] private PrefabPool<Boomerang> _boomerangPool;

		public bool isIdle => animation == Animation.Idle;

		private void Start()
		{
			_boomerangPool.Prewarm();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			animator.onSpawnBoomerang += SpawnBoomerang;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			animator.onSpawnBoomerang -= SpawnBoomerang;
		}

		private void OnDestroy()
		{
			_boomerangPool.Dispose();
		}

		public void ThrowBoomerang(bool toTheRight)
		{
			if (toTheRight)
				animator.ThrowBoomerang(BossFightScene.I.pitcherMoundPosition + new Vector3(1f, 0f, 0f), true);
			else
				animator.ThrowBoomerang(BossFightScene.I.pitcherMoundPosition - new Vector3(1f, 0f, 0f), false);
		}

		private void SpawnBoomerang(Vector3 spawnPosition)
		{
			Boomerang boomerang = SpawnEntityFromPool(_boomerangPool, spawnPosition);
			boomerang.Throw();
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			ThrowBoomerang = 2
		}
	}
}