using System;
using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(SwordsmanPitcherAIController), true)]
	public class SwordsmanPitcherAIControllerEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			SwordsmanPitcherAIController pitcher = (SwordsmanPitcherAIController) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Pitcher AI State", EditorStyles.boldLabel);
			EditorGUILayout.TextField("Commands", String.Join("\n", pitcher.commands), GUILayout.Height(60f));
			base.DrawState();
		}
	}
}