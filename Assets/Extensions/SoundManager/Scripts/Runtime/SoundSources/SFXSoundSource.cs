using UnityEngine;

namespace PierreMizzi.SoundManager
{
	[RequireComponent(typeof(AudioSource))]
	public class SFXSoundSource : SoundSource
	{

		#region Fields 

		public override void Play()
		{
			m_audioSource.volume = 1f;
			base.Play();
		}

		private void Release()
		{
			UnsetSoundData();
			SoundManager.ReleaseSFXSSToPool(this);
		}

		protected override void AudioClipEnded()
		{
			base.AudioClipEnded();
			Release();
		}

		protected override void FadeOutComplete()
		{
			base.FadeOutComplete();
			Release();
		}

		#endregion

	}
}