using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

public class PanelUseItem : MonoBehaviour {
	[SerializeField]
	private Image m_imgIcon;

	[SerializeField]
	private TextMeshProUGUI m_txtName;
	[SerializeField]
	private TextMeshProUGUI m_txtDetail;
	[SerializeField]
	private TextMeshProUGUI m_txtNum;

	[SerializeField]
	private TextMeshProUGUI m_txtAct;
	[SerializeField]
	private TextMeshProUGUI m_txtDelete;

	[SerializeField]
	public Button m_btnAct;
	[SerializeField]
	public Button m_btnDelete;
	[SerializeField]
	public Button m_btnClose;

	public DataItemParam data_param;
	public MasterItemParam master_param;

	public void Show(int _iItemSerial)
	{
		data_param = DataManager.Instance.dataItem.list.Find(p => p.serial == _iItemSerial);
		master_param = DataManager.Instance.masterItem.list.Find(p => p.item_id == data_param.item_id);

		m_txtDetail.text = master_param.detail;

		if( master_param.item_id / MasterItem.LargeCategory == MasterItem.CategoryAccessary)
		{
			string seinou = "";

			MasterAccessaryParam accessary = DataManager.Instance.masterAccessary.list.Find(p => p.item_id == master_param.item_id);
			MasterItemParam use_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == accessary.use_item_id);
			if( 0 < accessary.hp_rate)
			{
				seinou = string.Format("ショートカットにセットしている時、<color=#FF0>HPが{0}％未満</color>になると、<color=#0FF>{1}</color>を使用。({2:0.0}秒間隔)", accessary.hp_rate, use_item.name, accessary.interval);
			}
			else if( 0 < accessary.stamina_rate)
			{
				seinou = string.Format("ショートカットにセットしている時、<color=#FF0>Staminaが{0}％未満</color>になると、<color=#0FF>{1}</color>を使用。({2:0.0}秒間隔)", accessary.hp_rate, use_item.name, accessary.interval);
			}
			m_txtDetail.text += "\n" + seinou;
			/*
			*/
		}


		m_btnAct.interactable = true;

		if (master_param.item_id == 11001)
		{
			if (Advertisement.IsReady())
			{
				m_txtDetail.text += "\n<color=green>現在使用可能です</color>";
			}
			else
			{
				m_txtDetail.text += "\n<color=red>現在使用できません。しばらく時間を空けて下さい</color>";
			}
		}

		m_imgIcon.sprite = SpriteManager.Instance.Get(master_param.sprite_holder, master_param.sprite_name);
		m_imgIcon.color = master_param.GetSpriteColor();

		if (master_param.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			m_txtName.text = master_param.name;
			m_txtAct.text = "使用";
			m_txtDelete.text = "捨てる";

		}
		else if (master_param.item_id / MasterItem.LargeCategory == MasterItem.CategorySkin)
		{
			m_txtName.text = master_param.name;
			m_txtAct.text = "----";
			m_txtDelete.text = "捨てる";
		}
		else if (master_param.item_id / MasterItem.LargeCategory == MasterItem.CategoryAccessary)
		{
			m_txtName.text = master_param.name;
			m_txtAct.text = "----";
			m_txtDelete.text = "捨てる";

			m_btnAct.interactable = false;
			m_btnDelete.interactable = false;

		}
		else
		{
			m_txtName.text = master_param.GetItemName(data_param.craft_count);
			m_txtAct.text = "装備";
			m_txtDelete.text = string.Format("Gem変換\n{0}個", data_param.craft_count); ;
		}

	}

	public void Refresh()
	{
		m_txtNum.text = data_param.num.ToString();

		if( 0 < data_param.num)
		{
			m_btnAct.interactable = true;
			m_btnDelete.interactable = true;


			if (master_param.item_id == 11001)
			{
				if (Advertisement.IsReady())
				{
				}
				else
				{
					m_btnAct.interactable = false;
				}
			}
			else if (master_param.item_id / MasterItem.LargeCategory == MasterItem.CategoryAccessary)
			{
				m_btnAct.interactable = false;
				m_btnDelete.interactable = false;
			}


		}
		else
		{
			m_btnAct.interactable = false;
			m_btnDelete.interactable = false;
		}




	}

}






