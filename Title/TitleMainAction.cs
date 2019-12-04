using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TitleMainAction {
	public class TitleMainActionBase : FsmStateAction
	{
		protected TitleMain titleMain;
		public override void OnEnter()
		{
			base.OnEnter();
			titleMain = Owner.GetComponent<TitleMain>();
		}
	}
	[ActionCategory("TitleMainAction")]
	[HutongGames.PlayMaker.Tooltip("TitleMainAction")]
	public class Startup : TitleMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			string file = string.Format("{0}.csv", "test1/user_data");
			string fullpath = System.IO.Path.Combine(Application.persistentDataPath, file);
			Debug.Log(System.IO.File.Exists(fullpath));
			if (System.IO.File.Exists( fullpath ) == true)
			{
				
			}




		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}
	}

}
