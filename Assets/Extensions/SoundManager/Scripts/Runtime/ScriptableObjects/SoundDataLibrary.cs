using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace PierreMizzi.SoundManager
{

	/// <summary>
	/// Collection of a specific type of SoundDatas
	/// </summary>
	[CreateAssetMenu(fileName = "SoundDataLibrary", menuName = "Extensions/AudioManager/SoundDataLibrary", order = 1)]
	public class SoundDataLibrary : ScriptableObject
	{

		#region Fields

		[Tooltip("Short description of the library")]
		[SerializeField, TextArea(0, 10)] private string m_description = "";

		[Tooltip("Type of library")]
		[SerializeField] private SoundType m_type = SoundType.None;

		[Tooltip("Library data of SoundDatas")]
		[Space(40)]
		[SerializeField] private List<SoundData> m_soundDatas = null;

		[SerializeField] private bool m_generateStatic = true;

		public string Description => m_description;
		public List<SoundData> SoundDatas => m_soundDatas;
		public bool GenerateStatic => m_generateStatic;

		[Header("Fast fill")]
		[SerializeField] private List<AudioClip> m_audioClips = new List<AudioClip>();
		[SerializeField] private AudioMixerGroup m_mixerGroup = null;

		public List<AudioClip> audioClips => m_audioClips;
		public AudioMixerGroup mixerGroup => m_mixerGroup;

		#endregion

	}

}