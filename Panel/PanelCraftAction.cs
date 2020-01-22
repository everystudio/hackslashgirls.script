using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace PanelCraftAction {
	public class PanelCraftActionBase : FsmStateAction
	{
		protected PanelCraft panel;
		public override void OnEnter()
		{
			base.OnEnter();
			panel = Owner.GetComponent<PanelCraft>();
		}
	}
	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class Startup : PanelCraftActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_CRAFT);

			panel.m_txtElement.text = "素材";
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (panel.Initialized)
			{
				if (DataManager.Instance.user_data.HasKey(Defines.CRAFT_BULK_COUNT))
				{
					panel. m_iCraftBulkCount = DataManager.Instance.user_data.ReadInt(Defines.CRAFT_BULK_COUNT);
				}
				else
				{
					panel. m_iCraftBulkCount = 1;
				}


				Finish();
			}
		}
	}

	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class base_select : PanelCraftActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel.Startup();
			panel.m_btnBase.onClick.AddListener(OnClickBase);

			panel.m_btnExe.interactable = false;
			panel.m_btnReset.interactable = false;
			panel.m_btnCancel.onClick.AddListener(OnClose);
			panel.m_textBikou.text = "";

			panel.m_btnBulk.onClick.AddListener(OnBulk);
		}

		private void OnBulk()
		{
			Fsm.Event("bulk");
		}

		private void OnClose()
		{
			// イベント意味ない
			//Debug.Log("call");
			panel.m_btnFooterStatus.onClick.Invoke();
		}

		private void OnClickBase()
		{
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
			panel.m_btnBase.onClick.RemoveListener(OnClickBase);
			panel.m_btnCancel.onClick.RemoveListener(OnClose);
			panel.m_btnBulk.onClick.RemoveListener(OnBulk);

		}
	}


	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class base_list : PanelCraftActionBase
	{
		public FsmInt category;

		public FsmInt item_serial;

		public FsmInt element_item_id;

		public override void OnEnter()
		{
			base.OnEnter();

			//item_serial.Value = 0;

			StartCoroutine(panel.ShowList(category.Value));
			panel.HandleClickBanner.AddListener(OnClickBanner);
			panel.m_btnBaseCancel.onClick.AddListener(OnCancel);
			panel.m_panelCategory.HandleCategoryButton.AddListener(OnCategory);
		}

		private void OnCategory(int arg0)
		{
			if (category.Value != arg0)
			{
				category.Value = arg0;
				Fsm.Event("category");
			}
		}

		private void OnCancel()
		{
			Fsm.Event("cancel");
		}

		private void OnClickBanner(DataItemParam arg0)
		{
			//Debug.Log(arg0);
			item_serial.Value = arg0.serial;

			DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			MasterItemParam master_now = DataManager.Instance.masterItem.list.Find(p => p.item_id == data.item_id);
			//int item_category = master_now.item_id / MasterItem.LargeCategory;

			// これは良くないですね
			element_item_id.Value = master_now.craft_item_id;
			
			Fsm.Event("select");

		}

		public override void OnExit()
		{
			base.OnExit();
			panel.HandleClickBanner.RemoveListener(OnClickBanner);
			panel.m_btnBaseCancel.onClick.RemoveListener(OnCancel);
			panel.m_panelCategory.HandleCategoryButton.RemoveListener(OnCategory);

			panel.CloseList();
		}
	}

	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class craft_check : PanelCraftActionBase
	{
		public FsmInt item_serial;
		public FsmInt element_item_id;

		private int element_num;

		private bool can_craft;

		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);
			MasterItemParam master_now = DataManager.Instance.masterItem.list.Find(p => p.item_id == data.item_id);
			MasterItemParam master_next = DataManager.Instance.masterItem.list.Find(p => p.item_id == master_now.next_item_id);

			panel.m_btnBulk.onClick.AddListener(OnBulk);

			int iAddCraftCount = panel.m_iCraftBulkCount;

			panel.m_txtElement.text = string.Format("強化1回あたり<color=#FF0>{0}ジェム</color>必要", master_now.craft_gem_num);

			string now_item_name = master_now.GetItemName(data.craft_count);


			int category = data.item_id / MasterItem.LargeCategory;
			string bikou = "";
			switch( category)
			{
				case MasterItem.CategoryWeapon:
					bikou = "：Atkが上がります";
					break;
				case MasterItem.CategoryArmor:
					bikou = "：Defが上がります";
					break;
				case MasterItem.CategoryBracelet:
					bikou = "：Staminaの最大値が上がります";
					break;
				case MasterItem.CategoryCloak:
					bikou = "：最大HPが上がります";
					break;
				case MasterItem.CategoryHelmet:
					bikou = "：Magが上がります(魔法攻撃力アップ)";
					break;
				default:
					break;
			}
			panel.m_textBikou.text = bikou;

			string next_item_name = "";
			if( master_now.limit < data.craft_count + iAddCraftCount)
			{
				next_item_name = master_now.name;

				if( master_next != null)
				{
					next_item_name = master_next.name;
				}
				else
				{
					next_item_name = "強化限界に到達します";
				}
			}
			else
			{
				next_item_name = master_now.GetItemName(data.craft_count + iAddCraftCount);
			}

			DataItemParam data_element = DataManager.Instance.dataItem.list.Find(p => p.item_id == element_item_id.Value);
			if (data_element != null)
			{
				element_num = data_element.num;
			}
			else
			{
				element_num = 0;
			}

			MasterItemParam master_element = DataManager.Instance.masterItem.list.Find(p => p.item_id == element_item_id.Value);

			string strElement = string.Format("{0}[{1}]", master_element.name, element_num);

			if (element_num < iAddCraftCount)
			{
				strElement = string.Format("<color=red>{0}</color>", strElement);
			}
			panel.LabelUpdate(now_item_name, strElement, next_item_name);

			panel.m_btnBase.onClick.AddListener(OnClickBase);
			panel.m_btnExe.interactable = true;

			can_craft = true;

			int iRequestGem = master_now.craft_gem_num * panel.m_iCraftBulkCount;

			panel.m_textCraftCount.text = string.Format("x{0}", iRequestGem);



			if (DataManager.Instance.user_data.ReadInt(Defines.KeyGem) < iRequestGem)
			{
				can_craft = false;
			}
			else if( element_num < iAddCraftCount)
			{
				can_craft = false;
			}

			if (can_craft)
			{
				panel.m_btnExe.gameObject.GetComponent<Image>().color = Color.white;
			}
			else {
				panel.m_btnExe.gameObject.GetComponent<Image>().color = Color.gray;
			}
			panel.m_btnExe.onClick.AddListener(OnExe);

			panel.m_btnReset.interactable = true;
			panel.m_btnReset.onClick.AddListener(OnReset);
			panel.m_btnCancel.onClick.AddListener(OnClose);

		}

		private void OnBulk()
		{
			Fsm.Event("bulk");
		}

		private void OnExe()
		{
			if (can_craft)
			{
				Fsm.Event("craft");
			}
			else
			{

			}
		}

		private void OnReset()
		{
			Fsm.Event("reset");
		}

		private void OnClose()
		{
			// イベント意味ない
			//Debug.Log("call");
			panel.m_btnFooterStatus.onClick.Invoke();
		}

		private void OnClickBase()
		{
			Fsm.Event("base_change");
		}

		public override void OnExit()
		{
			base.OnExit();
			panel.m_btnExe.gameObject.GetComponent<Image>().color = Color.white;
			panel.m_btnBase.onClick.RemoveListener(OnClickBase);

			panel.m_btnBulk.onClick.RemoveListener(OnBulk);

			panel.m_btnExe.onClick.RemoveListener(OnExe);
			panel.m_btnReset.onClick.RemoveListener(OnReset);
			panel.m_btnCancel.onClick.RemoveListener(OnClose);


		}
	}

	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class craft : PanelCraftActionBase
	{
		public FsmInt item_serial;
		public FsmInt element_item_id;

		public override void OnEnter()
		{
			base.OnEnter();

			DataItemParam data = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial.Value);

			MasterItemParam master_now = DataManager.Instance.masterItem.list.Find(p => p.item_id == data.item_id);
			MasterItemParam master_next = DataManager.Instance.masterItem.list.Find(p => p.item_id == master_now.next_item_id);

			int iAddCraftCount = panel.m_iCraftBulkCount;

			if(master_now.limit < data.craft_count + iAddCraftCount)
			{
				iAddCraftCount = master_now.limit - data.craft_count;
			}

			if ( data.craft_count < master_now.limit)
			{
				data.craft_count += iAddCraftCount;
			}

			int iRequestGem = master_now.craft_gem_num * iAddCraftCount;


			string strResult = "success";

			if( master_now.limit <= data.craft_count && master_next != null)
			{
				data.item_id = master_next.item_id;
				data.craft_count = 0;
			}
			else if( master_now.limit <= data.craft_count && master_next == null)
			{
				strResult = "failed";
			}


			if (strResult != "failed")
			{
				DataItemParam data_element = DataManager.Instance.dataItem.list.Find(p => p.item_id == element_item_id.Value);
				data_element.num -= iAddCraftCount;

				DataManager.Instance.user_data.AddInt(Defines.KeyGem, -1 * iRequestGem);

				DataManager.Instance.dataItem.Save();
				DataManager.Instance.user_data.Save();

				DataManager.Instance.dataChara.RefreshEquip();
			}

			Fsm.Event(strResult);


		}
	}



	[ActionCategory("PanelCraftAction")]
	[HutongGames.PlayMaker.Tooltip("PanelCraftAction")]
	public class change_bulk_count : PanelCraftActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panel.panelCraftBuldSetting.btnCancel.onClick.AddListener(OnCancel);
			panel.panelCraftBuldSetting.btnOne.onClick.AddListener(OnOne);
			panel.panelCraftBuldSetting.btnTen.onClick.AddListener(OnTen);
			panel.panelCraftBuldSetting.btnHundred.onClick.AddListener(OnHundred);

		}

		private void OnCancel()
		{
			Finish();
		}

		private void OnOne()
		{
			panel.m_iCraftBulkCount = 1;
			Finish();
		}

		private void OnTen()
		{
			panel.m_iCraftBulkCount = 10;
			Finish();
		}

		private void OnHundred()
		{
			panel.m_iCraftBulkCount = 100;
			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();

			if(panel.panelCraftBuldSetting.is_save)
			{
				DataManager.Instance.user_data.WriteInt(Defines.CRAFT_BULK_COUNT, panel.m_iCraftBulkCount);
			}
			else
			{
				if(DataManager.Instance.user_data.HasKey(Defines.CRAFT_BULK_COUNT))
				{
					DataManager.Instance.user_data.Remove(Defines.CRAFT_BULK_COUNT);
				}
			}

			Debug.Log(panel.m_iCraftBulkCount);
			panel.m_textCraftCount.text = string.Format("x{0}", panel.m_iCraftBulkCount);

			Debug.Log(panel.m_textCraftCount.text);

			panel.panelCraftBuldSetting.btnCancel.onClick.RemoveListener(OnCancel);
			panel.panelCraftBuldSetting.btnOne.onClick.RemoveListener(OnOne);
			panel.panelCraftBuldSetting.btnTen.onClick.RemoveListener(OnTen);
			panel.panelCraftBuldSetting.btnHundred.onClick.RemoveListener(OnHundred);

		}
	}



}







