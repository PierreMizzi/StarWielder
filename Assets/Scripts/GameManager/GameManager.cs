using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameChannel m_gameChannel = null;

	private bool m_hasGameStarted;

	private void CallbackonFirstDocking()
	{
		if (!m_hasGameStarted)
		{
			m_hasGameStarted = true;
			m_gameChannel.onStartGame.Invoke();
			StartTimer();
		}
	}

	private void CallbackGameOver(GameOverReason reason)
	{
		StopTimer();

		GameOverData data = new GameOverData
		{
			reason = reason,
			time = m_currentTime,
			starEnergy = m_highestEnergy
		};

		m_gameChannel.onGameOverScreen(data);
	}

	[ContextMenu("Replay")]
	public void CallbackReplay()
	{
		SceneManager.LoadScene("Game");
	}

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

	#region MonoBehaviour

	private void Start()
	{
		if (m_gameChannel != null)
		{
			m_gameChannel.onSetHighestEnergy += CallbackSetHighestEnergy;

			m_gameChannel.onFirstDocking += CallbackonFirstDocking;
			m_gameChannel.onGameOver += CallbackGameOver;
			m_gameChannel.onReplay += CallbackReplay;
		}
	}

	private void OnDestroy()
	{
		if (m_gameChannel != null)
		{
			m_gameChannel.onSetHighestEnergy -= CallbackSetHighestEnergy;

			m_gameChannel.onFirstDocking -= CallbackonFirstDocking;
			m_gameChannel.onGameOver -= CallbackGameOver;
			m_gameChannel.onReplay -= CallbackReplay;
		}
	}





	#endregion

}