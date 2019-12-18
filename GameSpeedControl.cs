using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSpeedControl : MonoBehaviour {

	public int[] speed_arr;
	public GameObject[] img_arr;

	private int Index;
	public int GetSpeedLevel()
	{
		return Index + 1;
	}
	public void SetSpeedLevel(int _iLevel)
	{
		SetIndex(_iLevel - 1);
	}

	public GameObject m_goMeterArrow;
	public GameObject m_goMeterChara;

	void Start()
	{
		if(img_arr.Length != speed_arr.Length)
		{
			Debug.LogError("not same");
		}

		SetIndex(Index);
	}

	public void SetIndex( int _iIndex)
	{
		//Debug.LogWarning(_iIndex);
		Index = _iIndex;
		set_speed();
	}

	public void ChangeSpeed()
	{
		SEControl.Instance.Play("Page 01");
		Index += 1;
		set_speed();



		//GameMain.Instance.ChangeCharaTexture("skin_02_fairy");



	}

	public void Play()
	{
		SetIndex(Index);
	}

	private void set_speed()
	{
		Index = Index % speed_arr.Length;
		Time.timeScale = speed_arr[Index % speed_arr.Length];

		for( int i = 0; i < img_arr.Length; i++)
		{
			bool bSetView = false;
			if( i== Index)
			{
				bSetView = true;
			}
			img_arr[i].SetActive(bSetView);
		}
	}

	public void SetMeter(int value)
	{
		if(value == 1)
		{
			m_goMeterArrow.SetActive(true);
			m_goMeterChara.SetActive(false);
		}
		else
		{
			m_goMeterArrow.SetActive(false);
			m_goMeterChara.SetActive(true);
		}
	}
}



