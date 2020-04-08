using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnEquip : MonoBehaviour {

	public Button btn;
	public TMPro.TextMeshProUGUI m_txtName;

	public int equip_index;
	public int equip_category;

	public DataItemParam m_equipData = new DataItemParam();
	public MasterItemParam m_equipMaster;

	[SerializeField]
	private Image m_imgIcon;

	public void Refresh()
	{
		int item_serial = DataManager.Instance.user_data.ReadInt(MasterItem.GetEquipIndexName(equip_index));

		//Debug.Log(string.Format("index={0} serial={1}",equip_index, item_serial));
		//Debug.Log(m_equipData);

		if( 0 < item_serial )
		{
			m_equipData = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial);
		}
		else
		{
			m_equipData = null;
		}

		if (m_equipData != null)
		{
			m_equipMaster = DataManager.Instance.masterItem.list.Find(p => p.item_id == m_equipData.item_id);
		}
		else
		{
			m_equipMaster = null;
		}

		string strName = "Empty";
		if( m_equipMaster != null)
		{
			strName = m_equipMaster.GetItemName(m_equipData.craft_count);
			m_imgIcon.color = m_equipMaster.GetSpriteColor();
		}
		else
		{
			m_imgIcon.color = Color.white;
		}
		m_txtName.text = strName;





	}
}





