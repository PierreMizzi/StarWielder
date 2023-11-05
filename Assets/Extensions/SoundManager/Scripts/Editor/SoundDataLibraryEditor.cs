using UnityEngine;
using UnityEditor;

namespace PierreMizzi.SoundManager
{

	[CanEditMultipleObjects]
	[CustomEditor(typeof(SoundDataLibrary))]
	public class SoundDataLibraryEditor : Editor
	{

		private SoundDataLibrary m_target = null;

		private void OnEnable()
		{
			m_target = target as SoundDataLibrary;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Fast Fill Library"))
				FastFillLibrary();
		}

		public void FastFillLibrary()
		{
			if (m_target.audioClips.Count == 0)
				return;

			foreach (AudioClip clip in m_target.audioClips)
			{
				SoundData data = new SoundData(clip.name, clip, m_target.mixerGroup);
				m_target.SoundDatas.Add(data);
			}

			m_target.audioClips.Clear();
		}
	}
}