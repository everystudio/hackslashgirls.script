using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUnityAdsBonus : MonoBehaviour {

	public TMPro.TextMeshProUGUI m_txtNum;

	public void SetNum(int _iNum)
	{
		m_txtNum.text = string.Format("× {0}", _iNum);
	}

	public Button m_btnClose;

	void Start()
	{
		m_btnClose.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}

}
