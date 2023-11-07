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

		Awake();

		foreach (Enemy enemyStar in m_enemies)
			enemyStar.Initialize(this);

		foreach (EnemyTurret turret in m_turrets)
			turret.Initialize(this);
	}

	public void EnemyStarDestroyed(Enemy enemyStar)
	{
		if (m_enemies.Contains(enemyStar))
			m_enemies.Remove(enemyStar);

		if (m_enemies.Count == 0)
			Destroy(gameObject);
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		m_area = GetComponent<Collider2D>();
		m_animator = GetComponent<Animator>();
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


	[ContextMenu("CheckIsOverlaping")]
	public bool CheckIsOverlaping()
	{
		List<Collider2D> results = new List<Collider2D>();
		int resultsLength = m_area.OverlapCollider(m_overlapingFilter, results);
		// Debug.Log(resultsLength);
		return resultsLength > 0;
	}

	#endregion

	#region Turrets

	[SerializeField] private List<EnemyTurret> m_turrets;

	private void ActivateTurrets()
	{
		foreach (EnemyTurret turret in m_turrets)
			turret.Activate();
	}

	public void DeactivateTurrets()
	{
		foreach (EnemyTurret turret in m_turrets)
			turret.Deactivate();
	}

	#endregion

	#region Animations

	private Animator m_animator = null;

	private const string k_triggerAppear = "Appear";
	private const string m_triggerDestroy = "Destroy";

	public void Appear()
	{
		m_animator.SetTrigger(k_triggerAppear);
	}

	public void AnimEventAppearCompleted()
	{
		ActivateTurrets();
	}

	#endregion


}