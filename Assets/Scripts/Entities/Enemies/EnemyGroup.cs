using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

	[SerializeField] private List<Enemy> m_enemies;

	private void Awake()
	{
		foreach (Enemy enemy in m_enemies)
		{
			enemy.Initialize(this);
		}
	}

	public void DestroyEnemy(Enemy enemy)
	{
		if (m_enemies.Contains(enemy))
			m_enemies.Remove(enemy);

		if (m_enemies.Count == 0)
			Destroy(gameObject);
	}

}