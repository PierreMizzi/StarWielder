using System.Collections.Generic;
using UnityEngine;

/*

	The game reaches different stages as you play along
	One stage is completed after a certain amount of time
	Before an "enemy" stage, 3 enemies are selected and will spawn during the stage

	- Type of stages
		- Fight
		- Shop
		- Resources

*/

namespace StarWielder.Gameplay
{

	public class LevelManager : MonoBehaviour
	{
		#region MonoBehaviour

		[ContextMenu("Awake")]
		public void Awake()
		{
			BuildStageOrder();
		}

		#endregion


		#region Stage Building

		[Header("Stage Building")]
		[SerializeField] private int m_stagesAmount = 6;

		private List<StageType> m_stages = new List<StageType>();

		private void BuildStageOrder()
		{
			m_stages.Clear();

			List<int> availableStageIndex = new List<int>();

			for (int i = 0; i < m_stagesAmount; i++)
			{
				m_stages.Add(StageType.Fight);

				if (i != 0 && i != m_stagesAmount - 1)
					availableStageIndex.Add(i);
			}

			// Add Resource stage
			InsertStage(StageType.Resources, availableStageIndex);
			InsertStage(StageType.Resources, availableStageIndex);

			string stagesLog = "";
			foreach (StageType stage in m_stages)
			{
				stagesLog += stage.ToString() + " -> ";
			}

			stagesLog = stagesLog.Remove(stagesLog.Length - 4, 4);
			Debug.Log(stagesLog);

		}

		private void InsertStage(StageType stageType, List<int> availableStageIndex)
		{
			int rndResourceStageIndex = UnityEngine.Random.Range(0, availableStageIndex.Count);
			rndResourceStageIndex = availableStageIndex[rndResourceStageIndex];
			availableStageIndex.Remove(rndResourceStageIndex);
			m_stages[rndResourceStageIndex] = stageType;
		}

		#endregion

	}
}