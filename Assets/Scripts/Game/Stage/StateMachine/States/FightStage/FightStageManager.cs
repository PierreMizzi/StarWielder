using StarWielder.Gameplay.Enemies;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class FightStageManager : StageStateManager
	{

		public virtual void StartStage(FightStageData data)
		{
			m_enemyManager.SetupStageData(data);
			m_enemyManager.StartSpawning();
		}

		#region MonoBehaviour

		private void Start()
		{
			m_enemyManager.Initialize(this);
		}

		#endregion

		#region EnemyManager

		[SerializeField] private EnemyManager m_enemyManager;

		#endregion

	}
}
