using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgingControl : MonoBehaviour {

	public DebugAging debugAging;
	public void OnAging()
	{
		debugAging.gameObject.SetActive(!debugAging.gameObject.activeSelf);
	}

}
