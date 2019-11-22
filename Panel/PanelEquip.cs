using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelEquip : MonoBehaviour {
	public GameObject m_prefBanner;
	public GameObject m_goListContents;

	[System.Serializable]
	public class UnityEventDataItemParam : UnityEvent<DataItemParam>
	{
	}
	public UnityEventDataItemParam OnClickBanner = new UnityEventDataItemParam();

	public UnityEvent OnClose = new UnityEvent();
	[SerializeField]
	private Button m_btnClose;

	void Start()
	{
		m_prefBanner.SetActive(false);
		m_btnClose.onClick.AddListener(() => {
			OnClose.Invoke();
		});

	}

	public IEnumerator Initialize(int _iIndex , int _iCategory)
	{
		BannerEquip[] arr = m_goListContents.GetComponentsInChildren<BannerEquip>();
		foreach (BannerEquip banner in arr)
		{
			// なんか面倒なので除外処理入れておく
			if (m_prefBanner != banner.gameObject)
			{
				Destroy(banner.gameObject);
			}
		}

		int iEquipSerial = DataManager.Instance.user_data.ReadInt(MasterItem.GetEquipIndexName(_iIndex));

		List<DataItemParam> item_param_list = DataManager.Instance.dataItem.list.FindAll(p => p.item_id / MasterItem.LargeCategory == _iCategory && p.serial != iEquipSerial);

		// 装備中の場合は外すボタンを出す
		if (iEquipSerial != 0)
		{
			BannerEquip script_empty = PrefabManager.Instance.MakeScript<BannerEquip>(m_prefBanner, m_goListContents);
			script_empty.Initialize();
			script_empty.OnBannerEvent.AddListener(OnBannerEvent);
		}

		foreach (DataItemParam param in item_param_list)
		{
			BannerEquip script = PrefabManager.Instance.MakeScript<BannerEquip>(m_prefBanner, m_goListContents);
			script.Initialize(param);

			script.OnBannerEvent.AddListener(OnBannerEvent);
			yield return null;
		}

	}
	private void OnBannerEvent(DataItemParam arg0)
	{
		OnClickBanner.Invoke(arg0);
	}

}
