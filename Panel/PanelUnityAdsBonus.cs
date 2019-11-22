using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUnityAdsBonus : MonoBehaviour {

	public Button m_btnClose;

	void Start()
	{
		m_btnClose.onClick.AddListener(() =>
		{
			gameObject.SetActive(false);
		});
	}

}
