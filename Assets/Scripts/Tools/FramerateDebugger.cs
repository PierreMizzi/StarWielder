using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class FramerateDebugger : MonoBehaviour
{
	[SerializeField] private int m_frameRate = 60;

	[SerializeField] private int m_testFrameAmount = 60;
	private int m_testFrameCount = 0;
	private float m_testedFramerate;

	private void OnValidate()
	{
		Application.targetFrameRate = m_frameRate;
	}

	private void Update()
	{
		if (m_testFrameCount < m_testFrameAmount)
		{
			m_testFrameCount++;
			m_testedFramerate += Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.T))
			TestFramerate();
	}

	public void TestFramerate()
	{
		StartCoroutine(TestFramerateCoroutine());
	}

	private IEnumerator TestFramerateCoroutine()
	{
		m_testFrameCount = 0;
		m_testedFramerate = 0;

		while (m_testFrameCount < m_testFrameAmount)
		{
			m_testFrameCount++;
			m_testedFramerate += Time.deltaTime;
			yield return null;
		}

		m_testedFramerate /= m_testFrameAmount;
		m_testedFramerate = 1f / m_testedFramerate;
		Debug.Log(m_testedFramerate);
		yield return null;
	}
}