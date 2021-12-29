using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(SwordsmanPitcher))]
	public partial class SwordsmanPitcherAIController : EntityCommandController<SwordsmanPitcher>
	{
		private readonly Command IdleForOneSecond = new IdleCommand { duration = 1f };
		private readonly Command IdleForTwoSeconds = new IdleCommand { duration = 2f };

		public abstract class PitcherCommand : Command<SwordsmanPitcher>
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

		private class SlashCommand : PitcherCommand
		{
			public override void Start() => entity.Slash();
		}

		private class MeleeDownwardSlash : PitcherCommand
		{
			private bool _hasSlashed = false;

			public override void Start() => entity.EnterRaisedSwordStance();

			public override void Update()
			{
				if (!_hasSlashed && entity.CanCancelAnimation(3))
				{
					_hasSlashed = true;
					entity.MeleeDownwardSlash();
				}
			}
		}
	}
}