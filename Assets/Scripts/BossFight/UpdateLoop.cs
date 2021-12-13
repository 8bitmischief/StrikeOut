using System.Collections.Generic;
using UnityEngine;

namespace StrikeOut.BossFight
{
	public class UpdateLoop : SharedUnityMischief.Lifecycle.UpdateLoop
	{
		private HashSet<Hitbox> _hitboxes = new HashSet<Hitbox>();
		private HashSet<Hitbox> _hitboxesToAdd = new HashSet<Hitbox>();
		private HashSet<Hitbox> _hitboxesToRemove = new HashSet<Hitbox>();
		private HashSet<Hurtbox> _hurtboxes = new HashSet<Hurtbox>();
		private HashSet<Hurtbox> _hurtboxesToAdd = new HashSet<Hurtbox>();
		private HashSet<Hurtbox> _hurtboxesToRemove = new HashSet<Hurtbox>();

		private void Start()
		{
			Physics.autoSimulation = false;
		}

		protected override void UpdateState()
		{
			Scene.I.entityManager.EarlyUpdateEntities();
			Scene.I.entityManager.UpdateEntities();
			// Make changes to hitboxes and hurtboxes only now
			foreach (Hitbox hitbox in _hitboxesToAdd)
				_hitboxes.Add(hitbox);
			foreach (Hitbox hitbox in _hitboxesToRemove)
				_hitboxes.Remove(hitbox);
			foreach (Hurtbox hurtbox in _hurtboxesToAdd)
				_hurtboxes.Add(hurtbox);
			foreach (Hurtbox hurtbox in _hurtboxesToRemove)
				_hurtboxes.Remove(hurtbox);
			_hitboxesToAdd.Clear();
			_hitboxesToRemove.Clear();
			_hurtboxesToAdd.Clear();
			_hurtboxesToRemove.Clear();
			// Check for hits
			if (!isInterpolating)
				Physics.Simulate(UpdateLoop.TimePerUpdate);
			foreach (Hitbox hitbox in _hitboxes)
			{
				if (hitbox.isActiveAndEnabled)
				{
					foreach (Hurtbox hurtbox in _hurtboxes)
					{
						if (hurtbox.isActiveAndEnabled && hitbox.CanHit(hurtbox))
						{
							hitbox.OnHit(hurtbox);
							hurtbox.OnHurt(hitbox);
						}
					}
				}
			}
			Scene.I.entityManager.CheckEntityInteractions();
			Scene.I.entityManager.LateUpdateEntities();
			Scene.I.entityManager.SpawnAndDespawnEntities();
			if (isFinalUpdateThisFrame)
				Scene.I.entityManager.RenderEntities();
		}

		public void RegisterHitbox(Hitbox hitbox)
		{
			_hitboxesToAdd.Add(hitbox);
			_hitboxesToRemove.Remove(hitbox);
		}

		public void UnregisterHitbox(Hitbox hitbox)
		{
			_hitboxesToRemove.Add(hitbox);
			_hitboxesToAdd.Remove(hitbox);
		}

		public void RegisterHurtbox(Hurtbox hurtbox)
		{
			_hurtboxesToAdd.Add(hurtbox);
			_hurtboxesToRemove.Remove(hurtbox);
		}

		public void UnregisterHurtbox(Hurtbox hurtbox)
		{
			_hurtboxesToRemove.Add(hurtbox);
			_hurtboxesToAdd.Remove(hurtbox);
		}
	}
}