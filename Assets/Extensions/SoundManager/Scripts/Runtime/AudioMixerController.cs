using UnityEngine;
using System;
using UnityEngine.Audio;

namespace PierreMizzi.SoundManager
{

	[Serializable]
	public class AudioMixerController
	{

		#region Fields

		[SerializeField] private string m_name = "";
		[SerializeField] private SoundType m_type = SoundType.None;
		[SerializeField] private AudioMixer m_mixer = null;

		public const string FLOAT_MASTER_VOLUME = "MasterVolume";

		public string Name { get { return m_name; } }
		public SoundType Type { get { return m_type; } }
		public AudioMixer Mixer { get { return m_mixer; } }

		#endregion

		#region Methods

		public void SetMasterVolume(float value)
		{
			m_mixer.SetFloat(FLOAT_MASTER_VOLUME, value);
		}

		#endregion

	}

}