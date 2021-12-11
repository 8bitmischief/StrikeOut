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
				new IdleCommand(1f),
				new ThrowBoomerangCommand(true),
				new MoveToBatterCommand(),
				new IdleCommand(2f),
				new MoveCommand(Location.PitchersMound),
				new ThrowBoomerangCommand(true),
				new MoveCommand(Location.InFrontOfBatterFarLeft),
				new IdleCommand(1f),
				new MoveCommand(Location.PitchersMound),
				new ThrowBoomerangCommand(false)
			);
		}
	}
}