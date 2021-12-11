using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(Batter), true)]
	public class BatterEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			Batter batter = (Batter) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Batter State", EditorStyles.boldLabel);
			EditorGUILayout.IntField("Health", batter.health);
			EditorGUILayout.IntField("Lives", batter.lives);
			EditorGUILayout.TextField("Area", batter.area.ToString());
			EditorGUILayout.TextField("Destination", batter.destinationArea.ToString());
			EditorGUILayout.Toggle("Is On Right Side", batter.isOnRightSide);
			base.DrawState();
		}
	}
}