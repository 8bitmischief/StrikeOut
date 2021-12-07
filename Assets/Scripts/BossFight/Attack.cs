using System.Collections.Generic;
using UnityEngine;
using SharedUnityMischief.Entities;
using StrikeOut.BossFight.Data;

namespace StrikeOut.BossFight
{
	public class Attack
	{
		private AttackData _definition;
		private Entity _entity;
		private int _activeFramesLeft;
		private List<BatterArea> _areas;
		private bool _hasDealtDamage = false;

		public bool isDone => _activeFramesLeft == 0 || !_entity.isSpawned || _hasDealtDamage;

		public Attack(AttackData definition, Entity entity)
		{
			_definition = definition;
			_entity = entity;
			_activeFramesLeft = definition.activeFrames;
			switch (definition.target)
			{
				case AttackData.TargetType.BatterAreas:
					_areas = new List<BatterArea>(definition.areas);
					break;
				case AttackData.TargetType.RelativeBatterAreas:
					bool isOnRightSide = entity.transform.position.x >= Scene.I.locations.batter.center.x;
					_areas = new List<BatterArea>();
					foreach (RelativeBatterArea area in definition.relativeAreas)
					{
						switch (area)
						{
							case RelativeBatterArea.FarSameSide:
								_areas.Add(isOnRightSide ? BatterArea.FarRight : BatterArea.FarLeft);
								break;
							case RelativeBatterArea.SameSide:
								_areas.Add(isOnRightSide ? BatterArea.Right : BatterArea.Left);
								break;
							case RelativeBatterArea.Center:
								_areas.Add(BatterArea.Center);
								break;
							case RelativeBatterArea.OppositeSide:
								_areas.Add(isOnRightSide ? BatterArea.Left : BatterArea.Right);
								break;
							case RelativeBatterArea.FarOppositeSide:
								_areas.Add(isOnRightSide ? BatterArea.FarLeft : BatterArea.FarRight);
								break;
						}
					}
					break;
			}
		}

		public void UpdateState()
		{
			if (!UpdateLoop.I.isInterpolating)
			{
				_activeFramesLeft = Mathf.Max(0, _activeFramesLeft - 1);
				if (!_hasDealtDamage && _areas.Contains(Scene.I.batter.area))
				{
					if (Scene.I.batter.destinationArea == BatterArea.None || _areas.Contains(Scene.I.batter.destinationArea))
					{
						Scene.I.batter.Damage();
						_hasDealtDamage = true;
					}
				}
			}
		}
	}
}