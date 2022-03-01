using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcher))]
	public partial class SwordsmanPitcherAIController : EntityCommandController<SwordsmanPitcher>
	{
		protected override void DecideNextAction()
		{
			QueueCommands(
				new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.East },
				IdleForOneSecond,
				new TeleportSlashCommand(),
				IdleForTwoSeconds
			);
		}
	}
}