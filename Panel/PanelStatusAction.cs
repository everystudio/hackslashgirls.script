using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace PanelStatusAction {
	public class PanelStatusActionBase : FsmStateAction
	{
		protected PanelStatus panel_status;
		public override void OnEnter()
		{
			base.OnEnter();
			panel_status = Owner.GetComponent<PanelStatus>();
		}
		protected void ChangeShortcut(int _iIndex, int _iItemSerial)
		{
			//Debug.Log(string.Format("setindex:{0} setserial:{1}", _iIndex, _iItemSerial));
			int iIndex = 0;
			if ( _iItemSerial == 0)
			{
				// 差し替え処理は不要
			}
			else if (panel_status.m_panelShortcuts.CheckSerial(_iItemSerial, ref iIndex))
			{
				//Debug.Log("has index");
				//Debug.Log(iIndex);
				int iTempSerial = DataManager.Instance.user_data.ReadInt(DataManager.Instance.GetShortcutKey(_iIndex));
				DataManager.Instance.user_data.WriteInt(DataManager.Instance.GetShortcutKey(iIndex), iTempSerial);
			}
			//SEControl.Instance.Play("set_shortcut");

			DataManager.Instance.user_data.WriteInt(DataManager.Instance.GetShortcutKey(_iIndex), _iItemSerial);
			DataManager.Instance.user_data.Save();
		}


	}
	[ActionCategory("PanelStatusAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStatusAction")]
	public class Startup : PanelStatusActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (panel_status.Initialized)
			{
				Finish();
			}
		}
	}
	[ActionCategory("PanelStatusAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStatusAction")]
	public class Idle : PanelStatusActionBase
	{
		public FsmInt equip_index;
		public FsmInt equip_category;

		public FsmInt shortcut_serial;
		public FsmInt shortcut_index;

		public override void OnEnter()
		{
			base.OnEnter();

			panel_status.OnCurrentEquip.AddListener(OnEquip);
			panel_status.m_panelEquip.gameObject.SetActive(false);
			panel_status.Refresh();

			panel_status.m_panelShortcuts.OnShortcut.AddListener(OnShortcut);
			panel_status.m_panelShortcuts.OnLongTap.AddListener(OnLongTap);

			panel_status.m_panelShortcuts.Refresh();
		}

		private void OnShortcut(BtnShortcut arg0)
		{
			shortcut_index.Value = arg0.btn_index;
			shortcut_serial.Value = arg0.item_serial;

			//Debug.Log(shortcut_index.Value);
			//Debug.Log(shortcut_serial.Value);

			if (arg0.item_serial != 0)
			{
				Fsm.Event("shortcut");
			}
			else
			{
				Fsm.Event("shortcut_set");
			}
		}

		private void OnLongTap(BtnShortcut arg0)
		{
			shortcut_index.Value = arg0.btn_index;
			shortcut_serial.Value = arg0.item_serial;
			//Debug.Log(arg0.btn_index);
			if (arg0.item_serial != 0)
			{
				Fsm.Event("shortcut_detail");
			}
			else
			{
				Fsm.Event("shortcut_set");
			}
		}

		private void OnEquip(int _iIndex, int _iCategoryLarge)
		{
			equip_index.Value = _iIndex;
			//Debug.Log(_iIndex);
			equip_category.Value = _iCategoryLarge;
			Fsm.Event("equip");
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_status.OnCurrentEquip.RemoveListener(OnEquip);

			panel_status.m_panelShortcuts.OnShortcut.RemoveListener(OnShortcut);
			panel_status.m_panelShortcuts.OnLongTap.RemoveListener(OnLongTap);

		}
	}
	[ActionCategory("PanelStatusAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStatusAction")]
	public class Equip : PanelStatusActionBase
	{
		public FsmInt equip_index;
		public FsmInt equip_category;

		public override void OnEnter()
		{
			base.OnEnter();
			//Debug.Log(equip_index.Value);
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_SHORTCUT);
			//Debug.Log(equip_category.Value);
			panel_status.m_panelEquip.gameObject.SetActive(true);
			StartCoroutine( panel_status.m_panelEquip.Initialize(equip_index.Value, equip_category.Value));

			panel_status.m_panelEquip.OnClickBanner.AddListener(OnEquip);
			panel_status.m_panelEquip.OnClose.AddListener(OnClose);
		}

		private void OnEquip(DataItemParam arg0)
		{
			int iSetSerial = 0;
			if (arg0 != null)
			{
				iSetSerial = arg0.serial;
			}
			DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(equip_index.Value), iSetSerial);

			//Debug.Log(MasterItem.GetEquipIndexName(equip_index.Value));
			DataManager.Instance.user_data.Save();
			Fsm.Event("equip");
		}

		private void OnClose()
		{
			Fsm.Event("close");
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_status.m_panelEquip.OnClickBanner.RemoveListener(OnEquip);
			panel_status.m_panelEquip.OnClose.RemoveListener(OnClose);

		}
	}
	[ActionCategory("PanelStatusAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStatusAction")]
	public class Shortcut : PanelStatusActionBase
	{
		public FsmInt shortcut_serial;
		public FsmInt shortcut_index;

		private DataItemParam shortcut_data;
		private MasterItemParam shortcut_master;

		private BtnEquip btnEquip;


		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_SHORTCUT);

			shortcut_data = DataManager.Instance.dataItem.list.Find(p => p.serial == shortcut_serial.Value);
			shortcut_master = DataManager.Instance.masterItem.list.Find(p => p.item_id == shortcut_data.item_id);

			int iLargeCategory = shortcut_master.item_id / MasterItem.LargeCategory;

			if ( iLargeCategory == MasterItem.CategoryConsumable)
			{
				if (0 < shortcut_data.num)
				{
					if( shortcut_data.Use())
					{
						shortcut_data.num -= 1;
					}
				}
				Finish();
			}
			else if( iLargeCategory == MasterItem.CategoryMagic)
			{
				if( 0 < shortcut_data.num)
				{
					if (shortcut_data.Use())
					{
						shortcut_data.num -= 1;
					}
				}
				Finish();
			}
			else if (iLargeCategory == MasterItem.CategoryAccessary)
			{
				Fsm.Event("shortcut_set");
			}
			else
			{
				EquipChange(iLargeCategory);
			}
		}

		private void EquipChange( int _iLargeCategory)
		{
			// まずはボタンを探す
			btnEquip = panel_status.GetBtnEquipFromCategory(_iLargeCategory);

			if( btnEquip.m_equipData == null)
			{
				DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(btnEquip.equip_index), shortcut_serial.Value);
				DataManager.Instance.user_data.Save();
				Fsm.Event("equip");
			}
			else
			{
				int iEquipSerial = btnEquip.m_equipData.serial;
				if( iEquipSerial == shortcut_serial.Value)
				{
					DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(btnEquip.equip_index), 0);
				}
				else
				{
					// ステータスの装備表示変更
					DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(btnEquip.equip_index), shortcut_serial.Value);

					// ショートカットボタンの変更
					//DataManager.Instance.user_data.WriteInt(MasterItem.GetEquipIndexName(equip_index));
					int iIndex = 0;
					if (panel_status.m_panelShortcuts.CheckSerial(iEquipSerial, ref iIndex)== false )
					{
						Debug.Log("no shortcut");
						ChangeShortcut(shortcut_index.Value, iEquipSerial);
					}
				}
				DataManager.Instance.user_data.Save();
				Fsm.Event("equip");

			}

		}

	}

	[ActionCategory("PanelStatusAction")]
	[HutongGames.PlayMaker.Tooltip("PanelStatusAction")]
	public class ShortcutSet : PanelStatusActionBase
	{
		public FsmInt shortcut_serial;
		public FsmInt shortcut_index;

		private int show_category;

		public override void OnEnter()
		{
			base.OnEnter();
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_SHORTCUT);

			panel_status.m_panelCategory_shortcutset.HandleCategoryButton.AddListener(OnChangeCategory);

			show_category = 0;

			//Debug.Log(shortcut_index.Value);
			panel_status.m_panelShortcutSet.gameObject.SetActive(true);
			StartCoroutine( panel_status.m_panelShortcutSet.Show(show_category));

			panel_status.m_panelShortcutSet.m_btnClose.onClick.AddListener(OnClose);

			panel_status.m_panelShortcutSet.OnClickBanner.AddListener(OnClickBanner);
		}

		private void OnChangeCategory(int arg0)
		{
			if( show_category != arg0)
			{
				show_category = arg0;
				StartCoroutine(panel_status.m_panelShortcutSet.Show(show_category));
			}

		}

		private void OnClickBanner(int arg0)
		{
			/*
			// セットするか？
			// セットしたいアイテムが、すでにセットされてないか確認する
			int iIndex = 0;
			if( panel_status.m_panelShortcuts.CheckSerial(arg0 , ref iIndex))
			{
				int iTempSerial = DataManager.Instance.user_data.ReadInt(DataManager.Instance.GetShortcutKey(shortcut_index.Value));

				//Debug.LogError(iIndex);
				//Debug.LogError(iTempSerial);
				DataManager.Instance.user_data.WriteInt(DataManager.Instance.GetShortcutKey(iIndex), iTempSerial);
			}
			//Debug.Log(shortcut_index.Value);
			//Debug.Log(DataManager.Instance.GetShortcutKey(shortcut_index.Value) +":"+ arg0.ToString());
			DataManager.Instance.user_data.WriteInt(DataManager.Instance.GetShortcutKey(shortcut_index.Value), arg0);

			DataManager.Instance.user_data.Save();

			*/

			ChangeShortcut(shortcut_index.Value, arg0);
			Fsm.Event("change");
			//OnClose();
		}

		private void OnClose()
		{
			Fsm.Event("close");
		}


		public override void OnExit()
		{
			base.OnExit();
			panel_status.m_panelCategory_shortcutset.HandleCategoryButton.RemoveListener(OnChangeCategory);

			panel_status.m_panelShortcutSet.OnClickBanner.RemoveListener(OnClickBanner);
			panel_status.m_panelShortcutSet.m_btnClose.onClick.RemoveListener(OnClose);
			panel_status.m_panelShortcutSet.gameObject.SetActive(false);
		}
	}
}
