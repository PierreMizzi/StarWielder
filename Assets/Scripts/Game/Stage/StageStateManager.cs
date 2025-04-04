using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageStateManager : MonoBehaviour
	{

		[SerializeField] protected PlayerChannel m_playerChannel;

		[SerializeField] protected GameObject m_container;

		public Action onStageEnded;

		protected bool m_isActive;

		public virtual void StartStage()
		{ 
			m_isActive = true;
			m_container?.SetActive(true);
		}

		public virtual void StopStage()
		{
			m_isActive = false;
			onStageEnded.Invoke();
			m_container?.SetActive(false);
		}

		public virtual void CallbackGameOver() 
		{
			m_isActive = false;
		}

	}
}