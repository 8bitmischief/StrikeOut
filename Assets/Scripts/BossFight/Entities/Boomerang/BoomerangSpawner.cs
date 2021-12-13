using UnityEngine;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight.Entities
{
	public class BoomerangSpawner : EntitySpawner {
		protected override void OnSpawnChild(Entity entity)
		{
			Boomerang boomerang = entity as Boomerang;
			boomerang.Throw(false);
		}
	}
}