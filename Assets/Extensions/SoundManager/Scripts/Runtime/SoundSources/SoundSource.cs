using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace PierreMizzi.SoundManager
{

	/// <summary>
	/// Custom behaviour class for Audio Source
	/// </summary>
	[ExecuteInEditMode, RequireComponent(typeof(AudioSource))]
	public class SoundSource : MonoBehaviour
	{

		#region Fields

		protected AudioSource m_audioSource = null;
		public AudioMixerGroup audioMixerGroup => m_audioSource?.outputAudioMixerGroup;

		protected SoundData m_soundData = null;
		public SoundSourceState state { get; private set; }

		public bool IsPlaying
		{
			get
			{
				CheckAudioSource();
				return m_audioSource.isPlaying;
			}
		}

		[SerializeField]
		private string m_soundDataID;

		[SerializeField]
		private bool m_playOnAwake;

		[SerializeField]
		private bool m_fadeInOnAwake;

		#region Callbacks

		public Action onAudioClipEnded;
		public Action onAudioClipLooped;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

		private void Awake()
		{
			CheckAudioSource();
		}

		protected virtual IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			Initialize();
		}

		protected virtual void Update()
		{
			if (m_audioSource != null && m_audioSource.clip != null)
			{
				if ((m_audioSource.clip.length - m_audioSource.time) < 0.01f)
					AudioClipEnded();
			}
		}

		#endregion

		#region Behaviour

		public void Initialize()
		{
			CheckAudioSource();

			if (!string.IsNullOrEmpty(m_soundDataID))
			{
				SetSoundData(m_soundDataID);

				if (m_playOnAwake)
				{
					if (m_fadeInOnAwake)
						FadeIn(SoundManager.settings.BaseFadeInDuration);
					else
						Play();
				}
			}
		}

		#endregion

		#region Control Behaviour

		[ContextMenu("Play")]
		public void Play()
		{
			if (m_audioSource.clip != null)
			{
				m_audioSource.Play();
				state = SoundSourceState.Playing;
			}
		}

		public void Pause()
		{
			if (m_audioSource.isPlaying)
			{
				m_audioSource.Pause();
				state = SoundSourceState.Pause;
			}
		}

		public void Stop()
		{
			m_audioSource.Stop();
			m_audioSource.clip = null;
			m_audioSource.outputAudioMixerGroup = null;
			state = SoundSourceState.None;
		}

		public void FadeInFromZero(float duration, float toVolume = 1)
		{
			m_audioSource.volume = 0;
			FadeIn(duration, toVolume);
		}

		/// <summary>
		/// Fades In the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="fromZero">Volume starts from 0 ?</param>
		/// <param name="callback">Callback when done fading</param>
		public void FadeIn(float duration = 0, float toVolume = 1, Action onComplete = null)
		{
			if (duration == 0)
				duration = SoundManager.settings.BaseFadeOutDuration;

			state = SoundSourceState.FadeIn;

			if (!m_audioSource.isPlaying)
				m_audioSource.Play();

			m_audioSource
				.DOFade(toVolume, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					onComplete?.Invoke();
					state = SoundSourceState.Playing;
				});
		}

		/// <summary>
		/// Fades out the volume of the soundSource
		/// </summary>
		/// <param name="duration">Duration in seconds</param>
		/// <param name="callback">Callback when done fading, default value fades to 0</param>
		public void FadeOut(float duration = 0, float toVolume = 0, Action onComplete = null)
		{
			if (duration == 0)
				duration = SoundManager.settings.BaseFadeOutDuration;

			state = SoundSourceState.FadeOut;

			m_audioSource
				.DOFade(toVolume, duration)
				.SetEase(Ease.Linear)
				.OnComplete(() =>
				{
					onComplete?.Invoke();
					state = SoundSourceState.None;
				});
		}

		public void FadeTransition(string soundDataID, float duration = 0, Action onComplete = null)
		{
			if (duration == 0)
				duration = SoundManager.settings.BaseFadeOutDuration;

			FadeOut(duration, 0, () =>
			{
				SetSoundData(soundDataID);
				FadeIn(0, 1, onComplete);
			});
		}

		public void Restart()
		{
			m_audioSource.Stop();
			m_audioSource.Play();
		}

		#endregion

		#region Callbacks

		protected virtual void AudioClipEnded()
		{
			if (m_audioSource.loop)
				onAudioClipEnded?.Invoke();
			else
				onAudioClipLooped?.Invoke();
		}

		#endregion

		#region Control Settings

		/// <summary>
		/// Set a SoundData to play
		/// </summary>
		/// <param name="data">Given SoundData</param>
		public void SetSoundData(SoundData data)
		{
			CheckAudioSource();

			if (data != null && data.Clip != null)
			{
				m_soundData = data;
				m_audioSource.clip = m_soundData.Clip;
				m_audioSource.outputAudioMixerGroup = m_soundData.Mixer;
			}
		}

		public void SetSoundData(string ID)
		{
			SoundData data = SoundManager.GetSoundData(ID);

			SetSoundData(data);
		}

		public void UnsetSoundData()
		{
			m_soundData = null;
			m_audioSource.clip = null;
			m_audioSource.outputAudioMixerGroup = null;
		}

		public void SetLooping(bool isLooping)
		{
			CheckAudioSource();
			m_audioSource.loop = isLooping;
		}

		public void SetVolume(float volume)
		{
			CheckAudioSource();
			m_audioSource.volume = volume;
		}

		#endregion

		protected void CheckAudioSource()
		{
			if (m_audioSource == null)
				m_audioSource = GetComponent<AudioSource>();
		}

		#endregion

		#region Debug

		public override string ToString()
		{
			string info = "";
			info += string.Format("{0} \r", name);

			if (m_soundData != null)
				info += string.Format("{0} \r", m_soundData.ID);

			return info;
		}

		#endregion

	}


}