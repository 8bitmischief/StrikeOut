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
				idleForOneSecond,
				throwBoomerangRight,
				moveToBatter,
				idleForTwoSeconds,
				moveToPitchersMound,
				throwBoomerangRight,
				moveToBatter,
				idleForOneSecond,
				moveToPitchersMound,
				throwBoomerangLeft
			);
		}
	}
}