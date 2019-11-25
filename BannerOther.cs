using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerOther : MonoBehaviour {
	[SerializeField]
	public bool IsDebug;

	void Start()
	{
		if( IsDebug == true)
		{
			if( DataManager.Instance.config_holder.app_environment != "development")
			{
				gameObject.SetActive(false);
			}
		}
	}

}
