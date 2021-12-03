using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;
using StrikeOut.BossFight.Entities;

namespace StrikeOut.BossFight
{
	public class BossFightScene : SceneManager<BossFightScene>
	{
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
		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop _updateLoop;
		[Header("Data")]
		[SerializeField] private PitchDataObject _pitchData;
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

		private void Update()
		{
			// Pause the game
			if (Game.I.input.togglePause.justPressed)
			{
				if (_updateLoop.isPaused)
				{
					_updateLoop.Resume();
				}
				else if (Game.I.debugMode)
				{
					_updateLoop.Pause();
				}
			}
			// Step through individual frames
			if (Game.I.input.nextFrame.justPressed && Game.I.debugMode)
			{
				if (!_updateLoop.isPaused)
				{
					_updateLoop.Pause();
				}
				if (Game.I.input.alternateMode.isHeld)
				{
					_updateLoop.Advance(0.018f, true);
				}
				else
				{
					_updateLoop.AdvanceOneFrame(true);
				}
			}
			// Slow down time
			if (Game.I.input.slowTime.justReleased || Game.I.input.slowTime.justPressed)
				Time.timeScale = Game.I.input.slowTime.isHeld ? 0.10f : 1.00f;
			// Update the game
			if (!_updateLoop.updateAutomatically)
			{
				_updateLoop.Advance();
			}
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
	}
}