using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMenuAction {
	public class GameMenuActionBase : FsmStateAction
	{
		protected GameMenu gameMenu;
		public override void OnEnter()
		{
			base.OnEnter();
			gameMenu = Owner.GetComponent<GameMenu>();
		}
	}
	[ActionCategory("GameMenuAction")]
	[HutongGames.PlayMaker.Tooltip("GameMenuAction")]
	public class Close : GameMenuActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMenu.m_animMenu.SetBool("close", true);
		}
	}
	[ActionCategory("GameMenuAction")]
	[HutongGames.PlayMaker.Tooltip("GameMenuAction")]
	public class Open : GameMenuActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMenu.m_animMenu.SetBool("close", false);
		}
	}


}
