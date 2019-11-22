using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelBottomAction
{
	public class PanelBottomActionBase : FsmStateAction
	{
		protected PanelBottom panel_bottom;
		public override void OnEnter()
		{
			base.OnEnter();
			panel_bottom = Owner.GetComponent<PanelBottom>();
		}
	}
	[ActionCategory("PanelBottomAction")]
	[HutongGames.PlayMaker.Tooltip("PanelBottomAction")]
	public class Initialize : PanelBottomActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}

	[ActionCategory("PanelBottomAction")]
	[HutongGames.PlayMaker.Tooltip("PanelBottomAction")]
	public class FooterEvent : PanelBottomActionBase
	{
		public FsmString footer_event;
		public FsmString ignore_event;

		public override void OnEnter()
		{
			base.OnEnter();

			panel_bottom.panel_footer.HandleFooterButton.AddListener(OnFooterButton);
		}

		private void OnFooterButton(string arg0)
		{
			//Debug.Log(arg0);
			if (arg0 != ignore_event.Value)
			{
				footer_event.Value = arg0;
				Fsm.Event("footer");
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_bottom.panel_footer.HandleFooterButton.RemoveListener(OnFooterButton);
		}
	}

	[ActionCategory("PanelBottomAction")]
	[HutongGames.PlayMaker.Tooltip("PanelBottomAction")]
	public class Footer : PanelBottomActionBase
	{
		public FsmString footer_event;
		public override void OnEnter()
		{
			base.OnEnter();
			Fsm.Event(footer_event.Value);
		}

	}
}
