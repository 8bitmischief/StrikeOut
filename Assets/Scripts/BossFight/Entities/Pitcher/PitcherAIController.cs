using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public partial class PitcherAIController : EntityCommandController<Pitcher>
	{
		public override void UpdateState()
		{
			base.UpdateState();
		}

		protected override void DecideNextAction()
		{
			QueueCommands(
				IdleForOneSecond,
				ThrowBoomerangRight,
				MoveToBatter,
				IdleForTwoSeconds,
				MoveToPitchersMound,
				ThrowBoomerangRight,
				MoveToBatter,
				IdleForOneSecond,
				MoveToPitchersMound,
				ThrowBoomerangLeft
			);
		}
	}
}