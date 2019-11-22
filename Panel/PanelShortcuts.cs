using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelShortcuts : MonoBehaviour {

	public List<BtnShortcut> shortcut_list = new List<BtnShortcut>();

	public BtnShortcut.EventShortcut OnShortcut = new BtnShortcut.EventShortcut();
	public BtnShortcut.EventShortcut OnLongTap = new BtnShortcut.EventShortcut();

	void Start()
	{
		foreach( BtnShortcut btn in shortcut_list)
		{
			btn.OnShortcut.AddListener(HandleShortcut);
			btn.OnLongTap.AddListener(HandleLongTap);
		}
	}

	public void Refresh()
	{
		foreach (BtnShortcut btn in shortcut_list)
		{
			btn.Refresh();

		}

	}

	public bool CheckSerial( int _iSerial , ref int _iIndex )
	{
		foreach(BtnShortcut btn in shortcut_list)
		{
			if( btn.item_serial == _iSerial)
			{
				_iIndex = btn.btn_index;
				return true;
			}
		}

		return false;
	}

	public bool RefreshSerial( int _iSerial)
	{
		bool bRet = false;

		foreach (BtnShortcut btn in shortcut_list)
		{
			if( btn.item_serial == _iSerial)
			{
				btn.Refresh();
			}
		}
		return bRet;
	}

	void HandleShortcut(BtnShortcut _shortcut)
	{
		//Debug.Log("shortcut:" + _shortcut.btn_index.ToString());
		OnShortcut.Invoke(_shortcut);
	}

	void HandleLongTap(BtnShortcut _shortcut)
	{
		//Debug.Log("longtap:" + _shortcut.btn_index.ToString());
		OnLongTap.Invoke(_shortcut);
	}

}
