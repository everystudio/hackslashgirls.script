using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCategory : MonoBehaviour {
	[SerializeField]
	private int category;

	public int GetCategory()
	{
		return category;
	}

	[SerializeField]
	private Button m_btn;

	[SerializeField]
	private Image m_imgIcon;

	public Button btn { get { return m_btn; } }

}
