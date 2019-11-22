using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCaution : MonoBehaviour {

	public TextMeshProUGUI m_txtTitle;
	public TextMeshProUGUI m_txtMessage;

	public GameObject m_goRoot;

	void Start()
	{
		Close();
	}

	public void Close()
	{
		m_goRoot.SetActive(false);
	}
	
	public void Show( string _strTitle , string _strMessage)
	{
		m_txtTitle.text = _strTitle;
		m_txtMessage.text = _strMessage;
		m_goRoot.SetActive(true);
	}


}
