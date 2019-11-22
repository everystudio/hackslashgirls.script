using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackArea : MonoBehaviour {

	private DataEnemyParam m_dataEnemy;

	public void Initialize( DataEnemyParam _data)
	{
		m_dataEnemy = _data;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Chara")
		{
			// エントリーなので、履いた処理とかいらんでしょ
			// 親です
			CharaControl chara = collider.gameObject.transform.parent.gameObject.GetComponent<CharaControl>();
			/*
			Debug.Log(chara);
			Debug.Log(m_dataEnemy);
			Debug.Log(m_dataEnemy.attack);
			*/
			if (0 < DataManager.Instance.dataChara.hp)
			{
				chara.Damage(m_dataEnemy.attack, m_dataEnemy.attribute);
			}
		}
	}


}
