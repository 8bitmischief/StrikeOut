using UnityEngine;
using SharedUnityMischief.Entities;
using SharedUnityMischief.Lifecycle;

namespace StrikeOut.BossFight
{
	public class BossFightUpdateLoop : UpdateLoop
	{
		[Header("Managers")]
		[SerializeField] private EntityManager _entityManager;

		public EntityManager entityManager => _entityManager;

		protected override void UpdateState()
		{
			_entityManager.UpdateState();
		}
	}
}
