using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPBarControl : MonoBehaviour {

	[SerializeField]
	private string current_label;
	[SerializeField]
	private string max_label;

	private int current_param;
	private int max_param;

	//[SerializeField]
	//private EnergyBar energy_bar;

	[SerializeField]
	private TextMeshProUGUI text_param;

	/*
	void Start () {
		DataManager.Instance.user_data.AddListener(current_label, (string _str) =>
		{
			current_param = int.Parse(_str);
			ParamUpdate(current_param, max_param);

		});
		DataManager.Instance.user_data.AddListener(max_label, (string _str) =>
		{
			max_param = int.Parse(_str);
			ParamUpdate(current_param, max_param);
		});
	}
	*/
	void ParamUpdate(int _iCurrent, int _iMax)
	{
		current_param = _iCurrent;
		max_param = _iMax;

		text_param.text = string.Format("{0}/{1}", current_param, max_param);

		//energy_bar.SetValueMax(max_param);
		//energy_bar.SetValueCurrent(current_param);

	}

	void Update()
	{
		bool bUpdate = false;
		if( current_param != DataManager.Instance.dataChara.hp)
		{
			bUpdate = true;
		}
		else if( max_param != DataManager.Instance.dataChara.hp_max)
		{
			bUpdate = true;
		}
		else
		{
			;
		}
		if( bUpdate)
		{
			ParamUpdate(DataManager.Instance.dataChara.hp, DataManager.Instance.dataChara.hp_max);
		}
	}



}
