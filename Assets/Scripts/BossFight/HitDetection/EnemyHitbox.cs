using UnityEngine;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight.Entities
{
	[RequireComponent(typeof(BoxCollider))]
	public class EnemyHitbox : Hitbox
	{
		[SerializeField] private bool _hitsCenter = false;
		[SerializeField] private bool _hitsSides = true;
		[SerializeField] private bool _hitsFarSides = false;

		protected override bool CanHit(Hurtbox hurtbox)
		{
			if (base.CanHit(hurtbox))
			{
				if (hurtbox.entity == Scene.I.batter)
				{
					BatterArea area = Scene.I.batter.area;
					BatterArea destinationArea = Scene.I.batter.destinationArea;
					if (!_hitsSides && (area == BatterArea.Left || area == BatterArea.Right || destinationArea == BatterArea.Left || destinationArea == BatterArea.Right))
						return false;
					else if (!_hitsFarSides && (area == BatterArea.FarLeft || area == BatterArea.FarRight || destinationArea == BatterArea.FarLeft || destinationArea == BatterArea.FarRight))
						return false;
					else if (!_hitsCenter && (area == BatterArea.Center || destinationArea == BatterArea.Center))
						return false;
					else
						return true;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}
	}
}