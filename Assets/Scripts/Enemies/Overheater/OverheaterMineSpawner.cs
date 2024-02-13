using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;


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
		[SerializeField] private OverheaterMine m_minePrefab;
		[SerializeField] private PoolingChannel m_poolingChannel;

		public bool hasMine { get; set; }

		private float m_rangeDistance;
		private Vector3 m_rangeStartPosition;
		private Vector3 m_rangeEndPosition;

		public void ComputeRangePositions()
		{
			m_rangeStartPosition = transform.position + transform.up * m_minRange;
			m_rangeEndPosition = transform.position + transform.up * m_maxRange;
		}

		[ContextMenu("Spawn Mine")]
		public void SpawnMine()
		{
			OverheaterMine mine = m_poolingChannel.onGetFromPool.Invoke(m_minePrefab.gameObject).GetComponent<OverheaterMine>();
			mine.transform.position = transform.position;
			mine.transform.rotation = Quaternion.identity;
			Vector3 rndPosition = transform.position + transform.up * Random.Range(m_minRange, m_maxRange);
			mine.Initialize(this, rndPosition);
			hasMine = true;
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_rangeDistance = m_maxRange - m_minRange;
			ComputeRangePositions();
		}

		private void OnValidate()
		{
			ComputeRangePositions();
		}

		protected void OnDrawGizmos()
		{
			defaultGizmosColor = Gizmos.color;

			Gizmos.color = m_gizmosColor;
			Gizmos.DrawLine(m_rangeStartPosition, m_rangeEndPosition);

			Gizmos.color = defaultGizmosColor;
		}

		#endregion

		#region Open Field

		[Header("Open Field")]
		[SerializeField] private ContactFilter2D m_contactFilter;

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