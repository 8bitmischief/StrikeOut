using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : EntityComponent<Pitcher>
	{
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder;

		public override void LateUpdateState()
		{
			if (!UpdateLoop.I.isInterpolating)
			{
				if (entity.isIdle)
				{
					entity.ThrowBoomerang(true);
				}
			}
		}
	}
}