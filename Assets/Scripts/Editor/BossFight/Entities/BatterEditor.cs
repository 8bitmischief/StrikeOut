using UnityEngine;
using UnityEditor;
using SharedUnityMischief;

namespace StrikeOut {
	[CustomEditor(typeof(Batter), true)]
	public class BatterEditor : BaseEditor {
		public override bool RequiresConstantRepaint () => Application.isPlaying;

		protected override void DrawState () {
			Batter batter = (Batter) target;

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Batter State", EditorStyles.boldLabel);
			EditorGUILayout.Toggle("Is On Right Side", batter.isOnRightSide);
			base.DrawState();
		}
	}
}