using System;
using StarWielder.Gameplay.Player;
using StarWielder.UI;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{

	public class DialogueSunSocket : MonoBehaviour
	{

		#region Behaviour

		[SerializeField] private DialogueData m_data;
		[SerializeField] private UIChannel m_UIChannel;

		#endregion

		#region MonoBehaviour

		protected void Awake()
		{
			m_sunSocket = GetComponent<SunSocket>();
		}

		private void Start()
		{
			if (TryGetComponent(out SunSocket sunSocket))
			{
				m_sunSocket = sunSocket;
				m_sunSocket.onSocket += CallbackSocket;
				m_sunSocket.onUnsocket += CallbackUnsocket;
			}
		}

		private void OnDestroy()
		{
			if (m_sunSocket != null)
			{
				m_sunSocket.onSocket -= CallbackSocket;
				m_sunSocket.onUnsocket -= CallbackUnsocket;
			}
		}

		#endregion

		#region Sun Socket

		private SunSocket m_sunSocket;

		private void CallbackSocket(Star sun)
		{
			if (m_UIChannel != null)
			{
				m_UIChannel.onDisplayDialogue.Invoke(m_data, transform.position);
			}
		}

		private void CallbackUnsocket(Star sun)
		{
			if (m_UIChannel != null)
			{
				m_UIChannel.onHideDialogue.Invoke();
			}
		}

		#endregion

	}
}