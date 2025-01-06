using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageStateManager : MonoBehaviour
	{

		[SerializeField] protected PlayerChannel m_playerChannel;

		public Action onStageEnded;

		public virtual void StartStage() { }

		public virtual void StopStage()
		{
			onStageEnded.Invoke();
		}

	}
}