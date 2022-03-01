using System.Collections.Generic;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class Locations : MonoBehaviour
	{
		[Header("Location Targets")]
		[SerializeField] private Transform _batterFarLeft;
		[SerializeField] private Transform _batterLeft;
		[SerializeField] private Transform _batterCenter;
		[SerializeField] private Transform _batterRight;
		[SerializeField] private Transform _batterFarRight;
		[SerializeField] private Transform _pitchersMound;
		[SerializeField] private Transform _strikeZoneTopLeft;
		[SerializeField] private Transform _strikeZoneBottomRight;
		[SerializeField] private Vector3 _inFrontOfBatterOffset;
		private BatterAreaLocations _batterAreas;
		private BatterAreaLocations _inFrontOfBatterAreas;

		public Vector3 pitchersMound => _pitchersMound.position;
		public BatterAreaLocations batter
		{
			get
			{
				if (_batterAreas == null)
					_batterAreas = new BatterAreaLocations(_batterFarLeft, _batterLeft, _batterCenter, _batterRight, _batterFarRight);
				return _batterAreas;
			}
		}
		public BatterAreaLocations inFrontOfBatter
		{
			get
			{
				if (_inFrontOfBatterAreas == null)
					_inFrontOfBatterAreas = new BatterAreaLocations(_batterFarLeft, _batterLeft, _batterCenter, _batterRight, _batterFarRight, _inFrontOfBatterOffset);
				return _inFrontOfBatterAreas;
			}
		}
		public Vector3 this[Location location]
		{
			get
			{
				switch (location)
				{
					case Location.BatterFarLeft: return batter.farLeft;
					case Location.BatterLeft: return batter.left;
					case Location.BatterCenter: return batter.center;
					case Location.BatterRight: return batter.right;
					case Location.BatterFarRight: return batter.farRight;
					case Location.InFrontOfBatterFarLeft: return inFrontOfBatter.farLeft;
					case Location.InFrontOfBatterLeft: return inFrontOfBatter.left;
					case Location.InFrontOfBatterCenter: return inFrontOfBatter.center;
					case Location.InFrontOfBatterRight: return inFrontOfBatter.right;
					case Location.InFrontOfBatterFarRight: return inFrontOfBatter.farRight;
					case Location.PitchersMound: return pitchersMound;
					default: return Vector3.zero;
				}
			}
		}

		public class BatterAreaLocations
		{
			private Dictionary<BatterArea, Transform> _areas;
			private Vector3 _offset;

			public Vector3 farLeft => _areas[BatterArea.FarLeft].position + _offset;
			public Vector3 left => _areas[BatterArea.Left].position + _offset;
			public Vector3 center => _areas[BatterArea.Center].position + _offset;
			public Vector3 right => _areas[BatterArea.Right].position + _offset;
			public Vector3 farRight => _areas[BatterArea.FarRight].position + _offset;
			public Vector3 this[BatterArea area] => _areas[area].position + _offset;

			public BatterAreaLocations(Transform farLeft, Transform left, Transform center, Transform right, Transform farRight) : this(farLeft, left, center, right, farRight, Vector3.zero) {}

			public BatterAreaLocations(Transform farLeft, Transform left, Transform center, Transform right, Transform farRight, Vector3 offset)
			{
				_areas = new Dictionary<BatterArea, Transform> {
					{ BatterArea.FarLeft, farLeft },
					{ BatterArea.Left, left },
					{ BatterArea.Center, center },
					{ BatterArea.Right, right },
					{ BatterArea.FarRight, farRight }
				};
				_offset = offset;
			}
		}

		public Vector3 GetStrikeZonePosition(Vector2 target)
		{
			float x = (target.x + 1f) / 2f;
			float y = (target.y + 1f) / 2f;
			return new Vector3(
				_strikeZoneTopLeft.transform.position.x * (1f - x) + _strikeZoneBottomRight.transform.position.x * x,
				_strikeZoneTopLeft.transform.position.y * (1f - y) + _strikeZoneBottomRight.transform.position.y * y,
				_strikeZoneTopLeft.transform.position.z * (1f - y) + _strikeZoneBottomRight.transform.position.z * y);
		}
	}
}