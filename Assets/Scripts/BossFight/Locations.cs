using System.Collections.Generic;
using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class Locations : MonoBehaviour
	{
		[Header("Location Targets")]
		[SerializeField] private Transform _batterLeft;
		[SerializeField] private Transform _batterDodgeLeft;
		[SerializeField] private Transform _batterRight;
		[SerializeField] private Transform _batterDodgeRight;
		[SerializeField] private Transform _pitcherMound;
		[SerializeField] private Transform _northStrikeZone;
		[SerializeField] private Transform _eastStrikeZone;
		[SerializeField] private Transform _southStrikeZone;
		[SerializeField] private Transform _westStrikeZone;
		private StrikeZoneLocations _strikeZones;

		public Vector3 batterLeftPosition => _batterLeft.position;
		public Vector3 batterDodgeLeftPosition => _batterDodgeLeft.position;
		public Vector3 batterRightPosition => _batterRight.position;
		public Vector3 batterDodgeRightPosition => _batterDodgeRight.position;
		public Vector3 pitcherMoundPosition => _pitcherMound.position;
		public StrikeZoneLocations strikeZones
		{
			get
			{
				if (_strikeZones == null)
					_strikeZones = new StrikeZoneLocations(_northStrikeZone, _eastStrikeZone, _southStrikeZone, _westStrikeZone);
				return _strikeZones;
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