using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVersionHolder : MonoBehaviour {

	[SerializeField]
	private TMPro.TextMeshProUGUI m_textVersion;

	// Use this for initialization
	void Start () {

		m_textVersion.text = DataManager.Instance.config_holder.version;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
