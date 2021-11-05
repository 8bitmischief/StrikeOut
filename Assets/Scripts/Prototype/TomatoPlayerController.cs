using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Tomato))]
	public class TomatoPlayerController : EntityComponent<Tomato> {
		public override void UpdateState () {
			if (Toolbox.input.swingNorth.justPressed)
				entity.Hop();

			if (Toolbox.input.swingSouth.justPressed)
				entity.Flip();
		}
	}
}