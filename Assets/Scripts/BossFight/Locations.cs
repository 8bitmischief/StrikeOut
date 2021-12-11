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
		[SerializeField] private Transform _northStrikeZone;
		[SerializeField] private Transform _eastStrikeZone;
		[SerializeField] private Transform _southStrikeZone;
		[SerializeField] private Transform _westStrikeZone;
		[SerializeField] private Vector3 _inFrontOfBatterOffset;
		private BatterAreaLocations _batterAreas;
		private BatterAreaLocations _inFrontOfBatterAreas;
		private StrikeZoneLocations _strikeZones;

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
		public StrikeZoneLocations strikeZone
		{
			get
			{
				if (_strikeZones == null)
					_strikeZones = new StrikeZoneLocations(_northStrikeZone, _eastStrikeZone, _southStrikeZone, _westStrikeZone);
				return _strikeZones;
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
					case Location.NorthStrikeZone: return strikeZone.north;
					case Location.EastStrikeZone: return strikeZone.east;
					case Location.SouthStrikeZone: return strikeZone.south;
					case Location.WestStrikeZone: return strikeZone.west;
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

		public class StrikeZoneLocations
		{
			private Dictionary<StrikeZone, Transform> _strikeZones;

			public Vector3 north => _strikeZones[StrikeZone.North].position;
			public Vector3 east => _strikeZones[StrikeZone.East].position;
			public Vector3 south => _strikeZones[StrikeZone.South].position;
			public Vector3 west => _strikeZones[StrikeZone.West].position;
			public Vector3 this[StrikeZone strikeZone] => _strikeZones[strikeZone].position;

			public StrikeZoneLocations(Transform north, Transform east, Transform south, Transform west)
			{
				_strikeZones = new Dictionary<StrikeZone, Transform> {
					{ StrikeZone.North, north },
					{ StrikeZone.East, east },
					{ StrikeZone.South, south },
					{ StrikeZone.West, west }
				};
			}
		}
	}
}