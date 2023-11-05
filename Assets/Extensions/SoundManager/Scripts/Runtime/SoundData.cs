using System;
using UnityEngine;
using UnityEngine.Audio;

namespace PierreMizzi.SoundManager
{
	/// <summary>
	/// Custom class for an AudioClip
	/// </summary>
	[Serializable]
	public class SoundData
	{
		[SerializeField] private string m_ID = "";
		[SerializeField] private AudioClip m_clip = null;
		[SerializeField] private AudioMixerGroup m_mixer = null;

		public string ID { get { return m_ID; } }
		public AudioClip Clip { get { return m_clip; } }
		public AudioMixerGroup Mixer { get { return m_mixer; } }

		public SoundData(string ID, AudioClip clip, AudioMixerGroup mixer)
		{
			m_ID = ID;
			m_clip = clip;
			m_mixer = mixer;
		}

	}

}