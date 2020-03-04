using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BtnMedal : MonoBehaviour {

	public Button m_btn;
	public TextMeshProUGUI m_txtNum;
	public TextMeshProUGUI m_txtLimitTime;
	public Image m_imgIcon;
	public int medal_item_id;

	public PanelMedalCheck m_panelMedalCheck;

	public void ShowUpdate()
	{
		DataItemParam item_medal = DataManager.Instance.dataItem.list.Find(p => p.item_id == medal_item_id);

		int num = 0;
		if (item_medal != null)
		{
			num = item_medal.num;
		}
		m_txtNum.text = string.Format("x{0}", num);

	}
	public void SetMedalId(int _iItemId)
	{
		Debug.Log(_iItemId);
		medal_item_id = _iItemId;

		if (0 < _iItemId)
		{
			MasterItemParam master_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _iItemId);
			m_imgIcon.sprite = SpriteManager.Instance.Get(master_item.sprite_name);
			ShowUpdate();
		}
	}

}
