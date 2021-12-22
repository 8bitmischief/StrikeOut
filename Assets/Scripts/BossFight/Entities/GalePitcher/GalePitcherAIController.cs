using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(GalePitcher))]
	public partial class GalePitcherAIController : EntityCommandController<GalePitcher>
	{
		private void OnEnable()
		{
			entity.onParry += OnParry;
		}

		private void OnDisable()
		{
			entity.onParry += OnParry;
		}

		protected override void DecideNextAction()
		{
			QueueCommands(
				new PitchCommand { pitchType = PitchType.Straight, strikeZone = StrikeZone.East },
				new PitchCommand { pitchType = PitchType.Straight, strikeZone = StrikeZone.West },
				IdleForOneSecond,
				new PitchCommand { pitchType = PitchType.Curveball, strikeZone = StrikeZone.North },
				IdleForOneSecond,
				MoveInFrontOfBatterCenter,
				SlashLeft,
				MoveInFrontOfBatterCenter,
				SlashRight,
				MoveToBatter,
				Chop,
				Chop,
				MoveToPitchersMound,
				IdleForOneSecond
			);
		}

		private void OnParry()
		{
			ClearCommands();
			QueueCommands(
				IdleForOneSecond,
				MoveToPitchersMound,
				IdleForOneSecond
			);
		}
	}
}