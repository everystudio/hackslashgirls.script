using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessaryBase : MonoBehaviour {

	public MasterAccessaryParam accessary;

	public float delta_time;

	// Use this for initialization
	void Start () {

		delta_time = 0.0f;		
	}

	public bool UseItem()
	{
		bool bRet = false;
		DataItemParam potion = DataManager.Instance.dataItem.list.Find(p => p.item_id == accessary.use_item_id && 0 < p.num);
		if (potion != null)
		{
			if (potion.Use())
			{
				potion.num -= 1;
				GameMain.Instance.ShortcutRefresh(potion.serial);
				bRet = true;
			}
		}
		return bRet;
	}

	// Update is called once per frame
	void Update () {

		if(accessary == null)
		{
			return;
		}

		bool bUpdateTime = false;
		if( accessary.situation == 0)
		{
			bUpdateTime = true;
		}
		else if( accessary.situation == 1 && GameMain.Instance.charaControl.IsBattle == true)
		{
			bUpdateTime = true;
		}
		if (bUpdateTime)
		{
			delta_time += Time.deltaTime;
		}


		if( accessary.interval < delta_time)
		{
			if(0 < accessary.hp_rate && 0 < accessary.stamina_rate)
			{
				int hp_rate = DataManager.Instance.dataChara.GetHpRate();
				int stamina_rate = DataManager.Instance.dataChara.GetStaminaRate();
				if (hp_rate < accessary.hp_rate && stamina_rate < accessary.stamina_rate)
				{
					if (UseItem())
					{
						MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == accessary.use_item_id);
						GameMain.Instance.BattleLog(string.Format(accessary.log_format, master.name));
					}
					else
					{
						GameMain.Instance.BattleLog("Failed to use item");
					}
				}
			}
			else if( 0 < accessary.hp_rate)
			{
				delta_time = 0.0f;
				int hp_rate = DataManager.Instance.dataChara.GetHpRate();
				if(hp_rate < accessary.hp_rate)
				{
					if(UseItem())
					{
						MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == accessary.use_item_id);
						GameMain.Instance.BattleLog(string.Format(accessary.log_format, master.name));
					}
					else
					{
						GameMain.Instance.BattleLog("Failed to use item");
					}
				}
			}
			else if( 0 < accessary.stamina_rate)
			{
				delta_time = 0.0f;
				int stamina_rate = DataManager.Instance.dataChara.GetStaminaRate();
				if (stamina_rate < accessary.stamina_rate)
				{
					if (UseItem())
					{
						MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == accessary.use_item_id);
						GameMain.Instance.BattleLog(string.Format(accessary.log_format, master.name));
					}
					else
					{
						GameMain.Instance.BattleLog("Failed to use item");
					}
				}
			}

		}
	}
}
