using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	public class OverheaterMine : MonoBehaviour
	{
		private OverheaterMineSpawner m_spawner;

		[SerializeField] private StarAbsorbable m_starAbsorbable;

		public void Initialize(OverheaterMineSpawner spawner)
		{
			m_spawner = spawner;
		}

		public void Kill()
		{
			m_spawner.hasMine = false;
			Destroy(gameObject);
		}

		private void Start()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb += Kill;
		}

		private void OnDestroy()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb -= Kill;
		}
	}
}