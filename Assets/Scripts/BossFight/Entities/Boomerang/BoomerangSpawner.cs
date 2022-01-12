using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class BoomerangSpawner : EntitySpawner<Boomerang> {
		protected override void OnSpawnChildEntity(Boomerang boomerang)
		{
			boomerang.Throw(false);
		}
	}
}