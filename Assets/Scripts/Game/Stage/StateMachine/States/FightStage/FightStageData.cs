using System;

namespace StarWielder.Gameplay
{
	[Serializable]
	public class FightStageData : ICloneable
	{
		public int beginningEnemiesCount;
		public int stageEnemiesCount;
		public float minSpawnDelay;
		public float maxSpawnDelay;

		public FightStageData(FightStageData data)
		{
			stageEnemiesCount = data.stageEnemiesCount;
			minSpawnDelay = data.minSpawnDelay;
			maxSpawnDelay = data.maxSpawnDelay;
			beginningEnemiesCount = data.beginningEnemiesCount;
		}

		public object Clone()
		{
			return new FightStageData(this);
		}
	}
}
