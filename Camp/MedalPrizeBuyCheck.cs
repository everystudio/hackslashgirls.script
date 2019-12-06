using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MedalPrizeBuyCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtPrizeName;

	public Image m_imgPrizeItem;
	public Image m_imgPrizeToken;

	public TextMeshProUGUI m_txtCost;

	public Button m_btnYes;
	public Button m_btnCancel;

	public void Initialize(MasterMedalPrizeParam _master)
	{
		MasterItemParam prize_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _master.prize_item_id);
		m_txtPrizeName.text = prize_item.name;
		m_imgPrizeItem.sprite = SpriteManager.Instance.Get(prize_item.sprite_name);
		MasterItemParam master_token_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _master.medal_prize_id);
		m_imgPrizeToken.sprite = SpriteManager.Instance.Get(master_token_item.sprite_name);
		DataItemParam data_token_item = DataManager.Instance.dataItem.list.Find(p => p.item_id == _master.medal_prize_id);

		int token_num = 0;
		if( data_token_item != null)
		{
			token_num = data_token_item.num;
		}

		if (_master.medal_num <= token_num)
		{
			m_txtCost.text = string.Format("{0} → {1}", token_num, token_num - _master.medal_num);

			m_btnYes.interactable = true;
		}
		else
		{
			m_txtCost.text = "<color=red>メダルが足りません</color>";
			m_btnYes.interactable = false;
		}


	}
	
}
