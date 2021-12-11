using System;
using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(PitcherAIController), true)]
	public class PitcherAIControllerEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			PitcherAIController pitcher = (PitcherAIController) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Pitcher AI State", EditorStyles.boldLabel);
			EditorGUILayout.TextField("Commands", String.Join("\n", pitcher.commands), GUILayout.Height(60f));
			base.DrawState();
		}
	}
}