using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Tomato))]
	public class TomatoPlayerController : EntityComponent<Tomato> {
		public override void UpdateState () {
			if (Game.I.input.swingNorth.justPressed) {
				Debug.Log("Hop");
				entity.Hop();
			}

			if (Game.I.input.swingSouth.justPressed)
				entity.Flip();
		}
	}
}