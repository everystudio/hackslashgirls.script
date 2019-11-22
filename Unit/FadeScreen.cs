using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour {

	public enum STATE
	{
		NONE	= 0,
		CLOSE	,
		OPEN	,
	}
	public STATE fade_state;

	public void FadeOut()
	{
		fade_state = STATE.CLOSE;
	}
	public void FadeIn()
	{
		fade_state = STATE.OPEN;
	}

}
