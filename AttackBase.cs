using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBase : MonoBehaviour {

	public string effect_name {
		get
		{
			return gameObject.name;
		}
	}

	private bool m_bInitialized = false;
	public void Initialize()
	{
		if( m_bInitialized)
		{
			return;
		}
	}

	private int m_iAttack;
	private string m_strAttribute;

	private BoxCollider2D box_collider_2d;

	private int m_iCount;

	public List<EnemyBase> attacked_enemy_list = new List<EnemyBase>();

	public void Attack( int _iAttack , string _strAttribute )
	{
		m_iAttack = _iAttack;
		m_strAttribute = _strAttribute;
		attacked_enemy_list.Clear();
		gameObject.SetActive(true);

		box_collider_2d = gameObject.GetComponent<BoxCollider2D>();

		Destroy(box_collider_2d, 0.35f);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Enemy")
		{
			//collider.gameObject.name = "hit_enemy";
			//Debug.Log(collider.gameObject.name);

			EnemyBase enemy = collider.gameObject.GetComponent<EnemyBase>();

			if( attacked_enemy_list.Contains(enemy) == false)
			{
				enemy.Damage(m_iAttack, m_strAttribute, m_iCount);
				m_iCount += 1;
				attacked_enemy_list.Add(enemy);
			}
		}
	}

}
