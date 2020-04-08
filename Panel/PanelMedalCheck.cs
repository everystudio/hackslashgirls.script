using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelMedalCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtMessage;
	public Image m_imgIcon;
	public Button m_btnUse;
	public Button m_btnCancel;

	public static int GetBookGemNum()
	{
		int num = 20;
		return num;
	}

	public int Show(int _item_id)
	{
		MasterItemParam master_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _item_id);

		int num = GetBookGemNum();
		m_txtMessage.text = string.Format("Watch the video and\nget <color=#0FF>{0}</color> {1}s", num, master_item.name);


		m_imgIcon.sprite = SpriteManager.Instance.Get(master_item.sprite_name);

		return num;
	}


}
