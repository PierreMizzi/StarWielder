using System.Collections;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{

	#region Main

	private EnemyGroup m_group;

	private Transform m_bulletsContainer;

	public void Initialize(EnemyGroup group)
	{
		m_group = group;
		m_bulletsContainer = m_group.manager.bulletsContainer;
		m_shipTransform = m_group.manager.ship.transform;

		StartCoroutine(FireCoroutine());
	}

	#endregion

	#region MonoBehaviour


	private void Update()
	{
		UpdateCanonRotation();
	}

	#endregion

	#region Canon

	[Header("Canon")]
	[SerializeField] private Transform m_canonAxisTransform;
	[SerializeField] private Transform m_firePosition;

	private Transform m_shipTransform;
	private Vector3 m_directionToShip;

	private void UpdateCanonRotation()
	{
		if (m_shipTransform != null)
		{
			m_directionToShip = (m_shipTransform.position - transform.position).normalized;
			transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(m_directionToShip.y, m_directionToShip.x) * Mathf.Rad2Deg);
		}
	}

	#endregion

	#region Bullets

	[Header("Bullets")]
	// Delay
	[SerializeField] private float m_minDelayFiring = 2f;
	[SerializeField] private float m_maxDelayFiring = 4f;

	// Rate of Fire
	[SerializeField] private float m_rateOfFire = 2f;

	[SerializeField] private EnemyBullet m_bulletPrefab;

	private IEnumerator FireCoroutine()
	{
		float delayFiring = Random.Range(m_minDelayFiring, m_maxDelayFiring);
		yield return new WaitForSeconds(delayFiring);

		while (true)
		{
			FireBullet();
			yield return new WaitForSeconds(m_rateOfFire);
		}
	}

	private void FireBullet()
	{
		EnemyBullet bullet = Instantiate(m_bulletPrefab, m_bulletsContainer);
		bullet.transform.position = m_firePosition.position;
		bullet.transform.up = m_canonAxisTransform.right;
	}

	#endregion
}