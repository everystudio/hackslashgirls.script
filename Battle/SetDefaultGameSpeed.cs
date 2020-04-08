using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetDefaultGameSpeed : MonoBehaviour {

	public TextMeshProUGUI m_txtSpeed;

	private int now_speed_level;
	public void Initialize()
	{
		if( DataManager.Instance.user_data.HasKey(Defines.KEY_GAMESPEED_LEVEL))
		{
			now_speed_level = DataManager.Instance.user_data.ReadInt(Defines.KEY_GAMESPEED_LEVEL);
			m_txtSpeed.text = now_speed_level.ToString();
		}
		else
		{
			now_speed_level = 1;
			m_txtSpeed.text = "1";
		}
	}

	public void Save()
	{

		int speed_level = DataManager.Instance.gameSpeedControl.GetSpeedLevel();

		if( now_speed_level != speed_level)
		{
			DataManager.Instance.user_data.WriteInt(Defines.KEY_GAMESPEED_LEVEL, speed_level);
			m_txtSpeed.text = speed_level.ToString();
			DataManager.Instance.user_data.Save();

		}
		GameMain.Instance.BattleLog("<color=yellow>Set current game speed to default speed</color>");

	}


}
