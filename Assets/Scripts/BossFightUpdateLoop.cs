using UnityEngine;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut {
	public class BossFightUpdateLoop : UpdateLoop {
		[Header("Managers")]
		[SerializeField] private EntityManager entityManager;

		protected override void UpdateState () {
			entityManager.UpdateState();
		}
	}
}
