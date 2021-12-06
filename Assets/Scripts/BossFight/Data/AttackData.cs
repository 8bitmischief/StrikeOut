using System.Collections.Generic;
using UnityEngine;

namespace StrikeOut.BossFight.Data
{
	[CreateAssetMenu(menuName = "Strike Out/Data/Attack Data", fileName = "Attack")]
	public class AttackData : ScriptableObject
	{
		public int activeFrames = 1;
		public TargetType target = TargetType.RelativeBatterAreas;
		public List<BatterArea> areas = new List<BatterArea>();
		public List<RelativeBatterArea> relativeAreas = new List<RelativeBatterArea>();

		public enum TargetType
		{
			BatterAreas = 1,
			RelativeBatterAreas = 2,
		}
	}
}