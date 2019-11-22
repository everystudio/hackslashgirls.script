using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SEAction {

	[ActionCategory("SEAction")]
	[HutongGames.PlayMaker.Tooltip("SEAction")]
	public class PlaySE : FsmStateAction
	{
		public FsmString se_name;
		public override void OnEnter()
		{
			base.OnEnter();
			SEControl.Instance.Play(se_name.Value);
			Finish();

		}
	}

}
