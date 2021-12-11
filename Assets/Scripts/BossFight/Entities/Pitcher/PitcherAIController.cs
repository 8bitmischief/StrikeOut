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
				new IdleCommand(1f),
				new ThrowBoomerangCommand(true),
				new ThrowBoomerangCommand(true),
				new ThrowBoomerangCommand(false)
			);
		}
	}
}