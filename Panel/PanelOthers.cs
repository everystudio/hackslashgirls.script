using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOthers : MonoBehaviour {

	// リスタート
	public Button m_btnRestart;

	public TMPro.TextMeshProUGUI m_txtRetryFloor;
	public Button m_btnPrev10;
	public Button m_btnPrev1;
	public Button m_btnNext1;
	public Button m_btnNext10;
	public void SetRestartFloor(int _iFloor)
	{
		m_txtRetryFloor.text = string.Format("{0}F", _iFloor);
	}

	public Button m_btnGuide;
	public Button m_btnGuideClose;
	public GameObject m_goGuide;

	public UnityEngine.Audio.AudioMixer mixer;
	public Button m_btnSoundControl;
	public Button m_btnSoundControlDecide;
	public Button m_btnSoundControlCancel;

	public Toggle toggle_stop_100;
	public Toggle toggle_not_sleep;

	public PanelBulkRemove panel_bulk_remove;

	public void Stop100( bool _bFlag)
	{
		if(_bFlag)
		{
			DataManager.Instance.user_data.WriteInt(Defines.KEY_STOP_100, 1);
		}
		else
		{
			DataManager.Instance.user_data.WriteInt(Defines.KEY_STOP_100, 0);
		}
	}

	public void NotSleep(bool _bFlag)
	{
		if (_bFlag)
		{
			DataManager.Instance.user_data.WriteInt(Defines.KEY_NOT_SLEEP, 1);
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
		else
		{
			DataManager.Instance.user_data.WriteInt(Defines.KEY_NOT_SLEEP, 0);
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}
	}

}
