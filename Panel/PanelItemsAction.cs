using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelItemsAction
{
	public class PanelItemsActionBase : FsmStateAction
	{
		protected PanelItems panel_items;
		public override void OnEnter()
		{
			base.OnEnter();
			panel_items = Owner.GetComponent<PanelItems>();
		}
	}
	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class Startup : PanelItemsActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (panel_items.Initialized)
			{
				Finish();
			}
		}
	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class ItemList : PanelItemsActionBase
	{
		public FsmInt category;

		public FsmInt item_serial;

		private Coroutine create_list;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_items.m_panelCategory.HandleCategoryButton.AddListener(OnCategory);
			create_list = StartCoroutine( panel_items.ShowList(category.Value) );
			panel_items.HandleClickBanner.AddListener(OnClickBanner);
		}

		private void OnClickBanner(DataItemParam arg0)
		{
			StopCoroutine(create_list);
			item_serial.Value = arg0.serial;
			Fsm.Event("select");
		}

		private void OnCategory(int arg0)
		{
			if (arg0 != category.Value)
			{
				category.Value = arg0;
				Fsm.Event("category");
			}
		}
		public override void OnExit()
		{
			base.OnExit();
			panel_items.m_panelCategory.HandleCategoryButton.RemoveListener(OnCategory);
			panel_items.HandleClickBanner.RemoveListener(OnClickBanner);
		}
	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class ShowItem : PanelItemsActionBase
	{
		public FsmInt item_serial;
		public override void OnEnter()
		{
			base.OnEnter();
			panel_items.m_panelUseItem.gameObject.SetActive(true);
			panel_items.m_panelUseItem.Show(item_serial.Value);



			if (panel_items.m_panelUseItem.master_param.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
			{
				Fsm.Event("use");
			}
			else if(panel_items.m_panelUseItem.master_param.item_id / MasterItem.LargeCategory == MasterItem.CategorySkin)
			{
				Fsm.Event("use");
			}
			else
			{
				Fsm.Event("equip");
			}
		}
	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class CloseItem : PanelItemsActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel_items.m_panelUseItem.gameObject.SetActive(false);
			Finish();
		}
	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class EquipCheck : PanelItemsActionBase
	{
		public FsmInt item_serial;

		public override void OnEnter()
		{
			base.OnEnter();

			panel_items.m_panelUseItem.Refresh();

			panel_items.m_panelUseItem.m_btnAct.onClick.AddListener(equip);
			panel_items.m_panelUseItem.m_btnDelete.onClick.AddListener(delete);
			panel_items.m_panelUseItem.m_btnClose.onClick.AddListener(cancel);
		}

		private void equip()
		{

			MasterItemParam shortcut_master = panel_items.m_panelUseItem.master_param;
			int iLargeCategory = shortcut_master.item_id / MasterItem.LargeCategory;
			BtnEquip btnEquip = GameMain.Instance.panelStatus.GetBtnEquipFromCategory(iLargeCategory);
			DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(btnEquip.equip_index), item_serial.Value);
			DataManager.Instance.user_data.Save();

			Fsm.Event("equip");
		}

		private void delete()
		{

			panel_items.m_panelUseItem.data_param.num = 0;

			DataManager.Instance.user_data.AddInt(Defines.KeyGem, panel_items.m_panelUseItem.data_param.craft_count);
			//Debug.Log(DataManager.Instance.dataItem.list.Count);
			DataManager.Instance.dataItem.list.Remove(panel_items.m_panelUseItem.data_param);
			//Debug.Log(DataManager.Instance.dataItem.list.Count);

			MasterItemParam shortcut_master = panel_items.m_panelUseItem.master_param;
			int iLargeCategory = shortcut_master.item_id / MasterItem.LargeCategory;
			BtnEquip btnEquip = GameMain.Instance.panelStatus.GetBtnEquipFromCategory(iLargeCategory);
			DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(btnEquip.equip_index), 0);

			DataManager.Instance.user_data.Save();
			DataManager.Instance.dataItem.Save();

			Fsm.Event("delete");

		}

		private void cancel()
		{
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();

			panel_items.m_panelUseItem.m_btnAct.onClick.RemoveListener(equip);
			panel_items.m_panelUseItem.m_btnDelete.onClick.RemoveListener(delete);
			panel_items.m_panelUseItem.m_btnClose.onClick.RemoveListener(cancel);
		}

	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class Equip : PanelItemsActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}

	}


	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class UseCheck : PanelItemsActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel_items.m_panelUseItem.Refresh();

			panel_items.m_panelUseItem.m_btnAct.onClick.AddListener(use);
			panel_items.m_panelUseItem.m_btnDelete.onClick.AddListener(delete);
			panel_items.m_panelUseItem.m_btnClose.onClick.AddListener(cancel);
		}

		private void use()
		{
			if (panel_items.m_panelUseItem.master_param.item_id / MasterItem.LargeCategory == MasterItem.CategorySkin)
			{

			}
			else {
				Fsm.Event("use");
			}

		}

		private void delete()
		{
			Fsm.Event("delete");
		}

		private void cancel()
		{
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_items.m_panelUseItem.m_btnAct.onClick.RemoveListener(use);
			panel_items.m_panelUseItem.m_btnDelete.onClick.RemoveListener(delete);
			panel_items.m_panelUseItem.m_btnClose.onClick.RemoveListener(cancel);
		}
	}



	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class use : PanelItemsActionBase
	{
		public FsmInt item_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);

			// ここの効果別の処理
			if( data.Use())
			{
				data.num -= 1;
				GameMain.Instance.ShortcutRefresh(data.serial);
			}

			Finish();
		}
	}

	[ActionCategory("PanelItemsAction")]
	[HutongGames.PlayMaker.Tooltip("PanelItemsAction")]
	public class delete : PanelItemsActionBase
	{
		public FsmInt item_serial;
		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			data.num -= 1;

			// 効果が発生する以外は使うと変わらないのね
			Finish();
		}

	}

}