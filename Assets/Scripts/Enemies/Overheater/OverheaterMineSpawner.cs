using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace StarWielder.Gameplay.Enemies
{

	[ExecuteInEditMode]
	public class OverheaterMineSpawner : MonoBehaviour
	{

		#region Main

		// TODO : Raycasts mine so it's unavailable
		public bool canSpawn { get { return !hasMine && CheckHasOpenField(); } }

		#endregion

		#region Spawning

		[Header("Spawning")]
		[SerializeField] private float m_minRange = 1f;
		[SerializeField] private float m_maxRange = 2f;
		[SerializeField] private float m_tweenDuration = 1f;

		[SerializeField] private OverheaterMine m_minePrefab;

		public bool hasMine { get; set; }

		private float m_rangeDistance;
		private Vector3 m_rangeStartPosition;
		private Vector3 m_rangeEndPosition;

		[ContextMenu("Spawn Mine")]
		public void SpawnMine()
		{
			m_rangeStartPosition = transform.position + transform.up * m_minRange;
			m_openFieldRay = new Ray(m_rangeStartPosition, transform.up);

			OverheaterMine mine = Instantiate(m_minePrefab, transform.position, Quaternion.identity);
			mine.Initialize(this);
			hasMine = true;
			Vector3 rndPosition = transform.position + transform.up * Random.Range(m_minRange, m_maxRange);

			mine.transform.DOMove(rndPosition, m_tweenDuration).SetEase(Ease.OutCubic);
		}

		#endregion

		#region MonoBehaviour


		private void Awake()
		{
			m_rangeDistance = m_maxRange - m_minRange;
		}

		private void OnValidate()
		{
			m_rangeStartPosition = transform.position + transform.up * m_minRange;
			m_rangeEndPosition = transform.position + transform.up * m_maxRange;
		}

		protected void OnDrawGizmos()
		{
			defaultGizmosColor = Gizmos.color;

			Gizmos.color = m_gizmosColor;

			if (m_rangeStartPosition == Vector3.zero)
				m_rangeStartPosition = transform.position + transform.up * m_minRange;

			if (m_rangeEndPosition == Vector3.zero)
				m_rangeEndPosition = transform.position + transform.up * m_maxRange;

			Gizmos.DrawLine(m_rangeStartPosition, m_rangeEndPosition);

			Gizmos.color = defaultGizmosColor;
		}

		#endregion

		#region Open Field

		[Header("Open Field")]
		[SerializeField] private ContactFilter2D m_contactFilter;
		private Ray m_openFieldRay;

		public bool CheckHasOpenField()
		{
			List<RaycastHit2D> results = new List<RaycastHit2D>();
			int raycasts = Physics2D.Raycast(m_rangeStartPosition, transform.up, m_contactFilter, results, m_rangeDistance);
			return 1 == raycasts;
		}

		[ContextMenu("CheckHasOpenField")]
		public void TestHasOpenField()
		{
			Debug.Log(CheckHasOpenField());
		}

		#endregion

		#region Debug

		[Header("Debug")]
		[SerializeField] private Color m_gizmosColor;
		Color defaultGizmosColor;


		#endregion

	}
}