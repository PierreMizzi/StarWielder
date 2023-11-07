using UnityEngine;

namespace PierreMizzi.SoundManager
{
	[RequireComponent(typeof(AudioSource))]
	public class SFXSoundSource : SoundSource
	{

		#region Fields 

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