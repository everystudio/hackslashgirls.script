using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAging : MonoBehaviour {

	[SerializeField]
	private float m_fInterval;
	private float m_fTime;

	


	void Update () {

		m_fTime -= Time.deltaTime;
		if (m_fTime < 0.0f)
		{
			m_fTime += m_fInterval;

			if (DataManager.Instance.dataChara.hp < DataManager.Instance.dataChara.hp_max / 2)
			{
				DataItemParam potion = DataManager.Instance.dataItem.list.Find(p => p.item_id == 10101 && 0 < p.num);
				if (potion != null)
				{
					if (potion.Use())
					{
						potion.num -= 1;

						GameMain.Instance.ShortcutRefresh(potion.serial);

					}
				}
			}
			if (DataManager.Instance.dataChara.hunger < DataManager.Instance.dataChara.hunger_max / 3)
			{
				DataItemParam bread = DataManager.Instance.dataItem.list.Find(p => p.item_id == 10201 && 0 < p.num);
				if (bread != null)
				{
					if (bread.Use())
					{
						bread.num -= 1;
						GameMain.Instance.ShortcutRefresh(bread.serial);
					}
				}
			}
		}

	}
}
