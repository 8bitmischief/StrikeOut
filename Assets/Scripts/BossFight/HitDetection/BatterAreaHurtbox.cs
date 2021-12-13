using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class BatterAreaHurtbox : Hurtbox
	{
		[Header("Areas")]
		[SerializeField] private RelativeBatterArea _area;
		[SerializeField] private RelativeBatterArea _destinationArea;

		public BatterArea area
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_area)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}
		public BatterArea destinationArea
		{
			get
			{
				bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
				switch (_destinationArea)
				{
					case RelativeBatterArea.FarSameSide: return isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft;
					case RelativeBatterArea.SameSide: return isOnRightSide ? BatterArea.Right : BatterArea.Left;
					case RelativeBatterArea.Center: return BatterArea.Center;
					case RelativeBatterArea.OppositeSide: return isOnRightSide ? BatterArea.Left : BatterArea.Right;
					case RelativeBatterArea.FarOppositeSide: return isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight;
					default: return BatterArea.None;
				}
			}
		}
	}
}