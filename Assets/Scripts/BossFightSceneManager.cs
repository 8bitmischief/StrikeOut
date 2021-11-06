using UnityEngine;

namespace StrikeOut {
	public class BossFightSceneManager : SceneManager {
		[Header("Children")]
		[SerializeField] private BossFightUpdateLoop updateLoop;

		public override void UpdateState () {
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
			updateLoop.timeScale = Game.I.input.slowTime.isHeld && Game.I.debugMode ? 0.10f : 1.00f;
			// Update the game
			if (!updateLoop.updateAutomatically)
				updateLoop.Advance();
		}
	}
}