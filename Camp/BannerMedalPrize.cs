using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerMedalPrize : MonoBehaviour {

	public UnityEventInt OnMedalPrizeId = new UnityEventInt();
	private int m_iMedalPrizeId;

	public TextMeshProUGUI m_txtPrizeName;
	public Image m_imgPrizeItem;

	public TextMeshProUGUI m_txtExchange;
	public TextMeshProUGUI m_txtMedalNum;
	public TextMeshProUGUI m_txtItemNum;

	public Button btn;

	public void Initialize(MasterMedalPrizeParam _master)
	{
		MasterItemParam prize_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _master.prize_item_id);
		m_txtPrizeName.text = prize_item.name;
		m_imgPrizeItem.sprite = SpriteManager.Instance.Get(prize_item.sprite_name);



		MasterItemParam master_token_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _master.item_id_medal);
		DataItemParam data_token_item = DataManager.Instance.dataItem.list.Find(p => p.item_id == _master.item_id_medal);
		m_txtExchange.text = string.Format("{0}必要枚数[{1}]", master_token_item.name, prize_item.price);

		int token_num = 0;
		if (data_token_item != null)
		{
			token_num = data_token_item.num;
		}
		m_txtMedalNum.text = string.Format("所持{0}:{1}枚", master_token_item.name, token_num);


		DataItemParam data_item_param = DataManager.Instance.dataItem.list.Find(p => p.item_id == _master.prize_item_id);

		int item_num = 0;
		if(data_item_param != null)
		{
			item_num = data_item_param.num;
		}

		m_txtItemNum.text = string.Format("所持数{0}", item_num);

		m_iMedalPrizeId = _master.medal_prize_id;
		btn.onClick.AddListener(() =>
		{
			OnMedalPrizeId.Invoke(m_iMedalPrizeId);
		});
	}


}
