using UnityEngine;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameChannel m_gameChannel = null;

	#region Time

	private float m_currentTime;

	private void ManageTimer()
	{
		m_currentTime += Time.deltaTime;
		m_gameChannel.onRefreshTimer.Invoke(m_currentTime);
	}

	#endregion

	private void Update()
	{
		ManageTimer();
	}

}