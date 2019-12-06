using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerSkin : MonoBehaviour {

	public UnityEventInt OnClickSkin = new UnityEventInt();
	private int skin_id;
	public Image m_imgIcon;
	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtDetail;
	public Button m_btn;

	public void Initialize(MasterSkinParam _master)
	{
		skin_id = _master.skin_id;

		MasterItemParam item = DataManager.Instance.masterItem.list.Find(p => p.item_id == _master.item_id);

		m_imgIcon.sprite = SpriteManager.Instance.Get(item.sprite_holder, item.sprite_name);
		m_txtName.text = _master.skin_name;

		bool bShow = false;

		if( skin_id == 1)
		{
			bShow = true;
		}
		else
		{
			DataItemParam item_skin = DataManager.Instance.dataItem.list.Find(p => p.item_id == _master.item_id);
			if(item_skin != null)
			{
				bShow = true;
			}
		}

		if (bShow == false)
		{
			m_txtDetail.text = string.Format("<color=red>未所持</color>:{0}", _master.skin_outline);
		}
		else
		{
			m_txtDetail.text = string.Format("{0}", _master.skin_outline);
		}

		m_btn.interactable = bShow;



		m_btn.onClick.AddListener(() =>
		{
			OnClickSkin.Invoke(skin_id);
		});

	}


}
