using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut.BossFight.Entities
{
	[CustomEditor(typeof(Pitcher), true)]
	public class PitcherEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			Pitcher pitcher = (Pitcher) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Pitcher State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is Idle", pitcher.isIdle);
			EditorGUILayout.FloatField("Idle Time", pitcher.idleTime);
			base.DrawState();
		}
	}
}