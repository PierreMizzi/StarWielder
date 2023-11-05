using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

namespace PierreMizzi.SoundManager
{

	public static class SoundDataIDGenerator
	{

		private const string k_soundDataIDFolder = "/Scripts/SoundManager";
		private const string k_soundDataIDFile = "/SoundDataID.cs";

		private const string TEMPLATE_SOUND_DESCRIPTION =
			@"
			/*
				{0}
			*/
		";

		private const string TEMPLATE_SOUND_DATA_ID =
			@"
			public static class SoundDataID
			{0}
		";


		public static void WriteFile(List<SoundDataLibrary> libraries)
		{
			string path = Application.dataPath + k_soundDataIDFolder + k_soundDataIDFile;
			bool exists = File.Exists(path);
			string fileContent = GenerateFileContent(libraries);

			// Checks file exists
			if (!exists)
				Directory.CreateDirectory(Application.dataPath + k_soundDataIDFolder);

			File.WriteAllText(path, fileContent);
		}

		public static string GenerateFileContent(List<SoundDataLibrary> libraries)
		{
			string data = " { \r";

			foreach (SoundDataLibrary library in libraries)
			{
				if (!library.GenerateStatic)
					continue;

				data += string.Format(TEMPLATE_SOUND_DESCRIPTION, library.name);

				foreach (SoundData soundData in library.SoundDatas)
					data += string.Format("public static readonly string {0} = \"{1}\"; \r", UpperCamelCaseToConstant(soundData.ID), soundData.ID);

			}

			data += " } \r ";

			return string.Format(TEMPLATE_SOUND_DATA_ID, data);
		}

		public static string UpperCamelCaseToConstant(string data)
		{
			string result = "";
			MatchCollection collection = Regex.Matches(data, @"(?:[A-Z][a-z]*|\d+)");
			for (int i = 0; i < collection.Count - 1; i++)
				result += (collection[i].Value + "_").ToUpper();

			result += collection[^1].Value.ToUpper();
			return result;
		}


	}

}

