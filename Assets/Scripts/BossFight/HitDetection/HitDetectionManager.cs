using System.Collections.Generic;
using UnityEngine;
using StrikeOut.BossFight.Data;
using SharedUnityMischief.Entities;

namespace StrikeOut.BossFight
{
	public class HitDetectionManager : MonoBehaviour
	{
		private HashSet<BatterHitbox> _batterHitboxes = new HashSet<BatterHitbox>();
		private HashSet<BatterHurtbox> _batterHurtboxes = new HashSet<BatterHurtbox>();
		private HashSet<EnemyHitbox> _enemyHitboxes = new HashSet<EnemyHitbox>();
		private HashSet<EnemyHurtbox> _enemyHurtboxes = new HashSet<EnemyHurtbox>();

		public ICollection<BatterHitbox> batterHitboxes => _batterHitboxes;
		public ICollection<BatterHurtbox> batterHurtboxes => _batterHurtboxes;
		public ICollection<EnemyHitbox> enemyHitboxes => _enemyHitboxes;
		public ICollection<EnemyHurtbox> enemyHurtboxes => _enemyHurtboxes;

		public void CheckForHits()
		{
			// Check for the batter hitting enemies
			if (_batterHitboxes.Count > 0 && _enemyHurtboxes.Count > 0)
			{
				HashSet<BatterHitbox> hitboxes = new HashSet<BatterHitbox>(_batterHitboxes);
				HashSet<EnemyHurtbox> hurtboxes = new HashSet<EnemyHurtbox>(_enemyHurtboxes);
				foreach (BatterHitbox hitbox in hitboxes)
				{
					if (hitbox.isActiveAndEnabled)
					{
						foreach (EnemyHurtbox hurtbox in hurtboxes)
						{
							if (hurtbox.isActiveAndEnabled)
							{
								BatterHitRecord hit = hitbox.CheckForHit(hurtbox);
								if (hit != null)
								{
									if (hitbox.isActive && hurtbox.isActive)
									{
										hitbox.OnHit(hit);
										hurtbox.OnHurt(hit);
									}
									else
									{
										int frames = CheckForFutureHit(hit, hitbox, hurtbox);
										if (frames >= 0)
										{
											hitbox.OnPredictedHit(hit, frames);
											hurtbox.OnPredictedHurt(hit, frames);
										}
									}
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
					if (hitbox.isActiveAndEnabled)
					{
						foreach (BatterHurtbox hurtbox in hurtboxes)
						{
							if (hurtbox.isActiveAndEnabled)
							{
								EnemyHitRecord hit = hitbox.CheckForHit(hurtbox);
								if (hit != null)
								{
									if (hitbox.isActive && hurtbox.isActive)
									{
										hitbox.OnHit(hit);
										hurtbox.OnHurt(hit);
									}
									else
									{
										int frames = CheckForFutureHit(hit, hitbox, hurtbox);
										if (frames >= 0)
										{
											hitbox.OnPredictedHit(hit, frames);
											hurtbox.OnPredictedHurt(hit, frames);
										}
									}
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

		public bool DoAnyHitboxesHit(BatterArea area, int startFrame = 0, int endFrame = -1)
		{
			HashSet<EnemyHitbox> hitters = new HashSet<EnemyHitbox>();
			foreach (EnemyHitbox hitbox in _enemyHitboxes)
				if (hitbox.isActiveAndEnabled && hitbox.WillHit(area, startFrame, endFrame))
					return true;
			return false;
		}

		public bool DoAnyHitboxesHit(StrikeZone strikeZone, int startFrame = 0, int endFrame = -1)
		{
			HashSet<BatterHitbox> hitters = new HashSet<BatterHitbox>();
			foreach (BatterHitbox hitbox in _batterHitboxes)
				if (hitbox.isActiveAndEnabled && hitbox.WillHit(strikeZone, startFrame, endFrame))
					return true;
			return false;
		}

		public bool DoAnyHurtboxesGetHurtBy(BatterArea area, int startFrame = 0, int endFrame = -1)
		{
			HashSet<BatterHurtbox> hurtees = new HashSet<BatterHurtbox>();
			foreach (BatterHurtbox hurtbox in _batterHurtboxes)
				if (hurtbox.isActiveAndEnabled && hurtbox.WillBeHurtBy(area, startFrame, endFrame))
					return true;
			return false;
		}

		public bool DoAnyHurtboxesGetHurtBy(StrikeZone strikeZone, int startFrame = 0, int endFrame = -1)
		{
			HashSet<EnemyHurtbox> hurtees = new HashSet<EnemyHurtbox>();
			foreach (EnemyHurtbox hurtbox in _enemyHurtboxes)
				if (hurtbox.isActiveAndEnabled && hurtbox.WillBeHurtBy(strikeZone, startFrame, endFrame))
					return true;
			return false;
		}

		public ICollection<EnemyHitbox> GetHitboxesThatHit(BatterArea area, int startFrame = 0, int endFrame = -1)
		{
			HashSet<EnemyHitbox> hitters = new HashSet<EnemyHitbox>();
			foreach (EnemyHitbox hitbox in _enemyHitboxes)
				if (hitbox.isActiveAndEnabled && hitbox.WillHit(area, startFrame, endFrame))
					hitters.Add(hitbox);
			return hitters;
		}

		public ICollection<BatterHitbox> GetHitboxesThatHit(StrikeZone strikeZone, int startFrame = 0, int endFrame = -1)
		{
			HashSet<BatterHitbox> hitters = new HashSet<BatterHitbox>();
			foreach (BatterHitbox hitbox in _batterHitboxes)
				if (hitbox.isActiveAndEnabled && hitbox.WillHit(strikeZone, startFrame, endFrame))
					hitters.Add(hitbox);
			return hitters;
		}

		public ICollection<BatterHurtbox> GetHurtboxesHurtBy(BatterArea area, int startFrame = 0, int endFrame = -1)
		{
			HashSet<BatterHurtbox> hurtees = new HashSet<BatterHurtbox>();
			foreach (BatterHurtbox hurtbox in _batterHurtboxes)
				if (hurtbox.isActiveAndEnabled && hurtbox.WillBeHurtBy(area, startFrame, endFrame))
					hurtees.Add(hurtbox);
			return hurtees;
		}

		public ICollection<EnemyHurtbox> GetHurtboxesHurtBy(StrikeZone strikeZone, int startFrame = 0, int endFrame = -1)
		{
			HashSet<EnemyHurtbox> hurtees = new HashSet<EnemyHurtbox>();
			foreach (EnemyHurtbox hurtbox in _enemyHurtboxes)
				if (hurtbox.isActiveAndEnabled && hurtbox.WillBeHurtBy(strikeZone, startFrame, endFrame))
					hurtees.Add(hurtbox);
			return hurtees;
		}

		private int CheckForFutureHit(HitRecord hit, Hitbox hitbox, Hurtbox hurtbox)
		{
			int framesUntilHitboxActive;
			if (hitbox.isActive)
				framesUntilHitboxActive = 0;
			else if (hitbox.willBeActive)
				framesUntilHitboxActive = hitbox.framesUntilActive;
			else
				framesUntilHitboxActive = -1;

			int framesUntilHitboxInactive;
			if (hitbox.willBeInactive)
				framesUntilHitboxInactive = hitbox.framesUntilInactive;
			else
				framesUntilHitboxInactive = -1;

			int framesUntilHurtboxActive;
			if (hurtbox.isActive)
				framesUntilHurtboxActive = 0;
			else if (hurtbox.willBeActive)
				framesUntilHurtboxActive = hurtbox.framesUntilActive;
			else
				framesUntilHurtboxActive = -1;

			int framesUntilHurtboxInactive;
			if (hurtbox.willBeInactive)
				framesUntilHurtboxInactive = hurtbox.framesUntilInactive;
			else
				framesUntilHurtboxInactive = -1;

			if (framesUntilHitboxActive != -1 &&
				framesUntilHurtboxActive != -1 &&
				(framesUntilHitboxInactive == -1 || framesUntilHitboxInactive > framesUntilHurtboxActive) &&
				(framesUntilHurtboxInactive == -1 || framesUntilHurtboxInactive > framesUntilHitboxActive))
				return Mathf.Max(framesUntilHitboxActive, framesUntilHurtboxActive);
			else
				return -1;
		}
	}
}