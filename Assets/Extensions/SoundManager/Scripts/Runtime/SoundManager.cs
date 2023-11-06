using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/*

Type de SoundSource



*/
namespace PierreMizzi.SoundManager
{

	/// <summary>
	/// Control class
	/// </summary>
	public static class SoundManager
	{

		#region Fields

		#region Behaviour

		public static SoundManagerToolSettings settings = null;
		private static List<SoundDataLibrary> SoundDataLibraries { get { return settings.SoundDataLibraries; } }

		public static bool m_isInitialized = false;

		#endregion

		#region SFXSoundSources

		private static string k_SFXSSContainerName = "SFXSSContainer";
		private static Transform m_SFXSSContainer;
		private static ObjectPool<SFXSoundSource> m_SFXSSPool;
		private static List<SFXSoundSource> m_ativeSFXSSs;

		#endregion

		#region Global Settings

		private static float BaseFadeInDuration { get { return settings.BaseFadeInDuration; } }
		private static float BaseFadeOutDuration { get { return settings.BaseFadeOutDuration; } }

		private static readonly float UNMUTE_VOLUME = 0f;
		private static readonly float MUTE_VOLUME = -80f;
		public static readonly string k_soundManagerToolSettings = "SoundManagerToolSettings";

		#endregion

		#endregion

		#region Methods

		#region Behaviour

		/// <summary>
		/// Initialize the AudioManager
		/// </summary>
		public static void Init(string pathToSettings)
		{
			/// Settings
			settings = Resources.Load(pathToSettings + "/" + k_soundManagerToolSettings) as SoundManagerToolSettings;

			if (settings == null)
			{
				Debug.LogErrorFormat("SoundManagerTool is not properly initialized");
				return;
			}

			// SFXSoundSources
			InitializeSFXSSPool();

			m_isInitialized = true;
			Debug.Log("SoundManagerTool is initialized !");
		}

		#endregion

		#region SFX

		#region Pooling

		public static void InitializeSFXSSPool()
		{
			// Transform container
			GameObject container = GameObject.Find(k_SFXSSContainerName);
			if (container != null)
				m_SFXSSContainer = container.transform;
			else
			{
				m_SFXSSContainer = new GameObject(k_SFXSSContainerName).transform;
				Debug.LogError("couldn't find SFXSSContainer, created it");
			}
			UnityEngine.Object.DontDestroyOnLoad(m_SFXSSContainer.gameObject);

			// Pool
			m_SFXSSPool = new ObjectPool<SFXSoundSource>(
				CreateSFXSS,
				GetSFXSS,
				ReleaseSFXSS,
				DestroySFXSS,
				true,
				settings.SFXSSPoolDefaultSize,
				settings.SFXSSPoolMaxSize
			);

			m_ativeSFXSSs = new List<SFXSoundSource>();
		}

		private static SFXSoundSource CreateSFXSS()
		{
			GameObject gameObject = new GameObject("SFXSS_" + m_SFXSSPool.CountAll + 1, typeof(SFXSoundSource));
			gameObject.transform.SetParent(m_SFXSSContainer.transform);

			return gameObject.GetComponent<SFXSoundSource>();
		}

		private static void GetSFXSS(SFXSoundSource soundSource)
		{
			soundSource.gameObject.SetActive(true);
			if (!m_ativeSFXSSs.Contains(soundSource))
				m_ativeSFXSSs.Add(soundSource);
		}

		private static void ReleaseSFXSS(SFXSoundSource soundSource)
		{
			soundSource.gameObject.SetActive(false);
			if (m_ativeSFXSSs.Contains(soundSource))
				m_ativeSFXSSs.Remove(soundSource);
		}

		private static void DestroySFXSS(SFXSoundSource soundSource)
		{
			UnityEngine.Object.Destroy(soundSource.gameObject);
			if (m_ativeSFXSSs.Contains(soundSource))
				m_ativeSFXSSs.Remove(soundSource);
		}

		#endregion

		public static SFXSoundSource PlaySFX(string soundDataID)
		{
			if (!m_isInitialized)
				return null;

			// Check if all libraries has this ID stored
			SoundData soundData = GetSoundData(soundDataID);
			if (soundData == null)
			{
				Debug.LogError(string.Format("SoundData with ID {0} doesn't exists", soundDataID));
				return null;
			}

			// Find a non playing SoundSource to play the sound
			SFXSoundSource source = m_SFXSSPool.Get();
			source.SetSoundData(soundData);
			source.Play();
			return source;
		}

		public static SFXSoundSource PlayRandomSFX(List<string> soundDataIDs)
		{
			int randomIndex = UnityEngine.Random.Range(0, soundDataIDs.Count);

			string randomID = soundDataIDs[randomIndex];

			return PlaySFX(randomID);
		}

		public static void ReleaseSFXSSToPool(SFXSoundSource soundSource)
		{
			m_SFXSSPool.Release(soundSource);
		}

		#endregion

		#region SoundData

		/// <summary>
		/// Get the SoundData with given ID
		/// </summary>
		/// <param name="ID"> ID of the SoundData</param>
		/// <returns></returns>
		public static SoundData GetSoundData(string ID)
		{
			if (!m_isInitialized)
				return null;

			SoundData sound = null;

			foreach (SoundDataLibrary library in SoundDataLibraries)
			{
				sound = library.SoundDatas.Find(data => data.ID == ID);

				if (sound != null)
					return sound;
			}

			Debug.LogError(string.Format("There is no SoundData with ID {0} in libraries", ID));
			return null;
		}

		#endregion

		#region Mute / Unmute

		/// <summary>
		/// Mute or Unmute SoundType, linked to AudioMixerController
		/// </summary>
		/// <param name="type"></param>
		/// <param name="muteOrUnmute"></param>
		public static void MuteUnmuteSoundType(SoundType type, bool muteOrUnmute)
		{
			AudioMixerController mixer = MixerControllerFromSoundType(type);
			mixer?.SetMasterVolume(muteOrUnmute ? MUTE_VOLUME : UNMUTE_VOLUME);
		}

		private static AudioMixerController MixerControllerFromSoundType(SoundType type)
		{
			return settings.AudioMixerControllers.Find(mixer => mixer.Type == type);
		}

		#endregion

		#endregion

	}

}
