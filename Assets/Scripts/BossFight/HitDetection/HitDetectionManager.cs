using System.Collections.Generic;
using UnityEngine;

namespace StrikeOut.BossFight
{
	public class HitDetectionManager : MonoBehaviour
	{
		private HashSet<BatterHitbox> _batterHitboxes = new HashSet<BatterHitbox>();
		private HashSet<BatterHurtbox> _batterHurtboxes = new HashSet<BatterHurtbox>();
		private HashSet<EnemyHitbox> _enemyHitboxes = new HashSet<EnemyHitbox>();
		private HashSet<EnemyHurtbox> _enemyHurtboxes = new HashSet<EnemyHurtbox>();

		public void CheckForHits()
		{
			// Check for the batter hitting enemies
			if (_batterHitboxes.Count > 0 && _enemyHurtboxes.Count > 0)
			{
				HashSet<BatterHitbox> hitboxes = new HashSet<BatterHitbox>(_batterHitboxes);
				HashSet<EnemyHurtbox> hurtboxes = new HashSet<EnemyHurtbox>(_enemyHurtboxes);
				foreach (BatterHitbox hitbox in hitboxes)
				{
					if (hitbox.isActiveAndEnabled && hitbox.isActive)
					{
						foreach (EnemyHurtbox hurtbox in hurtboxes)
						{
							if (hurtbox.isActiveAndEnabled && hurtbox.isActive)
							{
								BatterHitRecord hit = hitbox.CheckForHit(hurtbox);
								if (hit != null)
								{
									hitbox.OnHit(hit);
									hurtbox.OnHurt(hit);
								}
							}
						}
					}
				}
			}
			// Check for enemies hitting the batter
			if (_enemyHitboxes.Count > 0 && _batterHurtboxes.Count > 0)
			{
				HashSet<EnemyHitbox> hitboxes = new HashSet<EnemyHitbox>(_enemyHitboxes);
				HashSet<BatterHurtbox> hurtboxes = new HashSet<BatterHurtbox>(_batterHurtboxes);
				foreach (EnemyHitbox hitbox in hitboxes)
				{
					if (hitbox.isActiveAndEnabled && hitbox.isActive)
					{
						foreach (BatterHurtbox hurtbox in hurtboxes)
						{
							if (hurtbox.isActiveAndEnabled && hurtbox.isActive)
							{
								EnemyHitRecord hit = hitbox.CheckForHit(hurtbox);
								if (hit != null)
								{
									hitbox.OnHit(hit);
									hurtbox.OnHurt(hit);
								}
							}
						}
					}
				}
			}
		}

		public void RegisterHitbox(BatterHitbox hitbox) => _batterHitboxes.Add(hitbox);
		public void RegisterHitbox(EnemyHitbox hitbox) => _enemyHitboxes.Add(hitbox);

		public void UnregisterHitbox(BatterHitbox hitbox) => _batterHitboxes.Remove(hitbox);
		public void UnregisterHitbox(EnemyHitbox hitbox) => _enemyHitboxes.Remove(hitbox);

		public void RegisterHurtbox(BatterHurtbox hurtbox) => _batterHurtboxes.Add(hurtbox);
		public void RegisterHurtbox(EnemyHurtbox hurtbox) => _enemyHurtboxes.Add(hurtbox);

		public void UnregisterHurtbox(BatterHurtbox hurtbox) => _batterHurtboxes.Remove(hurtbox);
		public void UnregisterHurtbox(EnemyHurtbox hurtbox) => _enemyHurtboxes.Remove(hurtbox);
	}
}