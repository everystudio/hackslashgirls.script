using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBuyCheck : MonoBehaviour {

	public Button m_btnCancel;
	public Button m_btnClose;

	public Button m_btnPlus;
	public Button m_btnMinus;

	public Button m_btnBuy;
	public Button m_btnBulk;

	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtNum;
	public TextMeshProUGUI m_txtPrice;
	public TextMeshProUGUI m_txtDetail;

	public TextMeshProUGUI m_txtBuyButtonLabel;

	public MasterItemParam master_param;
	public TextMeshProUGUI m_txtBulkBuy;

	public void ShowInitialize(int _iItemId)
	{
		master_param = DataManager.Instance.masterItem.list.Find(p => p.item_id == _iItemId);
		m_txtName.text = master_param.name;
		m_txtDetail.text = master_param.detail;
		m_txtPrice.text = string.Format("{0}Coin", master_param.price);

	}

	public void ShowUpdate(int _iItemId , int _iBuyNum)
	{
		// カテゴリーとかみてないけど、必要があればやってね
		List<DataItemParam> item_list = DataManager.Instance.dataItem.list.FindAll(p => p.item_id == _iItemId);
		int iNum = 0;
		foreach (DataItemParam data in item_list)
		{
			iNum += data.num;
		}
		m_txtNum.text = string.Format("{0}", iNum);

		bool bAbleBulk = MasterItem.AbleBulkBuy(_iItemId);

		m_btnBuy.interactable = true;

		if (bAbleBulk)
		{
			m_btnPlus.interactable = true;
			m_btnMinus.interactable = true;
			m_txtBulkBuy.text = "You can change the number\nof purchases in bulk";
		}
		else
		{
			m_btnPlus.interactable = false;
			m_btnMinus.interactable = false;
			m_txtBulkBuy.text = "This item cannot be\npurchased in bulk";

			int equip_item_num = DataManager.Instance.dataItem.list.FindAll(p => MasterItem.CategoryMagic < p.item_id / MasterItem.LargeCategory).Count;
			//Debug.Log(equip_item_num);
			if(Defines.EQUIP_ITEM_LIMIT <= equip_item_num )
			{
				m_btnBuy.interactable = false;
				m_txtDetail.text = string.Format("装備系のアイテムは{0}個以上所持できません\nアイテムページからGemに変換して減らしてください", Defines.EQUIP_ITEM_LIMIT);
			}
		}

		if (_iBuyNum == 1)
		{
			m_txtPrice.text = string.Format("{0}Coin", master_param.price);
		}
		else
		{
			m_txtPrice.text = string.Format("{0}Coin", master_param.price * _iBuyNum);
		}

		m_txtBuyButtonLabel.text = string.Format("{0} times\npurchase", _iBuyNum);

		Color color_one = master_param.price * _iBuyNum <= DataManager.Instance.user_data.ReadInt(Defines.KeyCoin) ? Color.white : Color.gray;
		m_btnBuy.gameObject.GetComponent<Image>().color = color_one;

		Color color_ten = master_param.price * 10 <= DataManager.Instance.user_data.ReadInt(Defines.KeyCoin) ? Color.white : Color.gray;
		m_btnBulk.gameObject.GetComponent<Image>().color = color_ten;
	}



}
