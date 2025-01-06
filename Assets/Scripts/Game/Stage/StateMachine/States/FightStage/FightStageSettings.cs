using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay
{
	[CreateAssetMenu(fileName = "FightStageSettings", menuName = "StarWielder/StageSettings/Fight", order = 0)]
	public class FightStageSettings : StageSettings
	{

		public FightStageSettings()
		{
			m_type = StageStateType.Fight;
		}

		[Header("Settings")]
		public int beginningEnemiesCount = 2;
		public int stageEnemiesCount = 6;
		public float minSpawnDelay = 4f;
		public float maxSpawnDelay = 5.5f;

	}
}