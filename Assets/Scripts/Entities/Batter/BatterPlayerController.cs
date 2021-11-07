using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	[RequireComponent(typeof(Batter))]
	public class BatterPlayerController : EntityComponent<Batter> {}
}