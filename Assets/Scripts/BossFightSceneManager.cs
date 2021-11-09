using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	public class BossFightSceneManager : SceneManager {
		public static readonly Vector3 batterLeftPosition = new Vector3(-2.75f, 0f, 0f);
		public static readonly Vector3 batterRightPosition = new Vector3(2.75f, 0f, 0f);
		public static readonly Dictionary<CardinalDirection, Vector3> strikeZonePositions = new Dictionary<CardinalDirection, Vector3>() {
			{ CardinalDirection.North, new Vector3(0f, 3.8f, 0f) },
			{ CardinalDirection.East, new Vector3(1.1f, 2.5f, 0f) },
			{ CardinalDirection.South, new Vector3(0f, 1.2f, 0f) },
			{ CardinalDirection.West, new Vector3(-1.1f, 2.5f, 0f) }
		};
		public static Vector3 northStrikeZonePosition => strikeZonePositions[CardinalDirection.North];
		public static Vector3 eastStrikeZonePosition => strikeZonePositions[CardinalDirection.East];
		public static Vector3 southStrikeZonePosition => strikeZonePositions[CardinalDirection.South];
		public static Vector3 westStrikeZonePosition => strikeZonePositions[CardinalDirection.West];

		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop updateLoop;

		[Header("Prefabs")]
		[SerializeField] private Ball ballPrefab;

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
				updateLoop.AdvanceOneFrame(true);
			}
			// Slow down time
			if (Game.I.input.slowTime.justPressed && Game.I.input.slowTime.isHeld)
				updateLoop.timeScale = 0.10f;
			if (Game.I.input.slowTime.justReleased && !Game.I.input.slowTime.isHeld)
				updateLoop.timeScale = 1.00f;
			// Update the game
			if (!updateLoop.updateAutomatically)
				updateLoop.Advance();
		}

		public Ball SpawnBall (Vector3 position) {
			return updateLoop.entityManager.SpawnEntityFromPrefab(ballPrefab, position);
		}

		public void DespawnEntity (Entity entity) {
			updateLoop.entityManager.DespawnEntity(entity);
		}
	}
}