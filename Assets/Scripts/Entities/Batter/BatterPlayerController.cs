using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : EntityComponent<Batter> {
		public override void UpdateState () {
			if (Game.I.input.dodgeLeft.justPressed && entity.CanDodgeLeft())
				entity.DodgeLeft();
			if (Game.I.input.dodgeRight.justPressed && entity.CanDodgeRight())
				entity.DodgeRight();
		}
	}
}