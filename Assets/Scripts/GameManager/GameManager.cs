using System;
using System.Collections;
using PierreMizzi.Extensions.CursorManagement;
using PierreMizzi.SoundManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QGamesTest.Gameplay
{

	/// <summary>
	/// Controls globaly related things, like playing boundaries, Game Start, Game Over & Restarting.
	/// </summary>
	public class GameManager : MonoBehaviour
	{

		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private CursorChannel m_cursorChannel;

		private bool m_hasGameStarted;

		private void CallbackOnFirstDocking()
		{
			if (!m_hasGameStarted)
			{
				m_hasGameStarted = true;
				m_gameChannel.onStartGame.Invoke();
				StartTimer();
				SetNormalCutoff();
			}
		}

		private void CallbackGameOver(GameOverReason reason)
		{
			m_cursorChannel.onSetCursor(CursorType.Normal);

			StopTimer();

			GameOverData data = new GameOverData
			{
				reason = reason,
				time = m_currentTime,
				starEnergy = m_highestEnergy
			};

			m_gameChannel.onGameOverScreen(data);

			SetLowCutoff();
		}

		[ContextMenu("Replay")]
		public void CallbackReplay()
		{
			SceneManager.LoadScene("Game");
		}

		#region MonoBehaviour

		private void Awake()
		{
			InitializeBoundaries();
		}

		private void Start()
		{
			InitializeSoundManager();
			SetLowCutoff();
			m_cursorChannel.onSetCursor(CursorType.Aim);

#if !UNITY_EDITOR
			Cursor.lockState = CursorLockMode.Confined;
#endif

			if (m_gameChannel != null)
			{
				m_gameChannel.onSetHighestEnergy += CallbackSetHighestEnergy;

				m_gameChannel.onFirstDocking += CallbackOnFirstDocking;
				m_gameChannel.onGameOver += CallbackGameOver;
				m_gameChannel.onReplay += CallbackReplay;
			}
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onSetHighestEnergy -= CallbackSetHighestEnergy;

				m_gameChannel.onFirstDocking -= CallbackOnFirstDocking;
				m_gameChannel.onGameOver -= CallbackGameOver;
				m_gameChannel.onReplay -= CallbackReplay;
			}
		}

		#endregion

		#region Boundaries

		private Camera m_camera;
		public static Vector2 topRightBoundCorner;
		public static Vector2 botLeftBoundCorner;

		private void InitializeBoundaries()
		{
			m_camera = Camera.main;

			float m_horizontalSize = m_camera.orthographicSize * m_camera.aspect;
			Vector2 extents = new Vector2(m_horizontalSize, m_camera.orthographicSize);

			topRightBoundCorner = m_camera.transform.position;
			topRightBoundCorner += extents;

			botLeftBoundCorner = m_camera.transform.position;
			botLeftBoundCorner -= extents;

		}

		public static bool CheckInBoundaries(Vector2 topRight, Vector2 botLeft)
		{
			return topRight.x < topRightBoundCorner.x &&
				   topRight.y < topRightBoundCorner.y &&
				   botLeft.x > botLeftBoundCorner.x &&
				   botLeft.y > botLeftBoundCorner.y;
		}

		#endregion

		#region Score

		private float m_currentTime;

		private IEnumerator m_timerCoroutine;

		private float m_highestEnergy;

		private void StartTimer()
		{
			if (m_timerCoroutine == null)
			{
				m_timerCoroutine = TimeCoroutine();
				StartCoroutine(m_timerCoroutine);
			}
		}

		private void StopTimer()
		{
			if (m_timerCoroutine != null)
			{
				StopCoroutine(m_timerCoroutine);
				m_timerCoroutine = null;
			}
		}

		private IEnumerator TimeCoroutine()
		{
			while (true)
			{
				m_currentTime += Time.deltaTime;
				m_gameChannel.onRefreshTimer.Invoke(m_currentTime);
				yield return new WaitForEndOfFrame();
			}
		}

		private void CallbackSetHighestEnergy(float highestEnergy)
		{
			m_highestEnergy = highestEnergy;
		}

		#endregion

		#region Sound Manager

		[Header("Sound Manager")]
		[SerializeField] private SoundManagerToolSettings m_soundSettings = null;

		[ContextMenu("Sound Manager")]
		public void InitializeSoundManager()
		{
			SoundManager.Init("SoundManager");
		}

		#endregion

		#region Main Loop

		[Header("Main Loop")]
		[SerializeField] private SoundSource m_mainLoop;
		private const string k_cutoffParameter = "MainLoopCutoff";
		private const int k_normalCutoffFrequency = 5000;
		private const int k_lowCutoffFrequency = 1000;

		/// <summary>
		/// Sounds is deafen, like in the background
		/// </summary>
		private void SetNormalCutoff()
		{
			m_mainLoop.audioMixerGroup.audioMixer.SetFloat(k_cutoffParameter, k_normalCutoffFrequency);
		}

		/// <summary>
		/// Sounds is normal
		/// </summary>
		private void SetLowCutoff()
		{
			m_mainLoop.audioMixerGroup.audioMixer.SetFloat(k_cutoffParameter, k_lowCutoffFrequency);
		}

		#endregion

	}
}