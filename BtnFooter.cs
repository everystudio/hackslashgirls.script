using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnFooter : MonoBehaviour {

	[SerializeField]
	private string footer_name;

	public string GetFooterName()
	{
		return footer_name;
	}

	[SerializeField]
	private Button m_btn;

	[SerializeField]
	private Image m_imgIcon;

	public Button btn { get { return m_btn; } }



}
