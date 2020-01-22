using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBookCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtMessage;

	public Button m_btnUse;
	public Button m_btnCancel;

	public int Show()
	{
		int num = 50;
		foreach( MasterDungeonParam m in DataManager.Instance.masterDungeon.list)
		{
			int iFloorBest = DataManager.Instance.GetBestFloor(m.dungeon_id);
			num += iFloorBest;
		}
		m_txtMessage.text = string.Format("動画視聴を行うと、\n{0}個のジェムを\n獲得できます", num);

		return num;
	}


}
