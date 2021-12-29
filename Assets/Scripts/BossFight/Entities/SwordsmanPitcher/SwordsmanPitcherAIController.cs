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
				QueueCommand(new MoveCommand { location = Location.InFrontOfBatterCenter });
			}
			QueueCommands(
				new MeleeDownwardSlash(),
				IdleForOneSecond
			);
		}
	}
}