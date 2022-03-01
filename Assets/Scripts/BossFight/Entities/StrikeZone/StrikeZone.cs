using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class StrikeZone : Entity
	{
		[Header("Children")]
		[SerializeField] private Transform _reticle;

		public override void OnSpawn()
		{
			Scene.I.entityManager.strikeZone = this;
		}

		public void SetAim(Vector2 aim)
		{
			_reticle.transform.localPosition = aim;
		}

		public override void OnDespawn()
		{
			if (Scene.I.entityManager.strikeZone == this)
				Scene.I.entityManager.strikeZone = null;
		}
	}
}