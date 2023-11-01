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

		m_area = GetComponent<Collider2D>();
		// areaSqrRadius = Mathf.Pow(m_area.radius, 2f);
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
		m_area = GetComponent<Collider2D>();

		foreach (Enemy enemy in m_enemies)
			enemy.Initialize(this);
	}

	private void OnDestroy()
	{
		m_manager.RemoveEnemyGroup(this);
	}

	#endregion

	#region Area & Spawning

	[Header("Area & Spawning")]
	private Collider2D m_area;

	[SerializeField] private ContactFilter2D m_overlapingFilter;

	[HideInInspector] public float areaSqrRadius;

	[ContextMenu("CheckIsOverlaping")]
	public bool CheckIsOverlaping()
	{
		List<Collider2D> results = new List<Collider2D>();
		int resultsLength = m_area.OverlapCollider(m_overlapingFilter, results);
		print(resultsLength);
		return resultsLength > 0;
	}

	public bool CheckNotTooClose(Vector3 otherPosition, float otherSqrRadius)
	{
		Vector3 selfPosition = new Vector3(m_area.offset.x, m_area.offset.y, 0);
		selfPosition += transform.position;

		float distance = (otherPosition - selfPosition).sqrMagnitude;

		return distance < areaSqrRadius + otherSqrRadius;
	}

	#endregion

}