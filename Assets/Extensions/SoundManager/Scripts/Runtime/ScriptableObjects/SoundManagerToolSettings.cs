using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.SoundManager
{
	[CreateAssetMenu(fileName = "SoundManagerToolSettings", menuName = "Extensions/AudioManager/SoundManagerToolSettings", order = 1)]
	public class SoundManagerToolSettings : ScriptableObject
	{

		#region Fields

		#region Settings

		[SerializeField] private string _path = "";
		public string path { get { return _path; } }


		[Header("AudioMixer")]
		[SerializeField] private List<AudioMixerController> m_audioMixerControllers = new List<AudioMixerController>();
		public List<AudioMixerController> AudioMixerControllers { get { return m_audioMixerControllers; } }

		[Header("SoundData Libraries")]
		[SerializeField] private List<SoundDataLibrary> m_soundDataLibraries = null;
		public List<SoundDataLibrary> SoundDataLibraries { get { return m_soundDataLibraries; } }

		[Header("Fading in & out")]
		[Tooltip("Base duration when fading in a sound")]
		[SerializeField] private float m_baseFadeInDuration = 1f;

		[Tooltip("Base duration when fading out a sound")]
		[SerializeField] private float m_baseFadeOutDuration = 1f;

		public float BaseFadeInDuration { get { return m_baseFadeInDuration; } }
		public float BaseFadeOutDuration { get { return m_baseFadeOutDuration; } }

		[Header("SFX SoundSources (SFXSS) Pool")]
		[SerializeField]
		private int m_SFXSSPoolMaxSize = 10;
		[SerializeField]
		private int m_SFXSSPoolDefaultSize = 10;

		public int SFXSSPoolMaxSize { get { return m_SFXSSPoolMaxSize; } }
		public int SFXSSPoolDefaultSize { get { return m_SFXSSPoolDefaultSize; } }

		#endregion

		#endregion

	}

}