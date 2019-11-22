using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecoverNum : MonoBehaviour {

	public TextMeshPro m_txtNum;

	public void Initialize( string _strMessage )
	{
		m_txtNum.text = _strMessage.ToString();
	}
	public void Initialize( int _num)
	{
		Initialize(_num.ToString());
	}

}
