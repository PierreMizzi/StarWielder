using System.Collections;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{

	#region Main

	private EnemyGroup m_group;

	private Transform m_bulletsContainer;

	private bool m_isActive;


	public void Initialize(EnemyGroup group)
	{
		m_group = group;
		m_bulletsContainer = m_group.manager.bulletsContainer;
		m_shipTransform = m_group.manager.ship.transform;

		Activate();
	}

	public void Activate()
	{
		m_isActive = true;
		StartFiring();
	}

	public void Deactivate()
	{
		m_isActive = false;
		StopFiring();
	}

	#endregion

	#region MonoBehaviour

	private void Update()
	{
		if (m_isActive)
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

	#region Firing

	[Header("Firing")]
	// Delay
	[SerializeField] private float m_minDelayFiring = 2f;
	[SerializeField] private float m_maxDelayFiring = 4f;

	// Rate of Fire
	[SerializeField] private float m_rateOfFire = 2f;

	[SerializeField] private EnemyBullet m_bulletPrefab;

	private IEnumerator m_fireCoroutine;

	private void StartFiring()
	{
		if (m_fireCoroutine == null)
		{
			m_fireCoroutine = FireCoroutine();
			StartCoroutine(m_fireCoroutine);
		}
	}

	private void StopFiring()
	{
		if (m_fireCoroutine != null)
		{
			StopCoroutine(m_fireCoroutine);
			m_fireCoroutine = null;
		}
	}

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