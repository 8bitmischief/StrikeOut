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
		[SerializeField] private Transform _pitcherMound;
		[SerializeField] private Transform _northStrikeZone;
		[SerializeField] private Transform _eastStrikeZone;
		[SerializeField] private Transform _southStrikeZone;
		[SerializeField] private Transform _westStrikeZone;
		private BatterAreaLocations _batterAreas;
		private StrikeZoneLocations _strikeZones;

		public Vector3 pitcherMoundPosition => _pitcherMound.position;
		public BatterAreaLocations batter
		{
			get
			{
				if (_batterAreas == null)
					_batterAreas = new BatterAreaLocations(_batterFarLeft, _batterLeft, _batterCenter, _batterRight, _batterFarRight);
				return _batterAreas;
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

		public class BatterAreaLocations
		{
			private Dictionary<BatterArea, Transform> _areas;

			public Vector3 farLeft => _areas[BatterArea.FarLeft].position;
			public Vector3 left => _areas[BatterArea.Left].position;
			public Vector3 center => _areas[BatterArea.Center].position;
			public Vector3 right => _areas[BatterArea.Right].position;
			public Vector3 farRight => _areas[BatterArea.FarRight].position;
			public Vector3 this[BatterArea area] => _areas[area].position;

			public BatterAreaLocations(Transform farLeft, Transform left, Transform center, Transform right, Transform farRight)
			{
				_areas = new Dictionary<BatterArea, Transform> {
					{ BatterArea.FarLeft, farLeft },
					{ BatterArea.Left, left },
					{ BatterArea.Center, center },
					{ BatterArea.Right, right },
					{ BatterArea.FarRight, farRight }
				};
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