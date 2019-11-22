using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShortcutBase : MonoBehaviour {

	public GameObject m_prefBanner;
	public GameObject m_goContents;

	//private List<BannerShortcut> shortcut_list = new List<BannerShortcut>();

	public UnityEventInt OnClickBanner = new UnityEventInt();

	public virtual IEnumerator Show( int _iCategory )
	{
		m_prefBanner.SetActive(false);
		BannerShortcut[] arr = m_goContents.GetComponentsInChildren<BannerShortcut>();
		foreach (BannerShortcut banner in arr)
		{
			Destroy(banner.gameObject);
		}
		/*
		foreach( DataItemParam p in DataManager.Instance.dataItem.list)
		{
			Debug.Log(string.Format("num={0} shortcutable={1}", p.num, p.shortcutable));
		}
		*/

		List<DataItemParam> data_list;

		if (_iCategory == 0)
		{
			data_list = DataManager.Instance.dataItem.list.FindAll(p => 0 < p.num && 0 < p.shortcutable);
		}
		else {
			data_list = DataManager.Instance.dataItem.list.FindAll(p => 0 < p.num && p.item_id / MasterItem.LargeCategory== _iCategory && 0 < p.shortcutable);
		}

		DataItemParam empty = new DataItemParam();
		empty.item_id = 0;
		data_list.Insert(0, empty);
		//List<DataItemParam> data_list = DataManager.Instance.dataItem.list.FindAll(p => 0 < p.num && 0 < p.shortcutable);

		foreach ( DataItemParam param in data_list)
		{
			BannerShortcut script = PrefabManager.Instance.MakeScript<BannerShortcut>(m_prefBanner, m_goContents);
			script.Initialize(param);
			script.OnClickBanner.AddListener((int _arg) =>
			{
				OnClickBanner.Invoke(_arg);
			});
			yield return null;
		}

	}

}
