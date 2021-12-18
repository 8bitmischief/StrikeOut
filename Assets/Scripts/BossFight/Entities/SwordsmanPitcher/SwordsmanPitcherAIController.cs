using UnityEngine;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcher))]
	public partial class SwordsmanPitcherAIController : EntityCommandController<SwordsmanPitcher>
	{
		protected override void DecideNextAction()
		{
			QueueCommands(
				Slash,
				Slash,
				IdleForOneSecond
			);
		}
	}
}