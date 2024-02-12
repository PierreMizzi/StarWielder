using System;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageStateManager : MonoBehaviour
	{

		public Action onStageEnded;

		public virtual void StartStage() { }

	}
}