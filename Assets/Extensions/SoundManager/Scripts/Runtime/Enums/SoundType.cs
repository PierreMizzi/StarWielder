using System;

namespace PierreMizzi.SoundManager
{

	/// <summary>
	/// Category of sound to play in an application
	/// </summary>
	[Flags]
	public enum SoundType
	{
		None = 0,

		/// <summary>
		/// Concerns all ambiant musics
		/// </summary>
		Music = 1 << 1,

		/// <summary>
		/// SFX used inside the game/gameplay
		/// </summary>
		GameSFX = 1 << 2,

		/// <summary>
		/// SFX used inside the menus (buttons, feedback)
		/// </summary>
		MenuSFX = 1 << 3,

		/// <summary>
		/// VoiceLines by characters
		/// </summary>
		VoiceLine = 1 << 4,
	}

}