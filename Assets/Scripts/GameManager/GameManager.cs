using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameChannel m_gameChannel = null;

	[ContextMenu("Replay")]
	public void CallbackReplay()
	{
		SceneManager.LoadScene("Game");
	}

	#region Score

	private float m_currentTime;

	private float m_highestEnergy;

	private void ManageTimer()
	{
		m_currentTime += Time.deltaTime;
		m_gameChannel.onRefreshTimer.Invoke(m_currentTime);
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
			m_gameChannel.onGameOver += CallbackGameOver;
			m_gameChannel.onReplay += CallbackReplay;
		}
	}

	private void Update()
	{
		ManageTimer();
	}

	private void OnDestroy()
	{
		if (m_gameChannel != null)
		{
			m_gameChannel.onSetHighestEnergy -= CallbackSetHighestEnergy;
			m_gameChannel.onGameOver -= CallbackGameOver;
			m_gameChannel.onReplay -= CallbackReplay;
		}
	}

	private void CallbackGameOver(GameOverReason reason)
	{
		GameOverData data = new GameOverData
		{
			reason = reason,
			time = m_currentTime,
			starEnergy = m_highestEnergy
		};

		m_gameChannel.onGameOverScreen(data);
	}



	#endregion

}