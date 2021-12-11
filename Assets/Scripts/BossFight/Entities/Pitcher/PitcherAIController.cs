using UnityEngine;

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
				ThrowBoomerangRight,
				IdleForTwoSeconds,

				MoveToBatter,
				Chop,
				IdleForOneSecond,

				MoveToPitchersMound,
				IdleForOneSecond
			);
		}
	}
}