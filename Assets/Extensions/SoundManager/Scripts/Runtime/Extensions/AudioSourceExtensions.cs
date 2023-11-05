using UnityEngine;
using System;

namespace PierreMizzi.SoundManager
{
	public static class AudioSourceExtensions
	{
		public static void SetSoundData(this AudioSource audioSource, SoundData soundData)
		{
			audioSource.clip = soundData.Clip;
			audioSource.outputAudioMixerGroup = soundData.Mixer;
		}
	}
}