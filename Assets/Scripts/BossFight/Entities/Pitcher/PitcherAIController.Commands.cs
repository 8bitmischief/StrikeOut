using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public partial class PitcherAIController : EntityCommandController<Pitcher>
	{
		private readonly Command IdleForOneSecond = new IdleCommand { duration = 1f };
		private readonly Command IdleForTwoSeconds = new IdleCommand { duration = 2f };
		private readonly Command MoveToPitchersMound = new MoveCommand { location = Location.PitchersMound };
		private readonly Command MoveToBatter = new MoveToBatterCommand();
		private readonly Command Pitch = new PitchCommand();
		private readonly Command Chop = new ChopCommand();
		private readonly Command ThrowBoomerangLeft = new ThrowBoomerangCommand { toTheRight = false };
		private readonly Command ThrowBoomerangRight = new ThrowBoomerangCommand { toTheRight = true };

		public abstract class PitcherCommand : Command<Pitcher>
		{
			public override bool IsDone() => entity.isIdle;
		}

		private class IdleCommand : PitcherCommand
		{
			public float duration;

			public override bool IsDone() => entity.isIdle && entity.idleTime >= duration;
		}

		private class MoveCommand : PitcherCommand
		{
			public Location location;

			public override void Start() => entity.Move(location);
		}

		private class MoveToBatterCommand : PitcherCommand
		{
			public override void Start() => entity.Move(Scene.I.entityManager.batter.isOnRightSide ? Location.InFrontOfBatterRight : Location.InFrontOfBatterLeft);
		}

		private class PitchCommand : PitcherCommand
		{
			public override void Start() => entity.Pitch();
		}

		private class ChopCommand : PitcherCommand
		{
			public override void Start() => entity.Chop();
		}

		private class ThrowBoomerangCommand : PitcherCommand
		{
			public bool toTheRight;

			public override void Start() => entity.ThrowBoomerang(toTheRight);
		}
	}
}