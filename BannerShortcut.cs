using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerShortcut : MonoBehaviour {

	public GameObject m_goSet;
	public Image m_imgIcon;

	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtDetail;
	public TextMeshProUGUI m_txtNum;

	public UnityEventInt OnClickBanner = new UnityEventInt();

	private int m_iItemSerial;
	private MasterItemParam m_masterParam;

	void Start()
	{
		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			OnClickBanner.Invoke(m_iItemSerial);
		});
	}

	public void Initialize( DataItemParam _data)
	{
		m_iItemSerial = _data.serial;

		int iIndex = 0;
		bool exist = GameMain.Instance.panelStatus.m_panelShortcuts.CheckSerial(m_iItemSerial, ref iIndex);

		// 外すは例外
		if( _data.item_id == 0)
		{
			exist = false;
		}
		m_goSet.SetActive(exist);

		if( _data.item_id == 0)
		{
			m_imgIcon.sprite = SpriteManager.Instance.Get("UI_Icon_GenderMale2");
			m_imgIcon.color = Color.white;

			m_txtNum.text = "Remove";
			m_txtName.text = "Set Empty";
			m_txtDetail.text = "";

			return;
		}


		MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == _data.item_id);

		m_imgIcon.sprite = SpriteManager.Instance.Get(master.sprite_holder, master.sprite_name);
		m_imgIcon.color = master.GetSpriteColor();

		string strNum = "";
		if (master.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			strNum = string.Format("[{0}]", _data.num);
		}
		else
		{
			strNum = string.Format("[{1}{0}]", _data.craft_count < master.limit ? _data.craft_count.ToString() : "★", "+");
		}

		m_txtNum.text = strNum;

		m_txtName.text = master.name;
		m_txtDetail.text = master.detail;
	}


}
