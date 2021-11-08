using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	public class BossFightSceneManager : SceneManager {
		public static readonly Vector3 batterLeftPosition = new Vector3(-3f, 0f, 0f);
		public static readonly Vector3 batterRightPosition = new Vector3(3f, 0f, 0f);

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