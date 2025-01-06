using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay
{
	[CreateAssetMenu(fileName = "FightStageSettings", menuName = "StarWielder/FightStageSettings", order = 0)]
	public class FightStageSettings : ScriptableObject
	{
		[Multiline(5)]
		[SerializeField] private string note;

		[SerializeField] private List<FightStageData> m_datas = new List<FightStageData>();

		public List<FightStageData> datas { get { return m_datas; } }

	}
}