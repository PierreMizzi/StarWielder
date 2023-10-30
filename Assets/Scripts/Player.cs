using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Character
/// </summary>
public class Player : MonoBehaviour
{
	[Header("Prefab")]
	public PlayerBullet m_prefab_player_bullet;

	[Header("Parameter")]
	public float m_move_speed = 1;

	//------------------------------------------------------------------------------

	public void StartRunning()
	{
		StartCoroutine(MainCoroutine());
	}

	//
	private IEnumerator MainCoroutine()
	{
		while (true)
		{
			//moving
			{
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					transform.position += new Vector3(-1, 0, 0) * m_move_speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.RightArrow))
				{
					transform.position += new Vector3(1, 0, 0) * m_move_speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					transform.position += new Vector3(0, 1, 0) * m_move_speed * Time.deltaTime;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					transform.position += new Vector3(0, -1, 0) * m_move_speed * Time.deltaTime;
				}
			}

			//shoot
			{
				if (Input.GetKeyDown(KeyCode.Z))
				{
					PlayerBullet bullet = Instantiate(m_prefab_player_bullet, transform.parent);
					bullet.transform.position = transform.position;
				}
			}

			yield return null;
		}
	}
}
