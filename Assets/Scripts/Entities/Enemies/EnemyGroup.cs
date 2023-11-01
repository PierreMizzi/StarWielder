using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

	#region Base

	[SerializeField] private List<Enemy> m_enemies;
	private EnemyManager m_manager;

	public void Initialize(EnemyManager manager)
	{
		m_manager = manager;
	}

	public void EnemyDestroyed(Enemy enemy)
	{
		if (m_enemies.Contains(enemy))
			m_enemies.Remove(enemy);

		if (m_enemies.Count == 0)
			Destroy(gameObject);
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		foreach (Enemy enemy in m_enemies)
			enemy.Initialize(this);

		m_area = GetComponent<CircleCollider2D>();
		m_areaSqrRadius = Mathf.Pow(m_area.radius, 2f);
	}

	private void OnDestroy()
	{
		m_manager.RemoveEnemyGroup(this);
	}

	#endregion

	#region Area & Spawning

	[Header("Area & Spawning")]
	private CircleCollider2D m_area = null;
	private float m_areaSqrRadius;

	public bool CheckInsideArea(Vector3 position)
	{
		Vector3 areaPosition = new Vector3(m_area.offset.x, m_area.offset.y, 0);
		areaPosition += transform.position;

		float distance = (position - areaPosition).sqrMagnitude;

		return distance < m_areaSqrRadius;
	}

	#endregion

}