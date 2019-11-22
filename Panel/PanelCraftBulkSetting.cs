using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCraftBulkSetting : MonoBehaviour {

	public bool is_save { get; set; }
	public Toggle toggle_is_save;

	public Button btnCancel;
	public Button btnOne;
	public Button btnTen;
	public Button btnHundred;

	public void Init()
	{
		toggle_is_save.isOn = DataManager.Instance.user_data.HasKey(Defines.CRAFT_BULK_COUNT);
	}

}
