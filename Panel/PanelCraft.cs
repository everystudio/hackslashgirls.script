using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PanelCraft : MonoBehaviour {

	public Button m_btnBase;
	public Button m_btnElement;
	public Button m_btnResult;		// ないね

	public Image m_imgBaseIcon;
	public Image m_imgElementIcon;
	public Image m_imgResultIcon;

	public TextMeshProUGUI m_textBaseName;
	public TextMeshProUGUI m_textElementName;
	public TextMeshProUGUI m_textResultName;
	public TextMeshProUGUI m_textBikou;

	public TextMeshProUGUI m_textCraftCount;

	public Button m_btnExe;
	public Button m_btnBulk;
	public Button m_btnReset;
	public Button m_btnCancel;

	public GameObject m_prefBaseBanner;
	public GameObject m_goContents;

	public GameObject m_goPanelSelect;

	public PanelCategory m_panelCategory;
	public Button m_btnBaseCancel;
	public Button m_btnFooterStatus;    // キャンセル用

	public PanelCraftBulkSetting panelCraftBuldSetting;

	public int m_iCraftBulkCount;

	[System.Serializable]
	public class UnityEventDataItemParam : UnityEvent<DataItemParam>
	{
	}

	public UnityEventDataItemParam HandleClickBanner = new UnityEventDataItemParam();


	public bool Initialized = false;

	void Start()
	{
		m_prefBaseBanner.SetActive(false);
		Initialized = true;
	}

	public void Startup()
	{
		m_textCraftCount.text = string.Format("x{0}", m_iCraftBulkCount);

		m_textBaseName.text = "強化するアイテムを選択";
		m_textElementName.text = "素材にするアイテム";
		m_textResultName.text = "強化結果";

		CloseList();
	}

	public void LabelUpdate(string _base , string _element , string _result )
	{
		m_textBaseName.text = _base;
		m_textElementName.text = _element;
		m_textResultName.text = _result;
	}

	public IEnumerator ShowList( int _iCategory)
	{
		clear_list();

		m_goPanelSelect.SetActive(true);

		List<DataItemParam> param_list = null;
		if( _iCategory == 0)
		{
			param_list = DataManager.Instance.dataItem.list.FindAll(p => MasterItem.CategoryWeapon <= p.item_id / MasterItem.LargeCategory);
		}
		else
		{
			param_list = DataManager.Instance.dataItem.list.FindAll(p => p.item_id / MasterItem.LargeCategory == _iCategory);
		}
		yield return StartCoroutine(show_list(param_list));
	}

	public void CloseList()
	{
		m_goPanelSelect.SetActive(false);
	}

	private void clear_list()
	{
		BannerCraftBase[] arr = m_goContents.GetComponentsInChildren<BannerCraftBase>();
		foreach (BannerCraftBase banner in arr)
		{
			Destroy(banner.gameObject);
		}
	}

	private IEnumerator show_list(List<DataItemParam> item_list)
	{
		foreach( DataItemParam param in item_list)
		{
			BannerCraftBase script = PrefabManager.Instance.MakeScript<BannerCraftBase>(m_prefBaseBanner, m_goContents);
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
