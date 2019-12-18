using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.Advertisements;

public class BtnShortcut : MonoBehaviour {

	[System.Serializable]
	public class EventShortcut : UnityEvent<BtnShortcut>
	{
	}
	public EventShortcut OnShortcut = new EventShortcut();
	public EventShortcut OnLongTap = new EventShortcut();

	public class ShortcutData
	{
		public ShortcutData()
		{

		}
	}
	[SerializeField]
	private int m_iBtnIndex;
	public int btn_index { get { return m_iBtnIndex; } }
	private int m_iItemSerial = -1;
	private int m_iItemNum = 0;
	public int item_serial { get { return m_iItemSerial; } }
	public DataItemParam m_dataSetItemParam;
	private MasterItemParam m_masterSetItemParam;

	private AccessaryBase accessaryBase;

	[SerializeField]
	private Button m_btn;
	[SerializeField]
	private TextMeshProUGUI m_txtName;
	[SerializeField]
	private TextMeshProUGUI m_txtNum;
	[SerializeField]
	private Image m_imgIcon;

	[SerializeField]
	private Sprite m_sprEmpty;

	public void Initialize(Sprite _spr , string _strName , int _iNum )
	{
		m_imgIcon.sprite = _spr;
		m_imgIcon.color = Color.white;

		m_txtName.text = _strName;

		string strNum = "";
		if( 0 <= _iNum)
		{
			strNum = string.Format("x{0}", _iNum);
		}
		m_txtNum.text = strNum;
	}

	public void Refresh()
	{
		int iItemSerial = DataManager.Instance.user_data.ReadInt(DataManager.Instance.GetShortcutKey(m_iBtnIndex));

		// なんか外れてた場合のフォロー
		DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == iItemSerial);
		if( data == null)
		{
			iItemSerial = 0;
			//Debug.Log("not found shortcut serial");
		}


		//Debug.Log(string.Format("index:{0} serial:{1}", m_iBtnIndex, iItemSerial));
		if (m_iItemSerial != iItemSerial )
		{
			SetShortcut(iItemSerial);
		}
		else if((m_dataSetItemParam != null && m_dataSetItemParam.num != m_iItemNum))
		{
			NumUpdate();
		}
		else
		{

		}
	}
	public void Empty()
	{
		m_imgIcon.sprite = m_sprEmpty;
		m_txtNum.text = "";

		m_iItemSerial = 0;
		m_dataSetItemParam = null;

		if (accessaryBase != null)
		{
			accessaryBase.accessary = null;
		}
	}

	public void SetShortcut(int _iItemSerial)
	{
		m_iItemSerial = _iItemSerial;
		//Debug.Log(m_iItemSerial);
		if ( m_iItemSerial == 0)
		{
			Empty();
			return;
		}

		m_dataSetItemParam = DataManager.Instance.dataItem.list.Find(p => p.serial == _iItemSerial);
		m_masterSetItemParam = DataManager.Instance.masterItem.list.Find(p => p.item_id == m_dataSetItemParam.item_id);

		NumUpdate();

		m_imgIcon.sprite = SpriteManager.Instance.Get(m_masterSetItemParam.sprite_holder, m_masterSetItemParam.sprite_name);
		m_imgIcon.color = m_masterSetItemParam.GetSpriteColor();

	}

	private void NumUpdate()
	{
		string strNum = "";
		if (m_dataSetItemParam != null && DataItem.IsCategoryConsumable(m_dataSetItemParam.item_id))
		{
			strNum = string.Format("x{0}", m_dataSetItemParam.num);
		}
		else if (m_dataSetItemParam != null && m_dataSetItemParam.item_id / MasterItem.LargeCategory == MasterItem.CategoryAccessary)
		{
			MasterAccessaryParam ac = DataManager.Instance.masterAccessary.list.Find(p => p.item_id == m_dataSetItemParam.item_id);
			strNum = ac.label;

			if (accessaryBase == null)
			{
				GameObject obj_accessary = new GameObject();
				obj_accessary.name = string.Format("{0}_accessary", gameObject.name);
				accessaryBase = obj_accessary.AddComponent<AccessaryBase>();
			}
			accessaryBase.accessary = ac;
		}
		else if (m_dataSetItemParam != null && m_dataSetItemParam.item_id / MasterItem.LargeCategory == MasterItem.CategoryMagic)
		{
			strNum = string.Format("x{0}", m_dataSetItemParam.num);
		}
		else
		{
			strNum = m_masterSetItemParam.GetItemName(m_dataSetItemParam.craft_count);
		}
		m_txtNum.text = strNum;

		m_iItemNum = m_dataSetItemParam.num;
	}

	private bool is_pressing = false;
	private bool is_longtap = false;
	private float pressing_time;

	void Update()
	{
		if( is_pressing)
		{
			pressing_time += Time.deltaTime / Time.timeScale;
			if(is_longtap == false && DataManager.LONG_TAP_TIME <= pressing_time)
			{
				is_longtap = true;
				OnLongTap.Invoke(this);
			}
		}

		if(m_masterSetItemParam != null && m_masterSetItemParam.item_id == Defines.ITEM_ID_GEM_BOOK)
		{
			if (Advertisement.IsReady())
			{
				m_imgIcon.color = Color.white;
			}
			else
			{
				m_imgIcon.color = Color.gray;
			}
		}
	}

	public void OnPointerDown()
	{
		is_pressing = true;
		pressing_time = 0.0f;
	}

	public void OnPointerUp()
	{
		if( pressing_time < DataManager.LONG_TAP_TIME)
		{
			OnShortcut.Invoke(this);
		}
		is_pressing = false;
		is_longtap = false;
		pressing_time = 0.0f;
	}

}
