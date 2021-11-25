using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : EntityComponent<Pitcher>
	{
		public override int componentUpdateOrder => EntityComponent.controllerUpdateOrder;

		public override void UpdateState() {
			if (entity.IsIdle())
			{
				entity.ThrowBoomerang();
			}
		}
	}
}