using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : EntityComponent<Batter> {
		private int maxBufferedInputFrames = 8;

		private BatterInput bufferedInput = BatterInput.None;
		private int bufferedInputFrames = 0;

		public override void UpdateState () {
			// Listen to inputs
			if (Game.I.input.dodgeLeft.justPressed)
				UseOrBufferInput(BatterInput.DodgeLeft);
			if (Game.I.input.dodgeRight.justPressed)
				UseOrBufferInput(BatterInput.DodgeRight);

			// Try using buffered input
			if (bufferedInput != BatterInput.None)
				if (TryUsingInput(bufferedInput))
					bufferedInput = BatterInput.None;

			// Only buffer inputs for so long
			if (bufferedInput != BatterInput.None && !UpdateLoop.I.isInterpolating) {
				bufferedInputFrames++;
				if (bufferedInputFrames > maxBufferedInputFrames)
					bufferedInput = BatterInput.None;
			}
		}

		private void UseOrBufferInput (BatterInput input) {
			if (!TryUsingInput(input)) {
				bufferedInput = input;
				bufferedInputFrames = 0;
			}
		}

		private bool TryUsingInput (BatterInput input) {
			switch (input) {
				case BatterInput.DodgeLeft:
					return TryDodgingLeft();
				case BatterInput.DodgeRight:
					return TryDodgingRight();
			}
			return false;
		}

		private bool TryDodgingLeft () {
			if (entity.CanDodgeLeft()) {
				entity.DodgeLeft();
				return true;
			}
			return false;
		}

		private bool TryDodgingRight () {
			if (entity.CanDodgeRight()) {
				entity.DodgeRight();
				return true;
			}
			return false;
		}

		private enum BatterInput {
			None = 0,
			SwingNorth = 1,
			SwingEast = 2,
			SwingSouth = 3,
			SwingWest = 4,
			DodgeLeft = 5,
			DodgeRight = 6
		}
	}
}