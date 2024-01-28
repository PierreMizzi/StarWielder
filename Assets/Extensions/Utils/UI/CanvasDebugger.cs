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
		}

		public void DebugDisplay()
		{
			m_canvas.enabled = true;
		}

		public void DebugHide()
		{
			m_canvas.enabled = false;
		}
	}
}