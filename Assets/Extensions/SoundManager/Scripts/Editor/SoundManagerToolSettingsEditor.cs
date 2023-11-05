using UnityEditor;
using UnityEngine;

namespace PierreMizzi.SoundManager
{


	[CanEditMultipleObjects]
	[CustomEditor(typeof(SoundManagerToolSettings))]
	public class SoundManagerToolSettingsEditor : Editor
	{

		private SoundManagerToolSettings m_target = null;

		private void OnEnable()
		{
			m_target = target as SoundManagerToolSettings;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Generate SoundDataID File"))
				SoundDataIDGenerator.WriteFile(m_target.SoundDataLibraries);

		}

	}
}