using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelStoreAction {
	public class PanelStoreActionBase : FsmStateAction
	{
		protected PanelStore panel_store;
		public override void OnEnter()
		{
			base.OnEnter();
			panel_store = Owner.GetComponent<PanelStore>();
		}
	}
	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class Startup : PanelStoreActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (panel_store.Initialized)
			{
				Finish();
			}
		}
	}
	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class ShopList : PanelStoreActionBase
	{
		public FsmInt shop_id;
		public FsmInt category;

		public FsmInt item_id;

		public override void OnEnter()
		{
			base.OnEnter();


			int iFloorBest = DataManager.Instance.GetBestFloor("normal");

			shop_id.Value = (iFloorBest - 1) / 100 + 1;

			panel_store.m_panelCategory.HandleCategoryButton.AddListener(OnCategory);
			panel_store.ShowList(shop_id.Value, category.Value);
			panel_store.HandleClickBanner.AddListener(OnClickBanner);
		}

		private void OnClickBanner(MasterItemParam arg0)
		{
			item_id.Value = arg0.item_id;
			Fsm.Event("select");
		}

		private void OnCategory(int arg0)
		{
			if( arg0 != category.Value)
			{
				category.Value = arg0;
				Fsm.Event("category");
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_store.m_panelCategory.HandleCategoryButton.RemoveListener(OnCategory);
			panel_store.HandleClickBanner.RemoveListener(OnClickBanner);

		}
	}


	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class ItemDetail : PanelStoreActionBase
	{
		public FsmInt item_id;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_store.m_panelBuyCheck.ShowInitialize(item_id.Value);
			Finish();
		}
	}


	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class DetailClose : PanelStoreActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel_store.m_panelBuyCheck.gameObject.SetActive(false);
			Finish();
		}
	}



	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class ItemBuyCheck : PanelStoreActionBase
	{
		public FsmInt item_id;
		public FsmInt buy_num;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_store.m_panelBuyCheck.ShowUpdate(item_id.Value,buy_num.Value);
			panel_store.m_panelBuyCheck.gameObject.SetActive(true);
			
			panel_store.m_panelBuyCheck.m_btnBuy.onClick.AddListener(buy);
			panel_store.m_panelBuyCheck.m_btnBulk.onClick.AddListener(bulk);
			panel_store.m_panelBuyCheck.m_btnCancel.onClick.AddListener(cancel);
			panel_store.m_panelBuyCheck.m_btnClose.onClick.AddListener(cancel);

			panel_store.m_panelBuyCheck.m_btnPlus.onClick.AddListener(plus);
			panel_store.m_panelBuyCheck.m_btnMinus.onClick.AddListener(minus);

		}

		private void plus()
		{
			if( 1000 <= buy_num.Value)
			{
				SEControl.Instance.Play("cancel_01");
			}
			else
			{
				buy_num.Value *= 10;
				SEControl.Instance.Play("Locker 1");

				panel_store.m_panelBuyCheck.ShowUpdate(item_id.Value, buy_num.Value);
			}
		}
		private void minus()
		{
			if (buy_num.Value <= 1)
			{
				SEControl.Instance.Play("cancel_01");
			}
			else
			{
				buy_num.Value /= 10;
				SEControl.Instance.Play("Locker 1");

				panel_store.m_panelBuyCheck.ShowUpdate(item_id.Value, buy_num.Value);
			}

		}

		private void buy()
		{
			Fsm.Event("buy");
		}

		private void bulk()
		{
			Fsm.Event("bulk");
		}

		private void cancel()
		{
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_store.m_panelBuyCheck.m_btnBuy.onClick.RemoveListener(buy);
			panel_store.m_panelBuyCheck.m_btnBulk.onClick.RemoveListener(bulk);
			panel_store.m_panelBuyCheck.m_btnCancel.onClick.RemoveListener(cancel);
			panel_store.m_panelBuyCheck.m_btnClose.onClick.RemoveListener(cancel);

			panel_store.m_panelBuyCheck.m_btnPlus.onClick.RemoveListener(plus);
			panel_store.m_panelBuyCheck.m_btnMinus.onClick.RemoveListener(minus);

		}
	}

	[ActionCategory("PanelStoreAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStoreAction")]
	public class Buy : PanelStoreActionBase
	{
		public FsmInt item_id;
		public FsmInt buy_num;

		private bool is_success;

		public override void OnEnter()
		{
			is_success = false;
			base.OnEnter();

			MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == item_id.Value);

			int iCoin = DataManager.Instance.user_data.ReadInt(Defines.KeyCoin);
			if( master.price * buy_num.Value <= iCoin)
			{
				DataManager.Instance.user_data.AddInt(Defines.KeyCoin, -1 * master.price * buy_num.Value);
				if (DataManager.Instance.dataItem.AddItem(item_id.Value, buy_num.Value))
				{
					is_success = true;
					Fsm.Event("success");
				}
				else
				{
					Fsm.Event("false");
				}
			}
			else
			{
				Fsm.Event("fail");
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			if( is_success)
			{
				DataManager.Instance.user_data.Save();
				DataManager.Instance.dataItem.Save();
			}
		}
	}



}
