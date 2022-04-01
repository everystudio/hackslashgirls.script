using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelExpControl : MonoBehaviour {

	public int level { get; set; }

	public long exp { get; set; }
	public long exp_next { get; set; }
	public long exp_pre { get; set; }

	// ゲージ用
	public long exp_delta { get; set; }
	public long exp_delta_max { get; set; }

	[SerializeField]
	private TextMeshProUGUI m_textLevel;
	//[SerializeField]
	//private EnergyBar exp_bar;

	public void RefreshLevel()
	{
		if( level != DataManager.Instance.dataChara.level)
		{
			level = DataManager.Instance.dataChara.level;
			m_textLevel.text = level.ToString();
		}
	}

	public void RefreshExp()
	{

		bool bUpdateBar = false;
		if( exp_next != DataManager.Instance.dataChara.exp_next)
		{
			exp_next = DataManager.Instance.dataChara.exp_next;
			exp_pre = DataManager.Instance.dataChara.exp_pre;
		}

		if (exp != DataManager.Instance.dataChara.exp)
		{
			bUpdateBar = true;
			exp = DataManager.Instance.dataChara.exp;
		}

		if(bUpdateBar)
		{
			exp_delta = exp - exp_pre;
			exp_delta_max = exp_next - exp_pre;

			//exp_bar.SetValueCurrent((int)exp_delta);
			//exp_bar.SetValueMax((int)exp_delta_max);

			//Debug.Log(exp_delta);
			//Debug.Log(exp_delta_max);
		}

	}

	void Update()
	{
		RefreshLevel();
		RefreshExp();
	}


}
