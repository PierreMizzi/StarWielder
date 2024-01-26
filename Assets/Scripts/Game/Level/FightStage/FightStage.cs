using UnityEngine;

/*
	- Pick 3 types of enemies based on difficulty
	- Gives them to the EnemyManager
	- Stage starts, spawning starts
	- When all enemies have been killed -> Stage completed
*/

namespace StarWielder.Gameplay
{
	public class FightStage : Stage
	{
		[SerializeField] private GameChannel m_gameChannel;

		public FightStage(int enemiesCount, float minSpawnDelay, float maxSpawnDelay)
		{
			stageType = StageType.Fight;
			this.m_enemiesCount = enemiesCount;
		}

		#region MonoBehaviour

		private void Start()
		{

		}

		private void OnDestroy()
		{

		}

		#endregion

		#region Name

		private int m_enemiesCount;
		private float m_minSpawnDelay;
		private float m_maxSpawnDelay;

		#endregion

	}
}