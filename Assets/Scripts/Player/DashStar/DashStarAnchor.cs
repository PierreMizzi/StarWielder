using UnityEngine;

public class DashStarAnchor : MonoBehaviour
{
	[SerializeField] private Transform m_directionTransform;
	[SerializeField] private Transform m_positionTransform;
	[SerializeField] private float m_radius;

    public void Set(Transform directionTransform, Transform positionTransform)
    {
        m_directionTransform = directionTransform; 
        m_positionTransform = positionTransform;
    }

    public void LateUpdate()
    {
        if (m_positionTransform == null || m_directionTransform == null)
        {
            return;
        }
        transform.position = m_positionTransform.position - m_directionTransform.right * m_radius;
    }

}