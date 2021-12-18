using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(SwordsmanPitcher), true)]
	public class SwordsmanPitcherEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			SwordsmanPitcher pitcher = (SwordsmanPitcher) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Pitcher State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Idle", pitcher.isIdle);
			EditorGUILayout.FloatField("Idle Time", pitcher.idleTime);
			base.DrawState();
		}
	}
}