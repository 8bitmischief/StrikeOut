using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class Hitbox : EntityComponent {
		public override void UpdateState()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
			Debug.Log("woop");
		}
	}
}
