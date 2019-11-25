using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerEquip : MonoBehaviour
{
	public Button btn
	{
		get { return m_btn; }
	}
	[SerializeField]
	private Button m_btn;
	[SerializeField]
	private Image m_imgIcon;

	public PanelEquip.UnityEventDataItemParam OnBannerEvent = new PanelEquip.UnityEventDataItemParam();


	[SerializeField]
	private TextMeshProUGUI m_txtName;
	[SerializeField]
	private TextMeshProUGUI m_txtDetail;

	private DataItemParam m_data;

	public void Initialize(DataItemParam _data )
	{
		MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == _data.item_id);
		m_txtName.text = master.name;
		m_txtDetail.text = master.detail;

		m_imgIcon.sprite = SpriteManager.Instance.Get(master.sprite_holder, master.sprite_name);
		m_imgIcon.color = master.GetSpriteColor();

		m_data = _data;
		m_btn.onClick.AddListener(() =>
		{
			OnBannerEvent.Invoke(m_data);
		});
	}

	public void Initialize()
	{
		m_txtName.text = "はずす";
		m_txtDetail.text = "";
		m_imgIcon.sprite = SpriteManager.Instance.Get("UI_Icon_GenderMale2");
		m_imgIcon.color = Color.white;

		m_btn.onClick.AddListener(() =>
		{
			OnBannerEvent.Invoke(null);
		});
	}





}
