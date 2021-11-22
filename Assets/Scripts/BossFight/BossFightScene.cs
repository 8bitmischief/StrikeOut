using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Lifecycle;
using SharedUnityMischief.Pool;

namespace StrikeOut.BossFight {
	public class BossFightScene : SceneManager<BossFightScene> {
		[Header("Locations")]
		[SerializeField] private Transform _batterLeft;
		[SerializeField] private Transform _batterDodgeLeft;
		[SerializeField] private Transform _batterRight;
		[SerializeField] private Transform _batterDodgeRight;
		[SerializeField] private Transform _northStrikeZone;
		[SerializeField] private Transform _eastStrikeZone;
		[SerializeField] private Transform _southStrikeZone;
		[SerializeField] private Transform _westStrikeZone;

		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop _updateLoop;

		[Header("Prefabs")]
		[SerializeField] private PrefabPool<Ball> ballPool;

		[Header("Data")]
		[SerializeField] private PitchDataObject _pitchData;

		public Vector3 batterLeftPosition => _batterLeft.position;
		public Vector3 batterDodgeLeftPosition => _batterDodgeLeft.position;
		public Vector3 batterRightPosition => _batterRight.position;
		public Vector3 batterDodgeRightPosition => _batterDodgeRight.position;
		public Vector3 northStrikeZonePosition => _northStrikeZone.position;
		public Vector3 eastStrikeZonePosition => _eastStrikeZone.position;
		public Vector3 southStrikeZonePosition => _southStrikeZone.position;
		public Vector3 westStrikeZonePosition => _westStrikeZone.position;
		public BossFightUpdateLoop updateLoop => _updateLoop;
		public PitchDataObject pitchData => _pitchData;

		[HideInInspector] public List<Ball> balls = new List<Ball>();

		private void Update () {
			// Pause the game
			if (Game.I.input.togglePause.justPressed) {
				if (updateLoop.isPaused)
					updateLoop.Resume();
				else if (Game.I.debugMode)
					updateLoop.Pause();
			}
			// Step through individual frames
			if (Game.I.input.nextFrame.justPressed && Game.I.debugMode) {
				if (!updateLoop.isPaused)
					updateLoop.Pause();
				if (Game.I.input.alternateMode.isHeld)
					updateLoop.Advance(0.018f, true);
				else
					updateLoop.AdvanceOneFrame(true);
			}
			// Slow down time
			if (Game.I.input.slowTime.justReleased || Game.I.input.slowTime.justPressed)
				Time.timeScale = Game.I.input.slowTime.isHeld ? 0.10f : 1.00f;
			// Update the game
			if (!updateLoop.updateAutomatically)
				updateLoop.Advance();
		}

		protected override void OnDestroy () {
			ballPool.Dispose();
			base.OnDestroy();
		}

		public Ball SpawnBall (Vector3 position) => updateLoop.entityManager.SpawnEntityFromPool(ballPool, position);

		public void DespawnEntity (Entity entity) => updateLoop.entityManager.DespawnEntity(entity);

		public Vector3 GetStrikeZonePosition (StrikeZone strikeZone) {
			switch (strikeZone) {
				case StrikeZone.North: return _northStrikeZone.position;
				case StrikeZone.East: return _eastStrikeZone.position;
				case StrikeZone.South: return _southStrikeZone.position;
				case StrikeZone.West: return _westStrikeZone.position;
				default: return Vector3.zero;
			}
		}
	}
}