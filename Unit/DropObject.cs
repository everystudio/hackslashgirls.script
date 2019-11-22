using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer m_sprItems;

	private bool m_bGet = false;
	private MasterItemParam m_masterItemParam;
	public void Initialize(MasterItemParam _master )
	{
		if ( _master.item_id < 10 )
		{
			m_sprItems.sprite = SpriteManager.Instance.Get(_master.sprite_holder, _master.sprite_name);
		}
		else
		{
			m_sprItems.sprite = SpriteManager.Instance.Get("items" , "drop_chest");
		}
		m_masterItemParam = _master;

		Vector2 dir = new Vector2(-0.1f, 1.0f);

		gameObject.GetComponent<Rigidbody2D>().AddForce(200f * dir);
	}


	void OnTriggerEnter2D(Collider2D _collision)
	{
		if (m_bGet == false && _collision.gameObject.tag == "Chara")
		{
			m_bGet = true;

			if( m_masterItemParam.item_id == Defines.ITEM_ID_DROP_COIN)
			{
				DataManager.Instance.user_data.AddInt(Defines.KeyCoin, 5);
				DataManager.Instance.user_data.Save();
				SEControl.Instance.Play("Coins 03");
			}
			else if (m_masterItemParam.item_id == Defines.ITEM_ID_DROP_GEM)
			{
				DataManager.Instance.user_data.AddInt(Defines.KeyGem, 1);
				DataManager.Instance.user_data.Save();
				SEControl.Instance.Play("eat");
			}
			else if (m_masterItemParam.item_id == Defines.ITEM_ID_DROP_GEM_BOOK)
			{
				int iGetItemId = Defines.ITEM_ID_GEM_BOOK;
				MasterItemParam gem_book = DataManager.Instance.masterItem.list.Find(p => p.item_id == iGetItemId);

				DataManager.Instance.dataItem.AddItem(iGetItemId, 1);
				DataItemParam data_param = DataManager.Instance.dataItem.list.Find(p => p.item_id == iGetItemId);
				GameMain.Instance.ShortcutRefresh(data_param.serial);
				DataManager.Instance.dataItem.Save();
				GameMain.Instance.BattleLog(string.Format("<color=#00ffff>{0}</color>を手に入れた", gem_book.name));
				SEControl.Instance.Play("eat");
			}
			else
			{
				DataManager.Instance.dataItem.AddItem(m_masterItemParam.item_id, 1);
				DataItemParam data_param = DataManager.Instance.dataItem.list.Find(p => p.item_id == m_masterItemParam.item_id);
				GameMain.Instance.ShortcutRefresh(data_param.serial);
				DataManager.Instance.dataItem.Save();
				GameMain.Instance.BattleLog(string.Format("<color=#00ffff>{0}</color>を手に入れた", m_masterItemParam.name));
				SEControl.Instance.Play("eat");
			}

			Destroy(gameObject.GetComponent<Rigidbody2D>());
			//gameObject.GetComponent<BoxCollider2D>().isTrigger = true; ;

			gameObject.GetComponent<Animator>().SetTrigger("get");

			Destroy(gameObject, 1.0f);
		}
	}

}
