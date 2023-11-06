using System;

using UnityEngine;

namespace PierreMizzi.Extensions.CursorManagement
{

	[CreateAssetMenu(fileName = "CursorChannel", menuName = "Channels/CursorChannel", order = 0)]
	public class CursorChannel : ScriptableObject
	{

		public CursorDelegate onSetCursor;

		private void OnEnable()
		{
			onSetCursor = (CursorType type) => { };
		}
	}
}