using System;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{

	[Serializable]
	public class SunMaterialConfig
	{
		[ColorUsage(true, true)] public Color innerColor;
		[ColorUsage(true, true)] public Color outerColor;
		public float scrollSpeed;
		
		[ColorUsage(true, true)] public Color trailColor;

		public const string color_innerColor = "_InnerColor";
		public const string color_outerColor = "_OuterColor";
		public const string float_speed = "_Speed";
		public const string color_color = "_Color"; // Nice variable name, right ?

	}
}