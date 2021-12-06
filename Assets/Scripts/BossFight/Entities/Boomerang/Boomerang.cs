using System;
using UnityEngine;
using SharedUnityMischief.Entities.Animated;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoomerangAnimator))]
	public class Boomerang : AnimatedEntity<BoomerangAnimator, Boomerang.Animation>
	{
		private bool _thrownToTheRight = false;

		public void Throw(bool toTheRight)
		{
			_thrownToTheRight = toTheRight;
			animator.Throw();
		}

		protected override void OnStartAnimation(Animation animation)
		{
			switch (animation)
			{
				case Animation.Throw:
					if (_thrownToTheRight)
					{
						animator.SetRootMotion(Scene.I.locations.batter.right + new Vector3(0f, 1.5f, 0f));
					}
					else
					{
						animator.SetRootMotion(Scene.I.locations.batter.left + new Vector3(0f, 1.5f, 0f));
					}
					break;
				case Animation.Rebound:
					if (_thrownToTheRight)
					{
						animator.SetRootMotion(Scene.I.locations.batter.left + new Vector3(0f, 1.5f, 0f));
					}
					else
					{
						animator.SetRootMotion(Scene.I.locations.batter.right + new Vector3(0f, 1.5f, 0f));
					}
					break;
				case Animation.Return:
					animator.SetRootMotion(Scene.I.pitcher.transform.position + new Vector3(0f, 1.5f, 0f));
					break;
				case Animation.Done:
					DespawnEntity(this);
					break;
			}
		}

		private void ANIMATION_Attack(AnimationEvent evt)
		{
			AttackData attackData = evt.objectReferenceParameter as AttackData;
			Scene.I.attacks.Add(new Attack(attackData, this));
		}

		public enum Animation
		{
			None = 0,
			Idle = 1,
			Throw = 2,
			Rebound = 3,
			Return = 4,
			Done = 5
		}
	}
}
