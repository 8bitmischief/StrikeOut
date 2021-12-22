using UnityEngine;

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

		public override void UpdateState()
		{
			base.UpdateState();
		}

		protected override void DecideNextAction()
		{
			QueueCommands(
				PitchNorth,
				PitchEast,
				MoveInFrontOfBatterCenter,
				SlashLeft,
				MoveInFrontOfBatterCenter,
				SlashRight,
				MoveToBatter,
				Chop,
				Chop,
				MoveToPitchersMound,
				IdleForTwoSeconds
			);
		}

		private void OnParry()
		{
			ClearCommands();
			QueueCommand(IdleForOneSecond);
		}
	}
}