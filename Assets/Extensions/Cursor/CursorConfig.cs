using UnityEngine;
using System;

namespace PierreMizzi.Extensions.CursorManagement
{

	/// <summary>
	/// Cursor properties
	/// </summary>
	[Serializable]
	public struct CursorConfig
	{
		public string name;
		public CursorType type;
		public Texture2D texture;
		public Vector2 hotspot;
		public CursorMode mode;
	}
}