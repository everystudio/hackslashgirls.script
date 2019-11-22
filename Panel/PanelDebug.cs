using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDebug : MonoBehaviour {

	public void HP_minus5()
	{
		DataManager.Instance.user_data.AddInt("hp", -5);
	}

	public void Hunger_minus5()
	{
		DataManager.Instance.user_data.AddInt("hunger", -5);
	}



}
