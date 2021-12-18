using System;
using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcherAnimator))]
	public class SwordsmanPitcher : AnimatedEntity<SwordsmanPitcherAnimator, string>
	{
		public bool isIdle => animation == "Idle";
		public float idleTime => animation == "Idle" ? totalAnimationTime : 0f;

		public override void OnSpawn()
		{
			Scene.I.entityManager.pitcher = this;
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.pitcher == this)
				Scene.I.entityManager.pitcher = null;
		}

		public void Slash() => animator.Slash();
	}
}