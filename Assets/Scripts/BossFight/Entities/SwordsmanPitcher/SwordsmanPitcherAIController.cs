using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcher))]
	public partial class SwordsmanPitcherAIController : EntityCommandController<SwordsmanPitcher>
	{
		 private bool _hasMovedIntoPosition = false;

		protected override void DecideNextAction()
		{
			 if (!_hasMovedIntoPosition)
			 {
			 	_hasMovedIntoPosition = true;
			 	QueueCommands(
					new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.North },
					new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.East },
					new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.South },
					new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.West },
					new MoveCommand { location = Location.InFrontOfBatterCenter }
				);
			 }
			QueueCommands(
				 new MeleeDownwardSlash(),
				IdleForOneSecond
			);
		}
	}
}