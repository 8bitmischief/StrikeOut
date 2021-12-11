using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public partial class PitcherAIController : EntityCommandController<Pitcher>
	{
		private readonly Command idleForOneSecond = new IdleCommand(1f);
		private readonly Command idleForTwoSeconds = new IdleCommand(2f);
		private readonly Command moveToPitchersMound = new MoveCommand(Location.PitchersMound);
		private readonly Command moveToBatter = new MoveToBatterCommand();
		private readonly Command throwBoomerangLeft = new ThrowBoomerangCommand(false);
		private readonly Command throwBoomerangRight = new ThrowBoomerangCommand(true);

		private class IdleCommand : Command<Pitcher>
		{
			private float _duration;

			public IdleCommand(float duration)
			{
				_duration = duration;
			}

			public override bool IsDone() => entity.isIdle && entity.idleTime >= _duration;
		}

		private class MoveCommand : Command<Pitcher>
		{
			private Location _location;

			public MoveCommand(Location location)
			{
				_location = location;
			}

			public override void Start() => entity.Move(_location);

			public override bool IsDone() => entity.isIdle;
		}

		private class MoveToBatterCommand : Command<Pitcher>
		{
			public override void Start()
			{
				switch (Scene.I.batter.area)
				{
					case BatterArea.FarLeft: entity.Move(Location.InFrontOfBatterFarLeft); break;
					case BatterArea.Left: entity.Move(Location.InFrontOfBatterLeft); break;
					case BatterArea.Center: entity.Move(Location.InFrontOfBatterCenter); break;
					case BatterArea.Right: entity.Move(Location.InFrontOfBatterRight); break;
					case BatterArea.FarRight: entity.Move(Location.InFrontOfBatterFarRight); break;
				}
			}

			public override bool IsDone() => entity.isIdle;
		}

		private class ThrowBoomerangCommand : Command<Pitcher>
		{
			private bool _throwToRight;

			public ThrowBoomerangCommand(bool throwToRight)
			{
				_throwToRight = throwToRight;
			}

			public override void Start() => entity.ThrowBoomerang(_throwToRight);

			public override bool IsDone() => entity.isIdle;
		}
	}
}