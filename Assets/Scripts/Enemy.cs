using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Parameter")]
	public float m_move_speed = 5;
	public float m_rotation_speed = 200;
	public float m_life_time = 5;
	public int m_score = 100;

	//------------------------------------------------------------------------------

	private void Start()
	{
		StartCoroutine(MainCoroutine());
	}

	private void DeleteObject()
	{
		GameObject.Destroy(gameObject);
	}

	//
	private IEnumerator MainCoroutine()
	{
		while (true)
		{
			//move
			transform.position += new Vector3(0, -1, 0) * m_move_speed * Time.deltaTime;

			//animation
			transform.rotation *= Quaternion.AngleAxis(m_rotation_speed * Time.deltaTime, new Vector3(1, 1, 0));


			//lifetime
			m_life_time -= Time.deltaTime;
			if (m_life_time <= 0)
			{
				DeleteObject();
				yield break;
			}

			yield return null;
		}
	}

	//------------------------------------------------------------------------------

	private void OnCollisionEnter(Collision collision)
	{
		PlayerBullet player_bullet = collision.transform.GetComponent<PlayerBullet>();
		if (player_bullet)
		{
			DestroyByPlayer(player_bullet);
		}
	}

	void DestroyByPlayer(PlayerBullet a_player_bullet)
	{
		//add score
		if (StageLoop.Instance)
		{
			StageLoop.Instance.AddScore(m_score);
		}

		//delete bullet
		if (a_player_bullet)
		{
			a_player_bullet.DeleteObject();
		}

		//delete self
		DeleteObject();
	}
}
