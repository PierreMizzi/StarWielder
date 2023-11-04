using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/*
	- Les croix apparaissent et s'approchent
	- Fill du background derrière
	- GAME  (Shake Screen)
	- OVER ! (Shake Screen) ------------------> Step 1
	- Explication décrypté petit à petit
	- apparition du loading
	- loading 
	- Disparition loading
	- Agrandissement du background
	- Apparition Survival Time :
	- Défilement du temps
	- Apparition Highest Energy :
	- Défilement de l'énergie
	- Replay

	TO TEST : 
	- Succession d'étape/animations ?!
	- 


*/

public class GameOverScreen : MonoBehaviour
{

	#region Animator

	private Animator m_animator = null;

	private const string k_triggerDisplay = "Display";
	private const string k_triggerNext = "Next";
	private const string k_triggerHide = "Hide";

	[ContextMenu("Display")]
	private void Display()
	{
		m_animator.SetTrigger(k_triggerDisplay);
	}

	[ContextMenu("Next")]
	private void Next()
	{
		m_animator.SetTrigger(k_triggerNext);
	}

	#endregion

	#region Main

	private void Initialize()
	{
		m_reasonMessageLabel.text = "";

		m_timeLabel.text = "";
		m_highestEnergyLabel.text = "";
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		m_animator = GetComponent<Animator>();

		Initialize();
	}

	private void Update()
	{
		ManageShortcuts();
	}

	#endregion

	#region Reason Message

	[Header("Reason Message")]
	[SerializeField] private TextMeshProUGUI m_reasonMessageLabel;
	[SerializeField] private string m_randomErrorCharacters = "01";
	[SerializeField] private float m_delayBetweenCharacters = 0.1f;

	private const string k_reasonShipDestroyed = "Your ship got destroyed ...";
	private const string k_reasonStarImploded = "Your star imploded ...";

	[ContextMenu("Reason Message")]
	private void AnimEventReasonMessage()
	{
		StartCoroutine(ReasonMessageCoroutine());
	}

	private IEnumerator ReasonMessageCoroutine()
	{
		string reason = k_reasonShipDestroyed;
		int reasonLength = reason.Length;
		StringBuilder builtString = new StringBuilder();
		string iChar;
		string jChar;

		for (int i = 0, j = -1; j < reasonLength; i++, j++)
		{
			// Encrypted
			if (i < reasonLength)
			{
				iChar = reason.Substring(i, 1);
				builtString.Append(GetRandomCharacter());
			}

			// Decrypted
			if (j >= 0)
			{
				jChar = reason.Substring(j, 1);
				builtString[j] = char.Parse(jChar);
			}

			m_reasonMessageLabel.text = builtString.ToString();

			yield return new WaitForSeconds(m_delayBetweenCharacters);
		}

		Next();
		yield return null;
	}

	private char GetRandomCharacter()
	{
		int randomCharIndex = UnityEngine.Random.Range(0, m_randomErrorCharacters.Length);
		return m_randomErrorCharacters[randomCharIndex];
	}


	#endregion

	#region Score

	[Header("Score")]
	[SerializeField] private TextMeshProUGUI m_timeLabel;
	[SerializeField] private TextMeshProUGUI m_highestEnergyLabel;

	public void AnimEventTime()
	{

	}

	public void AnimEventHighestEnergy()
	{

	}

	#endregion

	#region Debug

	private void ManageShortcuts()
	{
		if (Input.GetKeyDown(KeyCode.D))
			Display();

		else if (Input.GetKeyDown(KeyCode.N))
			Next();
	}

	#endregion

}