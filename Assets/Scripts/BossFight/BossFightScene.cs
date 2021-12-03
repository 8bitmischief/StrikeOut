using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Input.Control;
using StrikeOut.BossFight.Data;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	[DefaultExecutionOrder(-20)]
	public class BossFightScene : SceneManager<BossFightScene>
	{
		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop _updateLoop;
		[Header("Data")]
		[SerializeField] private PitchDataObject _pitchData;
		[Header("Locations")]
		[SerializeField] private Transform _batterLeft;
		[SerializeField] private Transform _batterDodgeLeft;
		[SerializeField] private Transform _batterRight;
		[SerializeField] private Transform _batterDodgeRight;
		[SerializeField] private Transform _northStrikeZone;
		[SerializeField] private Transform _eastStrikeZone;
		[SerializeField] private Transform _southStrikeZone;
		[SerializeField] private Transform _westStrikeZone;
		[SerializeField] private Transform _pitcherMound;
		private Batter _batter = null;
		private Pitcher _pitcher = null;
		private List<Ball> _balls = new List<Ball>();

		public BossFightUpdateLoop updateLoop => _updateLoop;
		public EntityManager entityManager => _updateLoop.entityManager;
		public PitchDataObject pitchData => _pitchData;
		public Batter batter { get => _batter; set => _batter = value; }
		public Pitcher pitcher { get => _pitcher; set => _pitcher = value; }
		public List<Ball> balls => _balls;
		public Vector3 batterLeftPosition => _batterLeft.position;
		public Vector3 batterDodgeLeftPosition => _batterDodgeLeft.position;
		public Vector3 batterRightPosition => _batterRight.position;
		public Vector3 batterDodgeRightPosition => _batterDodgeRight.position;
		public Vector3 northStrikeZonePosition => _northStrikeZone.position;
		public Vector3 eastStrikeZonePosition => _eastStrikeZone.position;
		public Vector3 southStrikeZonePosition => _southStrikeZone.position;
		public Vector3 westStrikeZonePosition => _westStrikeZone.position;
		public Vector3 pitcherMoundPosition => _pitcherMound.position;

		private void OnEnable()
		{
			_updateLoop.onPreUpdateState += OnPreUpdateState;
			_updateLoop.onPostUpdateState += OnPostUpdateState;
		}

		private void Update()
		{
			if (!Game.I.isPaused)
			{
				if (Game.I.isAdvancingFrameByFrame)
					_updateLoop.AdvanceOneFrame();
				else
					_updateLoop.Advance();
			}
		}

		private void OnDisable()
		{
			_updateLoop.onPreUpdateState -= OnPreUpdateState;
			_updateLoop.onPostUpdateState -= OnPostUpdateState;
		}

		public Vector3 GetStrikeZonePosition(StrikeZone strikeZone)
		{
			switch (strikeZone)
			{
				case StrikeZone.North: return _northStrikeZone.position;
				case StrikeZone.East: return _eastStrikeZone.position;
				case StrikeZone.South: return _southStrikeZone.position;
				case StrikeZone.West: return _westStrikeZone.position;
				default: return Vector3.zero;
			}
		}

		private void OnPreUpdateState()
		{
			if (Game.I.input.mode == SimulatedControlMode.Simulate)
				Game.I.input.SimulateUpdate();
		}

		private void OnPostUpdateState()
		{
			Game.I.input.ConsumeInstantaneousInputs();
		}
	}
}