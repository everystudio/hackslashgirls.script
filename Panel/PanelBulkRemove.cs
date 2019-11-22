using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelBulkRemove : MonoBehaviour {

	public GameObject m_goContents;
	public TextMeshProUGUI m_prefText;

	public TextMeshProUGUI m_txtRemoveNum;

	public Button m_btnRemove;
	public Button m_btnCancel;

	private int[] remove_item_id = new int[]
	{
		30001,
		31001,
		32001,
		33001,
		34001,
		35001,
		40001,
		41001,
		42001,
		43001,
		44001,
		45001,
		50001,
		60001,
		70001,
	};

	List<MasterItemParam> remove_master_list;
	public List<DataItemParam> remove_data_list;

	bool m_bInitialized = false;

	public void Initialize()
	{
		if (m_bInitialized == false)
		{
			remove_master_list = new List<MasterItemParam>();

			foreach (int item_id in remove_item_id)
			{
				MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == item_id);
				remove_master_list.Add(master);
			}

			m_prefText.gameObject.SetActive(false);
			foreach (MasterItemParam m in remove_master_list)
			{
				TextMeshProUGUI tmpugui = PrefabManager.Instance.MakeScript<TextMeshProUGUI>(m_prefText.gameObject, m_goContents);
				tmpugui.text = m.GetItemName(0);

			}
		}

		remove_data_list = new List<DataItemParam>();
		foreach ( DataItemParam data in DataManager.Instance.dataItem.list)
		{
			if( data.craft_count == 0)
			{
				foreach( int item_id in remove_item_id)
				{
					if( data.item_id == item_id)
					{
						remove_data_list.Add(data);
						break;
					}
				}
			}
		}
		m_txtRemoveNum.text = string.Format("破棄対象アイテム数[{0}]", remove_data_list.Count);

		m_bInitialized = true;
	}





}
