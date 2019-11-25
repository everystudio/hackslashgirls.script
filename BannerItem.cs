using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerItem : MonoBehaviour {

	public Button btn
	{
		get { return m_btn; }
	}

	public PanelItems.UnityEventDataItemParam HandleBannerEvent = new PanelItems.UnityEventDataItemParam();

	[SerializeField]
	private Button m_btn;
	[SerializeField]
	private Image m_imgIcon;

	[SerializeField]
	private TextMeshProUGUI m_txtName;
	[SerializeField]
	private TextMeshProUGUI m_txtNum;
	[SerializeField]
	private TextMeshProUGUI m_txtDetail;

	private DataItemParam m_tempDataParam;

	public void Initialize(DataItemParam _param , MasterItemParam _master)
	{
		m_tempDataParam = _param;

		m_txtName.text = _master.name;

		m_txtDetail.text = _master.detail;

		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_holder, _master.sprite_name);
		m_imgIcon.color = _master.GetSpriteColor();


		string strNum = "";
		if( _master.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			strNum = string.Format("[{0}]" , _param.num);
		}
		else
		{
			strNum = string.Format("[{1}{0}]", _param.craft_count < _master.limit ? _param.craft_count.ToString() : "★" , "+" );
		}
		m_txtNum.text = strNum;

		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_holder, _master.sprite_name);
		m_imgIcon.color = _master.GetSpriteColor();

	}

	void Start()
	{
		btn.onClick.AddListener(() =>
		{
			HandleBannerEvent.Invoke(m_tempDataParam);
		});
	}

}
