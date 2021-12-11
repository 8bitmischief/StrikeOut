using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Pitcher))]
	public partial class PitcherAIController : EntityCommandController<Pitcher>
	{
		private class IdleCommand : Command<Pitcher>
		{
			private float _duration;

			public IdleCommand(float duration)
			{
				_duration = duration;
			}

			public override bool IsDone()
			{
				return entity.isIdle && entity.idleTime >= _duration;
			}
		}

		private class ThrowBoomerangCommand : Command<Pitcher>
		{
			private bool _throwToRight;

			public ThrowBoomerangCommand(bool throwToRight)
			{
				_throwToRight = throwToRight;
			}

			public override void Start()
			{
				entity.ThrowBoomerang(_throwToRight);
			}

			public override bool IsDone()
			{
				return entity.isIdle;
			}
		}
	}
}