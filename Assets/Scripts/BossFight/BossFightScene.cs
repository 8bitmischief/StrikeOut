using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Input.Control;
using StrikeOut.BossFight.Data;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
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
		private float _timeScale = 1f;
		private bool _isPaused = false;
		private bool _pauseNextFrame = false;

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
			if (Game.I.debugMode)
			{
				if (_pauseNextFrame)
				{
					_pauseNextFrame = false;
					Pause();
				}
				// Pause and unpause the game
				if (Game.I.input.togglePause.justPressed)
				{
					if (_isPaused)
						Resume();
					else
						Pause();
				}
				// Step through individual frames
				if (Game.I.input.nextFrame.justPressed)
				{
					if (!_isPaused)
					{
						Pause();
					}
					else
					{
						Time.timeScale = 1f;
						_updateLoop.AdvanceOneFrame(true);
						_pauseNextFrame = true;
					}
				}
				// Slow down time
				_timeScale = Game.I.input.slowTime.isHeld ? 0.10f : 1.00f;
				if (!_isPaused)
					Time.timeScale = _timeScale;
			}
			// Update the game
			if (!_updateLoop.updateAutomatically)
				_updateLoop.Advance();
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

		private void Pause()
		{
			_isPaused = true;
			Time.timeScale = 0f;
			_updateLoop.Pause();
			Game.I.input.mode = SimulatedControlMode.Simulate;
		}

		private void Resume()
		{
			_isPaused = false;
			Time.timeScale = _timeScale;
			_updateLoop.Resume();
			Game.I.input.mode = SimulatedControlMode.PassThrough;
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