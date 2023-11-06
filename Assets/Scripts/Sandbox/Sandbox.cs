using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using System;

public class Sandbox : MonoBehaviour
{

	#region Trigger 2D

	// [SerializeField] private LayerMask m_layerMask;

	// [SerializeField] private Transform m_targetDestination = null;

	// [SerializeField] private float m_duration = 5f;

	// private void Update()
	// {
	// 	if (Input.GetKeyDown(KeyCode.S))
	// 		TweenTarget();
	// }

	// private void OnTriggerEnter2D(Collider2D other)
	// {
	// 	if (UtilsClass.CheckLayer(m_layerMask.value, other.gameObject.layer))
	// 	{
	// 		Debug.Log(other.name);
	// 	}
	// }

	// private void TweenTarget()
	// {
	// 	transform.position = Vector3.zero;
	// 	transform.DOMove(m_targetDestination.position, m_duration);

	// }

	#endregion

	#region Test Physic

	// [SerializeField] private Rigidbody2D m_rigidbody2D;

	// [SerializeField] private Transform m_targetReturn;

	// private int m_previousPositionFrequency = 2;
	// private int m_previousPositionFrame;
	// private Vector3 m_previousPosition;


	// private void Awake()
	// {
	// 	// m_rigidbody2D.velocity = transform.up;
	// }

	// private void Update()
	// {
	// 	if (Input.GetKeyDown(KeyCode.S))
	// 		Force();

	// 	else if (Input.GetKeyDown(KeyCode.T))
	// 		ToTarget();


	// 	if (m_isAimingTarget)
	// 		m_rigidbody2D.AddForce(m_toTargetDirection * m_toTargetAcceleration);
	// 	else
	// 	{
	// 		m_previousPositionFrame++;
	// 		if (m_previousPositionFrame > m_previousPositionFrequency)
	// 		{
	// 			transform.up = (transform.position - m_previousPosition).normalized;

	// 			m_previousPositionFrame = 0;
	// 			m_previousPosition = transform.position;
	// 		}
	// 	}
	// }

	// private void Force()
	// {
	// 	m_rigidbody2D.AddForce(transform.up, ForceMode2D.Impulse);
	// }

	// [SerializeField] private bool m_isAimingTarget;
	// private Vector2 m_toTargetDirection;
	// [SerializeField] private float m_toTargetAcceleration;

	// private void ToTarget()
	// {
	// 	m_rigidbody2D.velocity = Vector2.zero;

	// 	m_isAimingTarget = true;
	// 	m_toTargetDirection = (m_targetReturn.position - transform.position).normalized;
	// 	transform.up = m_toTargetDirection;

	// 	float m_magnitude = (transform.position - m_targetReturn.position).magnitude;

	// 	float duration = Mathf.Sqrt(2f * m_magnitude / m_toTargetAcceleration);
	// 	DOVirtual.DelayedCall(duration, Debug.Break);
	// }


	#endregion

	#region Scale From Velocity

	[SerializeField] private Vector2 m_velocity;




	#endregion

}