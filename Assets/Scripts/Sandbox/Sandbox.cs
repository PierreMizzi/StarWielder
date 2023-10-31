using PierreMizzi.Useful;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
	[SerializeField] private LayerMask m_layerMask;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (UtilsClass.CheckLayer(m_layerMask.value, other.gameObject.layer))
		{
			Debug.Log(other.name);
		}
	}
}