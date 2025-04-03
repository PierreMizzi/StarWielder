using StarWielder.Gameplay;
using StarWielder.Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

public class EventSunSocketBehaviour : MonoBehaviour, ISunSocketBehaviour
{
	#region MonoBehaviour

	[SerializeField] private UnityEvent m_onSocket;
	[SerializeField] private UnityEvent m_onUnsocket;

	protected void Start()
	{
		(this as ISunSocketBehaviour).Subscribe();
	}

	protected void OnDestroy()
	{
		(this as ISunSocketBehaviour).Unsubscribe();
	}

	#endregion

	#region ISunSocketBehaviour

	public SunSocket sunSocket { get; set; }

	public void CallbackSocket(Star sun)
	{
		m_onSocket.Invoke();
	}

	public void CallbackUnsocket(Star sun)
	{
		m_onUnsocket.Invoke();
	}

	#endregion
}