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
				new PitchCommand { pitchType = PitchType.Curveball, target = new Vector2(0.1f, 0.1f) },
				IdleForOneSecond,
				new TeleportSlashCommand(),
				IdleForTwoSeconds
			);
		}
	}
}