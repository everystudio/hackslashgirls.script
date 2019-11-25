using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerStore : MonoBehaviour {

	public Button btn
	{
		get { return m_btn; }
	}

	public PanelStore.UnityEventMasterItemParam HandleBannerEvent = new PanelStore.UnityEventMasterItemParam();

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

	private MasterItemParam m_tempMasterItemParam;

	public void Initialize(MasterItemParam _param)
	{
		m_txtName.text = _param.name;
		m_txtDetail.text = _param.detail;

		m_txtNum.text = string.Format("{0}", _param.price.ToString());
		m_tempMasterItemParam = _param;

		m_imgIcon.sprite = SpriteManager.Instance.Get(_param.sprite_holder, _param.sprite_name);
		m_imgIcon.color = _param.GetSpriteColor();
	}

	void Start()
	{
		btn.onClick.AddListener(() =>
		{
			HandleBannerEvent.Invoke(m_tempMasterItemParam);
		});
	}



}
