using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerCraftBase : MonoBehaviour {

	private Button m_btn;
	public PanelItems.UnityEventDataItemParam HandleBannerEvent = new PanelItems.UnityEventDataItemParam();

	public TextMeshProUGUI m_textName;
	public TextMeshProUGUI m_txtDetail;
	public Image m_imgIcon;

	private DataItemParam m_dataItem;

	public void Initialize( DataItemParam _param , MasterItemParam _master)
	{
		m_dataItem = _param;

		string strName = _master.name;
		if( 0 < _param.craft_count)
		{
			strName = string.Format("{0}+{1}", _master.name, _param.craft_count);
		}
		m_textName.text = strName;
		m_txtDetail.text = _master.detail;

		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_holder, _master.sprite_name);
		m_imgIcon.color = _master.GetSpriteColor();

		if (m_btn == null)
		{
			m_btn = gameObject.GetComponent<Button>();
			m_btn.onClick.AddListener(() =>
			{
				HandleBannerEvent.Invoke(m_dataItem);
			});
		}
	}

}
