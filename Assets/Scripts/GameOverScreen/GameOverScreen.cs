using System;
using System.Collections;
using System.Text;
using DG.Tweening;
using PierreMizzi.Useful;
using TMPro;
using UnityEngine;

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

	private void CallbackDisplay(GameOverData data)
	{
		m_reasonMessage = MessageFromReason(data.reason);
		m_time = data.time;
		m_starEnergy = data.starEnergy;

		m_animator.SetTrigger(k_triggerDisplay);
	}

	[ContextMenu("Next")]
	private void Next()
	{
		m_animator.SetTrigger(k_triggerNext);
	}

	#endregion

	#region Main

	[SerializeField] private GameChannel m_gameChannel = null;

	private void Initialize()
	{
		m_reasonMessageLabel.text = "";

		m_timeLabel.text = "";
		m_starEnergyLabel.text = "";
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		m_animator = GetComponent<Animator>();

		Initialize();

		if (m_gameChannel != null)
			m_gameChannel.onGameOverScreen += CallbackDisplay;
	}



	private void Update()
	{
		ManageShortcuts();
	}
	private void OnDestroy()
	{

		if (m_gameChannel != null)
			m_gameChannel.onGameOverScreen -= CallbackDisplay;
	}

	#endregion

	#region Reason Message

	[Header("Reason Message")]
	[SerializeField] private TextMeshProUGUI m_reasonMessageLabel;
	[SerializeField] private string m_randomErrorCharacters = "01";
	[SerializeField] private float m_delayBetweenCharacters = 0.1f;

	private string m_reasonMessage;

	private const string k_reasonShipDestroyed = "Your ship got destroyed ...";
	private const string k_reasonStarDied = "Your star ran out of energy ...";

	private void AnimEventReasonMessage()
	{
		StartCoroutine(ReasonMessageCoroutine());
	}

	private IEnumerator ReasonMessageCoroutine()
	{
		int reasonLength = m_reasonMessage.Length;
		StringBuilder builtString = new StringBuilder();
		string iChar;
		string jChar;

		for (int i = 0, j = -1; j < reasonLength; i++, j++)
		{
			// Encrypted
			if (i < reasonLength)
			{
				iChar = m_reasonMessage.Substring(i, 1);
				builtString.Append(GetRandomCharacter());
			}

			// Decrypted
			if (j >= 0)
			{
				jChar = m_reasonMessage.Substring(j, 1);
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

	private string MessageFromReason(GameOverReason reason)
	{
		switch (reason)
		{
			case GameOverReason.ShipDestroyed:
				return k_reasonShipDestroyed;
			case GameOverReason.StarDied:
				return k_reasonStarDied;
		}
		return "";
	}


	#endregion

	#region Score

	[Header("Score")]
	[SerializeField] private TextMeshProUGUI m_timeLabel;
	[SerializeField] private TextMeshProUGUI m_starEnergyLabel;

	[SerializeField] private float m_scoreTextDuration = 1f;

	public float m_time = 90f;
	public float m_starEnergy = 156.9f;

	public void AnimEventTime()
	{
		DOVirtual
		.Float(
			0f,
			m_time,
			m_scoreTextDuration,
			(float value) => m_timeLabel.text = UtilsClass.SecondsToTextTime(value)
		)
		.SetEase(Ease.InQuad)
		.OnComplete(Next);
	}

	// TODO : Rename HighestEnergy in StarEnergy
	public void AnimEventHighestEnergy()
	{
		DOVirtual
		.Float(
			0f,
			m_starEnergy,
			m_scoreTextDuration,
			(float value) => m_starEnergyLabel.text = String.Format("{0:0.0}", value)
		)
		.SetEase(Ease.InQuad)
		.OnComplete(Next);
	}


	#endregion

	#region Replay

	public void OnClickReplay()
	{
		m_gameChannel.onReplay.Invoke();
	}

	#endregion

	#region Debug

	private void ManageShortcuts()
	{
	}

	private void DebugCallbackDisplay()
	{
		m_animator.SetTrigger(k_triggerDisplay);
	}

	#endregion

}