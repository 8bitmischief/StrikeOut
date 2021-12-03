using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(PitcherAnimator))]
	public class Pitcher : AnimatedEntity<PitcherAnimator, Pitcher.Animation>
	{
		[Header("Prefab Pools")]
		[SerializeField] private BoomerangPool _boomerangPool;

		public bool isIdle => animation == Animation.Idle;

		public override void OnSpawn()
		{
			BossFightScene.I.pitcher = this;
		}

		public override void OnDespawn()
		{
			if (BossFightScene.I.pitcher == this)
			{
				BossFightScene.I.pitcher = null;
			}
		}

		public void ThrowBoomerang(bool toTheRight)
		{
			animator.ThrowBoomerang(BossFightScene.I.pitcherMoundPosition + new Vector3(toTheRight ? 3f : -3f, 0f, 0f), toTheRight);
		}

		public void SpawnBoomerang(Vector3 spawnPosition)
		{
			Boomerang boomerang = SpawnEntityFromPool(_boomerangPool, spawnPosition);
			boomerang.Throw(true);
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			ThrowBoomerang = 2
		}
	}
}