using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageStateManager : MonoBehaviour
	{

		[SerializeField] protected PlayerChannel m_playerChannel;

		public Action onStageEnded;

		protected bool m_isActive;

		public virtual void StartStage()
		{ 
			m_isActive = true;
		}

		public virtual void StopStage()
		{
			m_isActive = false;
			onStageEnded.Invoke();
		}

		public virtual void CallbackGameOver() 
		{
			m_isActive = false;
		}

	}
}