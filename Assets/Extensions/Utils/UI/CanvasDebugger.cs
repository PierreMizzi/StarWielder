using UnityEngine;

namespace PierreMizzi.Useful
{
	public class CanvasDebugger : MonoBehaviour
	{
		[SerializeField]
		private Canvas m_canvas;

		private void Start()
		{
			DebugDisplay();

			// Display all child
			int count = transform.childCount;
			GameObject child;
			for (int i = 0; i < count; i++)
			{
				child = transform.GetChild(i).gameObject;
				child.SetActive(true);
			}
		}

		public void DebugDisplay()
		{
			m_canvas.enabled = true;
		}

		public void DebugHide()
		{
			m_canvas.enabled = false;
		}

		public void IsolateChild(int childIndex)
		{
			DebugDisplay();

			int count = transform.childCount;
			GameObject child;
			for (int i = 0; i < count; i++)
			{
				child = transform.GetChild(i).gameObject;
				child.SetActive(i == childIndex);
			}
		}

	}
}