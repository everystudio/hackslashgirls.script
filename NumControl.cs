using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumControl : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI txt_num;

	public string m_strKey;
	void Start()
	{
		DataManager.Instance.user_data.AddListener(m_strKey, (string _str) =>
		 {
			 txt_num.text = _str;
		 });
	}



}
