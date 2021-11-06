using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Tomato))]
	public class TomatoPlayerController : EntityComponent<Tomato> {
		public override void UpdateState () {
			if (Game.I.input.swingNorth.justPressed)
				entity.Hop();

			if (Game.I.input.swingSouth.justPressed)
				entity.Flip();
		}
	}
}