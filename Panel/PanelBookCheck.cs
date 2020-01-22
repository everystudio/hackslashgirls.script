using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBookCheck : MonoBehaviour {

	public TextMeshProUGUI m_txtMessage;

	public Button m_btnUse;
	public Button m_btnCancel;

	public static int GetBookGemNum()
	{
		int num = 50;
		foreach (MasterDungeonParam m in DataManager.Instance.masterDungeon.list)
		{
			int iFloorBest = DataManager.Instance.GetBestFloor(m.dungeon_id);
			num += iFloorBest;
		}
		return num;
	}

	public int Show()
	{
		int num = GetBookGemNum();
		m_txtMessage.text = string.Format("動画視聴を行うと、\n<color=#0FF>{0}</color>個のジェムを\n獲得できます", num);

		return num;
	}


}
