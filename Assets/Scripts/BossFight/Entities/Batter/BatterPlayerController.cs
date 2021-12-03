using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Lifecycle;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : EntityComponent<Batter>
	{
		private BatterInput _bufferedInput = BatterInput.None;
		private int _bufferedInputFrames = 0;
		private int _maxBufferedInputFrames = 8;

		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder;

		public override void LateUpdateState()
		{
			// Listen to inputs
			if (Game.I.input.swingNorth.justPressed)
			{
				UseOrBufferInput(BatterInput.SwingNorth);
			}
			if (Game.I.input.swingEast.justPressed)
			{
				UseOrBufferInput(BatterInput.SwingEast);
			}
			if (Game.I.input.swingSouth.justPressed)
			{
				UseOrBufferInput(BatterInput.SwingSouth);
			}
			if (Game.I.input.swingWest.justPressed)
			{
				UseOrBufferInput(BatterInput.SwingWest);
			}
			if (Game.I.input.dodgeLeft.justPressed)
			{
				UseOrBufferInput(BatterInput.DodgeLeft);
			}
			if (Game.I.input.dodgeRight.justPressed)
			{
				UseOrBufferInput(BatterInput.DodgeRight);
			}

			// Try using buffered input
			if (_bufferedInput != BatterInput.None)
			{
				if (TryUsingInput(_bufferedInput))
				{
					_bufferedInput = BatterInput.None;
				}
			}

			// Only buffer inputs for so long
			if (_bufferedInput != BatterInput.None && !Scene.I.updateLoop.isInterpolating)
			{
				_bufferedInputFrames++;
				if (_bufferedInputFrames > _maxBufferedInputFrames)
				{
					_bufferedInput = BatterInput.None;
				}
			}
		}

		private void UseOrBufferInput(BatterInput input)
		{
			if (!TryUsingInput(input))
			{
				_bufferedInput = input;
				_bufferedInputFrames = 0;
			}
		}

		private bool TryUsingInput(BatterInput input)
		{
			switch (input)
			{
				case BatterInput.SwingNorth:
					return TrySwinging(StrikeZone.North);
				case BatterInput.SwingEast:
					return TrySwinging(StrikeZone.East);
				case BatterInput.SwingSouth:
					return TrySwinging(StrikeZone.South);
				case BatterInput.SwingWest:
					return TrySwinging(StrikeZone.West);
				case BatterInput.DodgeLeft:
					return TryDodgingLeft();
				case BatterInput.DodgeRight:
					return TryDodgingRight();
			}
			return false;
		}

		private bool TrySwinging(StrikeZone strikeZone)
		{
			if (entity.CanSwing(strikeZone))
			{
				entity.Swing(strikeZone);
				return true;
			}
			return false;
		}

		private bool TryDodgingLeft()
		{
			if (entity.CanDodgeLeft())
			{
				entity.DodgeLeft();
				return true;
			}
			return false;
		}

		private bool TryDodgingRight()
		{
			if (entity.CanDodgeRight())
			{
				entity.DodgeRight();
				return true;
			}
			return false;
		}

		private enum BatterInput
		{
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