using System;
using UnityEngine;

namespace StarWielder.UI
{
	public delegate void DialogueDelegate(DialogueData data, Vector3 worldPosition);

	[CreateAssetMenu(fileName = "UIChannel", menuName = "StarWielder/Channels/UIChannel", order = 0)]
	public class UIChannel : ScriptableObject
	{
		// Dialogue Sun Socket
		public DialogueDelegate onDisplayDialogue = (DialogueData data, Vector3 worldPosition) => { };
		public Action onHideDialogue = () => { };
	}
}