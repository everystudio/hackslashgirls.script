using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace GemBookAction {

	public class GemBookActionBase : FsmStateAction
	{
		protected GemBook gemBook;
		public override void OnEnter()
		{
			base.OnEnter();
			gemBook = Owner.GetComponent<GemBook>();
		}

		protected bool CanUseBook()
		{
			DataItemParam data_item = DataManager.Instance.dataItem.list.Find(p => p.item_id == Defines.ITEM_ID_GEM_BOOK && 0 < p.num);
			if (data_item != null && Advertisement.IsReady())
			{
				return true;
			}
			return false;
		}
	}

	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class wait : GemBookActionBase
	{
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (DataManager.Instance.Initialized)
			{
				Finish();
			}

		}

	}



	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class Disable : GemBookActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gemBook.m_btnGem.gameObject.SetActive(false);
			gemBook.m_panelBookCheck.gameObject.SetActive(false);
			Finish();
		}
	}


	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class Standby : GemBookActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (CanUseBook())
			{
				Finish();
			}
		}
	}

	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class Idle : GemBookActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gemBook.m_btnGem.gameObject.SetActive(true);
			gemBook.m_panelBookCheck.gameObject.SetActive(false);

			gemBook.m_btnGem.onClick.AddListener(() =>
			{
				Finish();
			});
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(!CanUseBook())
			{
				Fsm.Event("disable");
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			gemBook.m_btnGem.onClick.RemoveAllListeners();
		}
	}

	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class check : GemBookActionBase
	{
		public FsmInt add_num;
		public override void OnEnter()
		{
			base.OnEnter();
			gemBook.m_panelBookCheck.gameObject.SetActive(true);
			add_num.Value = gemBook.m_panelBookCheck.Show();
			gemBook.m_panelBookCheck.m_btnUse.onClick.AddListener(() =>
			{
				Fsm.Event("use");
			});
			gemBook.m_panelBookCheck.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("cancel");
			});
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!CanUseBook())
			{
				Fsm.Event("disable");
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			gemBook.m_panelBookCheck.m_btnUse.onClick.RemoveAllListeners();
			gemBook.m_panelBookCheck.m_btnCancel.onClick.RemoveAllListeners();
		}
	}
	[ActionCategory("GemBookAction")]
	[HutongGames.PlayMaker.Tooltip("GemBookAction")]
	public class use : GemBookActionBase
	{
		public FsmInt add_num;
		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data_item = DataManager.Instance.dataItem.list.Find(p => p.item_id == Defines.ITEM_ID_GEM_BOOK);

			//Debug.Log(data_item.num);
			if( 0 < data_item.num)
			{
				data_item.num -= 1;
			}
			DataManager.Instance.dataItem.Save();

			GameMain.Instance.ShortcutRefresh(data_item.serial);


			if ( false == GameMain.Instance.ShowAd(add_num.Value))
			{
				GameMain.Instance.panelCaution.Show("注意", "現在秘伝書は使えません。\nしばらく時間をあけてください");
			}
			else
			{

			}

			Finish();

		}

	}



}
