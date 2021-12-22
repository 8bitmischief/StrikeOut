using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : EntityComponent<Batter>
	{
		[SerializeField] private int _maxBufferedInputFrames = 8;
		private BatterInput _bufferedInput = BatterInput.None;
		private int _bufferedInputFrames = 0;

		public override int componentUpdateOrder => EntityComponent.ControllerUpdateOrder;

		public override void UpdateState()
		{
			// Check for swing inputs
			if (Game.I.input.swingNorth.justPressed)
				UseOrBufferInput(BatterInput.SwingNorth);
			if (Game.I.input.swingEast.justPressed)
				UseOrBufferInput(BatterInput.SwingEast);
			if (Game.I.input.swingSouth.justPressed)
				UseOrBufferInput(BatterInput.SwingSouth);
			if (Game.I.input.swingWest.justPressed)
				UseOrBufferInput(BatterInput.SwingWest);

			// Check for dodge inputs
			if (Game.I.input.dodgeLeft.justPressed)
				UseOrBufferInput(BatterInput.DodgeLeft);
			if (Game.I.input.dodgeRight.justPressed)
				UseOrBufferInput(BatterInput.DodgeRight);

			// Try using the buffered input
			if (_bufferedInput != BatterInput.None && TryUsingInput(_bufferedInput))
				_bufferedInput = BatterInput.None;

			// Clear the buffered input that's been buffered for too long
			if (_bufferedInput != BatterInput.None && !Scene.I.updateLoop.isInterpolating)
			{
				_bufferedInputFrames++;
				if (_bufferedInputFrames > _maxBufferedInputFrames)
					_bufferedInput = BatterInput.None;
			}
		}

		private void UseOrBufferInput(BatterInput input)
		{
			if (TryUsingInput(input))
			{
				_bufferedInput = BatterInput.None;
			}
			else
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
					return TryDodging(Direction.Left);
				case BatterInput.DodgeRight:
					return TryDodging(Direction.Right);
				default:
					return false;
			}
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

		private bool TryDodging(Direction direction)
		{
			if (entity.CanDodge(direction))
			{
				entity.Dodge(direction);
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