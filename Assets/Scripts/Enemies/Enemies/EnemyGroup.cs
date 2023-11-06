using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

	#region Main

	[SerializeField] private List<Enemy> m_enemies;
	private EnemyManager m_manager;
	public EnemyManager manager => m_manager;

	public void Initialize(EnemyManager manager)
	{
		m_manager = manager;

		m_area = GetComponent<Collider2D>();

		foreach (Enemy enemy in m_enemies)
			enemy.Initialize(this);

		foreach (EnemyTurret turret in m_turrets)
			turret.Initialize(this);
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

	private void OnDestroy()
	{
		m_manager.RemoveEnemyGroup(this);
	}

	#endregion

	#region Area & Spawning

	[Header("Area & Spawning")]
	private Collider2D m_area;
	[SerializeField] private ContactFilter2D m_overlapingFilter;


	[ContextMenu("CheckIsOverlaping")]
	public bool CheckIsOverlaping()
	{
		List<Collider2D> results = new List<Collider2D>();
		int resultsLength = m_area.OverlapCollider(m_overlapingFilter, results);
		Debug.Log(resultsLength);
		return resultsLength > 0;
	}

	#endregion

	#region Turrets

	[SerializeField] private List<EnemyTurret> m_turrets;

	public void Deactivate()
	{
		foreach (EnemyTurret turret in m_turrets)
			turret.Deactivate();
	}

	#endregion


}