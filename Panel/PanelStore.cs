using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelStore : MonoBehaviour {

	[System.Serializable]
	public class UnityEventMasterItemParam : UnityEvent<MasterItemParam>
	{
	}

	public UnityEventMasterItemParam HandleClickBanner = new UnityEventMasterItemParam();

	public TMPro.TextMeshProUGUI m_txtStoreTitle;

	public GameObject m_prefStoreBanner;
	public GameObject m_goStoreListContents;
	public PanelCategory m_panelCategory;

	public PanelBuyCheck m_panelBuyCheck;

	[HideInInspector]
	public bool Initialized = false;

	void Start()
	{
		m_prefStoreBanner.SetActive(false);
		m_panelBuyCheck.gameObject.SetActive(false);
		Initialized = true;
	}

	public void ClearList()
	{
		BannerStore[] arr = m_goStoreListContents.GetComponentsInChildren<BannerStore>();
		foreach( BannerStore banner in arr)
		{
			//Debug.Log(banner.gameObject.name);
			Destroy(banner.gameObject);
		}
	}

	public void ShowList(int _iShopId , int _iCategory)
	{
		m_txtStoreTitle.text = string.Format("Store(Lv{0})", _iShopId);

		ClearList();
		List<MasterItemParam> item_param_list;
		if (_iCategory == 0)
		{
			item_param_list = DataManager.Instance.masterItem.list.FindAll(p=>p.shop_id != 0 && p.shop_id <= _iShopId );
		}
		else {
			item_param_list = DataManager.Instance.masterItem.list.FindAll(p => p.shop_id != 0 && p.shop_id <= _iShopId && p.item_id / MasterItem.LargeCategory == _iCategory);
		}
		foreach( MasterItemParam param in item_param_list)
		{
			 BannerStore script = PrefabManager.Instance.MakeScript<BannerStore>(m_prefStoreBanner, m_goStoreListContents);
			script.Initialize(param);
			script.HandleBannerEvent.AddListener(OnBannerEvent);
		}
	}

	private void OnBannerEvent(MasterItemParam arg0)
	{
		HandleClickBanner.Invoke(arg0);
	}
}











