using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut
{
	[CustomEditor(typeof(Game), true)]
	public class GameEditor : BaseEditor
	{
		public override bool RequiresConstantRepaint() => Application.isPlaying;

		protected override void DrawState()
		{
			Game game = (Game) target;
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game State", EditorStyles.boldLabel);
			EditorGUILayout.TextField("Scene", game.sceneId.ToString());
			base.DrawState();
		}
	}
}