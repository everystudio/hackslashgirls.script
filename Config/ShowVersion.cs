using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVersion : MonoBehaviour {

	public ConfigHolder configHolder;
	public TMPro.TextMeshProUGUI txtShow;
	void Start()
	{
		if( configHolder.app_environment == "development")
		{
			txtShow.color = Color.yellow;
		}
		txtShow.text = configHolder.version;
	}

}
