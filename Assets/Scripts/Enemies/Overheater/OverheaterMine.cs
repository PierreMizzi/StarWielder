using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	public class OverheaterMine : MonoBehaviour
	{
		private OverheaterMineSpawner m_spawner;

		public void Initialize(OverheaterMineSpawner spawner)
		{
			m_spawner = spawner;
		}

		public void Kill()
		{
			m_spawner.hasMine = false;
		}
	}
}