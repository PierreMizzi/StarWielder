using System.Collections;
using PierreMizzi.Useful;
using PierreMizzi.Useful.UI;
using StarWielder.UI;
using TMPro;
using UnityEngine;



[ExecuteInEditMode]
public class DialogueUI : MonoBehaviour, IDisplayHideAnimator
{
	#region Behaviour

	[SerializeField] private UIChannel m_UIChannel;
	[SerializeField] private TextMeshProUGUI m_text;


	private void CallbackDisplay(DialogueData data, Vector3 worldPosition)
	{
		m_text.text = data.text;

		StartCoroutine(DisplayBehaviour(worldPosition));
	}

	private void CallbackHide()
	{
		(this as IDisplayHideAnimator).Hide();
	}

	#endregion
	
	#region UI

	[SerializeField] private RectTransform m_container;

	private Vector2 m_containerSizeDelta;

	private IEnumerator DisplayBehaviour(Vector3 worldPosition)
	{
		yield return null;
		yield return null;
		Vector3 containerOffset = new Vector3
		{
			x = -m_text.rectTransform.sizeDelta.x / 2f,
			y = m_text.rectTransform.sizeDelta.y / 2f,
			z = 0,
		};
		m_container.localPosition = containerOffset;
		m_container.sizeDelta = m_containerSizeDelta = new Vector2(50, 50);

		// Canvas Position : Might need to manage Canvas Scaler bs I don't know
		Vector3 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
		(transform as RectTransform).anchoredPosition = UtilsClass.ViewportToCanvasCoords(screenPosition);
		(transform as RectTransform).anchoredPosition += new Vector2(containerOffset.x, containerOffset.y);

		// needOnAnimatorMove = true inside "Display" animation. Eventually turn false.
		(this as IDisplayHideAnimator).Display();
	}

	public void UpdateContainerRectTransform()
	{
		if (m_container == null)
		{
			return;
		}

		m_containerSizeDelta.x = Mathf.Lerp(50, m_text.rectTransform.sizeDelta.x, m_progressWidth);
		m_containerSizeDelta.y = Mathf.Lerp(50, m_text.rectTransform.sizeDelta.y, m_progressHeight);
		m_container.sizeDelta = m_containerSizeDelta;
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	protected void Start()
	{
		if (m_UIChannel != null)
		{
			m_UIChannel.onDisplayDialogue += CallbackDisplay;
			m_UIChannel.onHideDialogue += CallbackHide;
		}
	}

	protected void Update()
	{
		if (needOnAnimatorMove)
		{
			UpdateContainerRectTransform();
		}
	}

	void OnDestroy()
	{
		if (m_UIChannel != null)
		{
			m_UIChannel.onDisplayDialogue -= CallbackDisplay;
			m_UIChannel.onHideDialogue -= CallbackHide;
		}
	}

	#endregion

	#region IDisplayHideAnimation

	public Animator animator { get; set; }

	[HideInInspector] public float m_progressWidth = 0f;
	[HideInInspector] public float m_progressHeight = 0f;
	public bool needOnAnimatorMove = false;
	

	#endregion

	#region Debug

	[ContextMenu("Call Test")]
	public void TestDisplay()
	{
		CallbackDisplay(new DialogueData(){text = "Jai, j'espere que tu seras contente de me voir ce soir !"}, Vector3.zero);
	}

	[ContextMenu("Call TestHide")]
	public void TestHide()
	{
		CallbackHide();
	}
		
	#endregion
}