using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameChannel m_gameChannel = null;

	private void CallbackGameOver()
	{

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
			m_gameChannel.onGameOver -= CallbackGameOver;
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
			m_gameChannel.onGameOver += CallbackGameOver;
		}
	}



	#endregion

}