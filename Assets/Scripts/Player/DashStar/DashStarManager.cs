using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	public class DashStarManager : MonoBehaviour
	{

		[SerializeField] private Ship m_ship;
 
		#region MonoBehaviour

		private void Start()
		{
			InitializeDashStars();
		}

		#endregion

		#region DashStar

		[Header("Dash Stars")]
		[SerializeField] private DashStar m_dashStarPrefab;
		[SerializeField] public DashStarAnchor m_firstAnchor;
		[SerializeField] private List<DashStar> m_dashStars = new List<DashStar>();

		private void InitializeDashStars()
		{
			if (m_ship == null)
			{
				return;
			}

			foreach (DashStar dashStar in m_dashStars)
			{
				dashStar.Initialize(m_ship, this);
			}
			AssignDashStarAnchors();
		}

		public DashStar GetUsableDashStar()
		{
			DashStar dashStar;
			for (int i = m_dashStars.Count - 1; i >= 0; i--)
			{
				dashStar = m_dashStars[i];
				if (dashStar.canUse)
				{
					RearrangeDashStars(dashStar);
					return dashStar;
				}
			}
			return null;
		}

		public void RearrangeDashStars(DashStar usedDashStar)
		{

			// There is only one Dash Star, no need to rearrange
			if (m_dashStars.Count == 1)
			{
				return;
			}

			int selectedIndex = m_dashStars.IndexOf(usedDashStar);

			// We used the last DashStar, there is no need to rearrange
			if (selectedIndex == m_dashStars.Count - 1)
			{
				return;
			}

			int index = selectedIndex + 1;
			int count = m_dashStars.Count - 1 - selectedIndex;
			List<DashStar> dashStarsToMove = m_dashStars.GetRange(index, count);

			for (int i = 0; i <= dashStarsToMove.Count - 1; i++)
			{
				m_dashStars[selectedIndex + i] = dashStarsToMove[i];
			}
			m_dashStars[m_dashStars.Count - 1] = usedDashStar;

			AssignDashStarAnchors();

		}

		public void AssignDashStarAnchors()
		{
			// We rearrange the anchor
			for (int i = 0; i < m_dashStars.Count; i++)
			{
				DashStar dashStar = m_dashStars[i];
				if (i == 0)
				{
					dashStar.SetAnchor(m_firstAnchor);
				}
				else
				{
					dashStar.SetAnchor(m_dashStars[i - 1].nextAnchor);
				}
			}
		}


		#endregion

		#region Module - Twin Dash Star

		public void CreateDashStar()
		{
			DashStar lastDashStar = m_dashStars[m_dashStars.Count - 1];
			DashStar newDashStar = Instantiate(m_dashStarPrefab);
			newDashStar.Initialize(m_ship, this);
			newDashStar.transform.position = lastDashStar.nextAnchor.transform.position;
			newDashStar.SetAnchor(lastDashStar.nextAnchor);
			m_dashStars.Add(newDashStar);
		}

		public void DeleteDashStar()
		{
			if (m_dashStars.Count == 1)
			{
				return;
			}

			DashStar dashStar = m_dashStars[m_dashStars.Count-1];
			m_dashStars.Remove(dashStar);
			Destroy(dashStar.gameObject);
		}

		#endregion
	}
}