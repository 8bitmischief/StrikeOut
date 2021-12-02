using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public class PitcherAIController : EntityComponent<Pitcher>
	{
		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder;

		public override void UpdateState()
		{
			if (entity.isIdle && !BossFightUpdateLoop.I.isInterpolating)
			{
				entity.TravelTo(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(0f, 50f)));
			}
		}
	}
}