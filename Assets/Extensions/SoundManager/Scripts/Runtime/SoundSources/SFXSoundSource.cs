using UnityEngine;

namespace PierreMizzi.SoundManager
{
	[RequireComponent(typeof(AudioSource))]
	public class SFXSoundSource : SoundSource
	{

		#region Fields 

		protected override void AudioClipEnded()
		{
			UnsetSoundData();
			onAudioClipEnded?.Invoke();
			SoundManager.ReleaseSFXSSToPool(this);
		}

		#endregion

	}
}