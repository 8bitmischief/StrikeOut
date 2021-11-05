using UnityEngine;

namespace StrikeOut {
	public class TomatoGameManager : SharedUnityMischief.Lifecycle.GameManager<TomatoGameManager> {
		[Header("Entities")]
		[SerializeField] private Tomato tomato;

		protected override void UpdateState () {
			tomato.UpdateEntityState();
		}
	}
}