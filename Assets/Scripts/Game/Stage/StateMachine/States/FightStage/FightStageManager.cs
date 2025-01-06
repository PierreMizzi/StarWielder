using StarWielder.Gameplay.Enemies;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class FightStageManager : StageStateManager
	{

		public virtual void StartStage(FightStageSettings settings)
		{
			m_enemyManager.SetupStageSettings(settings);
			m_enemyManager.StartStage();
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
