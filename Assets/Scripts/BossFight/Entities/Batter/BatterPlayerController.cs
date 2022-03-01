using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Entities;

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
			Vector2 circularAim = Game.I.input.aim.vector;
			float u = circularAim.x;
			float v = circularAim.y;
			Vector2 rectangularAim = new Vector2(
				u * u > v * v ? Mathf.Sign(u) * Mathf.Sqrt(u * u + v * v) : Mathf.Sign(v) * u / v * Mathf.Sqrt(u * u + v * v),
				u * u > v * v ? Mathf.Sign(u) * v / u * Mathf.Sqrt(u * u + v * v) : Mathf.Sign(v) * Mathf.Sqrt(u * u + v * v)
			);
			// Vector2 rectangularAim = new Vector2(
			// 	0.5f * Mathf.Sqrt(2f + u * u - v * v + 2f * u * Mathf.Sqrt(2f)) - 0.5f * Mathf.Sqrt(2f + u * u - v * v - 2f * u * Mathf.Sqrt(2f)),
			// 	0.5f * Mathf.Sqrt(2f - u * u + v * v + 2f * v * Mathf.Sqrt(2f)) - 0.5f * Mathf.Sqrt(2f - u * u + v * v - 2f * v * Mathf.Sqrt(2f))
			// );
			entity.SetAim(rectangularAim);

			// Check for swing inputs
			if (Game.I.input.swing.justPressed)
				UseOrBufferInput(BatterInput.Swing);

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
				case BatterInput.Swing:
					return TrySwinging();
				case BatterInput.DodgeLeft:
					return TryDodging(Direction.Left);
				case BatterInput.DodgeRight:
					return TryDodging(Direction.Right);
				default:
					return false;
			}
		}

		private bool TrySwinging()
		{
			if (entity.CanSwing())
			{
				entity.Swing();
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
			Swing = 1,
			DodgeLeft = 2,
			DodgeRight = 3
		}
	}
}