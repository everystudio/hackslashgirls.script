using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelItems : MonoBehaviour {

	[System.Serializable]
	public class UnityEventDataItemParam : UnityEvent<DataItemParam>
	{
	}

	public UnityEventDataItemParam HandleClickBanner = new UnityEventDataItemParam();


	[SerializeField]
	public PanelUseItem m_panelUseItem;

	public GameObject m_prefItemBanner;
	public GameObject m_goItemListContents;
	public PanelCategory m_panelCategory;

	[HideInInspector]
	public bool Initialized = false;

	void Start()
	{
		m_prefItemBanner.SetActive(false);
		m_panelUseItem.gameObject.SetActive(false);
		Initialized = true;
	}

	public void ClearList()
	{
		BannerItem[] arr = m_goItemListContents.GetComponentsInChildren<BannerItem>();
		foreach (BannerItem banner in arr)
		{
			Destroy(banner.gameObject);
		}
	}
	public IEnumerator ShowList( int _iCategory)
	{
		ClearList();
		List<DataItemParam> item_param_list;
		if (_iCategory == 0)
		{
			item_param_list = DataManager.Instance.dataItem.list.FindAll(p => 0 < p.num);
		}
		else {
			item_param_list = DataManager.Instance.dataItem.list.FindAll(p => 0 < p.num && p.item_id / MasterItem.LargeCategory == _iCategory);
		}

		foreach (DataItemParam param in item_param_list)
		{
			BannerItem script = PrefabManager.Instance.MakeScript<BannerItem>(m_prefItemBanner, m_goItemListContents);
			MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == param.item_id);
			script.Initialize(param, master);
			script.HandleBannerEvent.AddListener(OnBannerEvent);
			yield return null;
		}
	}

	private void OnBannerEvent(DataItemParam arg0)
	{
		HandleClickBanner.Invoke(arg0);
	}
}
