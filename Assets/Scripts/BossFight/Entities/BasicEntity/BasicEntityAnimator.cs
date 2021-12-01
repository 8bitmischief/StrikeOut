using UnityEngine;
using SharedUnityMischief.Entities.Animated;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Animator))]
	public class BasicEntityAnimator : EntityAnimator<BasicEntity, BasicEntity.Animation>
	{
		private static readonly int TriggerHash = Animator.StringToHash("Trigger");

		public void Trigger() => base.Trigger(TriggerHash);
	}
}