using System;

namespace StarWielder.Gameplay
{
	[Serializable]
	public class FightStageData : ICloneable
	{

		public int enemiesCount;
		public float minSpawnDelay;
		public float maxSpawnDelay;

		public FightStageData(FightStageData data)
		{
			this.enemiesCount = data.enemiesCount;
			this.minSpawnDelay = data.minSpawnDelay;
			this.maxSpawnDelay = data.maxSpawnDelay;
		}

		public object Clone()
		{
			return new FightStageData(this);
		}
	}
}
