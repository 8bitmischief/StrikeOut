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

		private class TeleportSlashCommand : PitcherCommand
		{
			public override void Start() => entity.TeleportSlash();
		}

		private class PitchCommand : PitcherCommand
		{
			public StrikeZone strikeZone;
			public PitchType pitchType;

			public override void Start() => entity.Pitch(pitchType, strikeZone);
		}
	}
}