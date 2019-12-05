using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumControlBestFloor : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI txt_num;

	void Update()
	{
		if (DataManager.Instance.Initialized)
		{
			txt_num.text = DataManager.Instance.floor_best.ToString();

		}
	}



}
