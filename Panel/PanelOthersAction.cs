using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PanelOthersAction  {
	public class PanelOthersActionBase : FsmStateAction
	{
		protected PanelOthers panelOthers;
		public override void OnEnter()
		{
			base.OnEnter();
			panelOthers = Owner.GetComponent<PanelOthers>();
		}
	}
	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class Startup : PanelOthersActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}

	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class Idle : PanelOthersActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			bool bstop_100 = false;
			if (DataManager.Instance.user_data.HasKey(Defines.KEY_STOP_100))
			{
				bstop_100 = (1 == DataManager.Instance.user_data.ReadInt(Defines.KEY_STOP_100));
			}
			panelOthers.toggle_stop_100.isOn = bstop_100;


			bool bnot_sleep = false;
			if (DataManager.Instance.user_data.HasKey(Defines.KEY_NOT_SLEEP))
			{
				bnot_sleep = (1 == DataManager.Instance.user_data.ReadInt(Defines.KEY_NOT_SLEEP));
			}
			panelOthers.toggle_not_sleep.isOn = bnot_sleep;

			panelOthers.m_btnRestart.onClick.AddListener(OnRestart);
			panelOthers.m_btnPrev10.onClick.AddListener(OnPref10);
			panelOthers.m_btnPrev1.onClick.AddListener(OnPref1);
			panelOthers.m_btnNext1.onClick.AddListener(OnNext1);
			panelOthers.m_btnNext10.onClick.AddListener(OnNext10);
			panelOthers.m_btnGuide.onClick.AddListener(OnGuide);
			panelOthers.m_btnSoundControl.onClick.AddListener(OnSoundControl);

			panelOthers.SetRestartFloor(DataManager.Instance.floor_restart);
		}

		private void OnRestart()
		{
			//０代入はここだけのはず？
			DataManager.Instance.dataChara.hp = 0;
		}

		private void OnPref10()
		{
			SEControl.Instance.Play("cursor_01");
			if( 10 < DataManager.Instance.floor_restart)
			{
				DataManager.Instance.floor_restart -= 10;
			}
			else
			{
				DataManager.Instance.floor_restart = 1;
			}
			panelOthers.SetRestartFloor(DataManager.Instance.floor_restart);
		}

		private void OnPref1()
		{
			SEControl.Instance.Play("cursor_01");
			if (1 < DataManager.Instance.floor_restart)
			{
				DataManager.Instance.floor_restart -= 1;
			}
			else
			{
				DataManager.Instance.floor_restart = 1;	// まぁ1以外なさそうだけど
			}
			panelOthers.SetRestartFloor(DataManager.Instance.floor_restart);
		}

		private void OnNext1()
		{
			SEControl.Instance.Play("cursor_01");
			if (DataManager.Instance.floor_best < DataManager.Instance.floor_restart + 1 )
			{
				;// エラー
			}
			else
			{
				DataManager.Instance.floor_restart += 1;
			}
			panelOthers.SetRestartFloor(DataManager.Instance.floor_restart);
		}

		private void OnNext10()
		{
			SEControl.Instance.Play("cursor_01");
			if (DataManager.Instance.floor_best < DataManager.Instance.floor_restart + 10)
			{
				DataManager.Instance.floor_restart = DataManager.Instance.floor_best;
			}
			else
			{
				DataManager.Instance.floor_restart += 10;
			}
			panelOthers.SetRestartFloor(DataManager.Instance.floor_restart);
		}

		private void OnGuide()
		{
			Fsm.Event("guide");
		}
		private void OnSoundControl()
		{
			Fsm.Event("sound");
		}

		public override void OnExit()
		{
			base.OnExit();
			panelOthers.m_btnRestart.onClick.RemoveListener(OnRestart);
			panelOthers.m_btnPrev10.onClick.RemoveListener(OnPref10);
			panelOthers.m_btnPrev1.onClick.RemoveListener(OnPref1);
			panelOthers.m_btnNext1.onClick.RemoveListener(OnNext1);
			panelOthers.m_btnNext10.onClick.RemoveListener(OnNext10);
			panelOthers.m_btnGuide.onClick.RemoveListener(OnGuide);
			panelOthers.m_btnSoundControl.onClick.RemoveListener(OnSoundControl);
		}
	}
	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class RemoveCheck : PanelOthersActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			panelOthers.panel_bulk_remove.gameObject.SetActive(true);
			panelOthers.panel_bulk_remove.Initialize();

			panelOthers.panel_bulk_remove.m_btnCancel.onClick.AddListener(OnCancel);
			panelOthers.panel_bulk_remove.m_btnRemove.onClick.AddListener(OnRemove);

		}

		private void OnRemove()
		{
			Fsm.Event("item_remove");
		}

		private void OnCancel()
		{
			Fsm.Event("cancel");
		}

		public override void OnExit()
		{
			base.OnExit();
			panelOthers.panel_bulk_remove.gameObject.SetActive(false);
			panelOthers.panel_bulk_remove.m_btnCancel.onClick.RemoveListener(OnCancel);
			panelOthers.panel_bulk_remove.m_btnRemove.onClick.RemoveListener(OnRemove);
		}
	}


	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class RemoveItems : PanelOthersActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			foreach(DataItemParam data in panelOthers.panel_bulk_remove.remove_data_list)
			{
				// これよくないよ
				for( int i = 0;  i < 5; i++)
				{
					string strKey = MasterItem.GetEquipIndexName(i);
					if( DataManager.Instance.user_data.ReadInt(strKey) == data.serial)
					{
						DataManager.Instance.user_data.WriteInt(strKey, 0);
					}
				}
				DataManager.Instance.dataItem.list.Remove(data);
			}
			Finish();
		}

	}


	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class SetSpeedMeter : PanelOthersActionBase
	{
		public FsmInt speed_meter;
		public override void OnEnter()
		{
			base.OnEnter();
			SEControl.Instance.Play("cursor_01");
			DataManager.Instance.SetSpeedMeter(speed_meter.Value);
			Finish();
		}

	}


	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class Guide : PanelOthersActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			panelOthers.m_goGuide.SetActive(true);
			panelOthers.m_btnGuideClose.onClick.AddListener(OnClose);
		}
		private void OnClose()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			panelOthers.m_goGuide.SetActive(false);
			panelOthers.m_btnGuideClose.onClick.RemoveListener(OnClose);

		}

	}

	[ActionCategory("PanelOthersAction")]
	[HutongGames.PlayMaker.Tooltip("PanelOthersAction")]
	public class Sound : PanelOthersActionBase
	{
		public FsmGameObject panel_sound;
		private bool bCancel;

		private float fBGM;
		private float fSE;

		public override void OnEnter()
		{
			base.OnEnter();
			panel_sound.Value.SetActive(true);
			bCancel = true;

			fBGM = DataManager.Instance.user_data.ReadFloat(Defines.KEY_SOUND_BGM);
			fSE = DataManager.Instance.user_data.ReadFloat(Defines.KEY_SOUND_SE);

			panelOthers.m_btnSoundControlDecide.onClick.AddListener(OnDecide);
			panelOthers.m_btnSoundControlCancel.onClick.AddListener(OnCancel);
		}

		private void OnDecide()
		{
			bCancel = false;
			Fsm.Event("decide");
		}

		private void OnCancel()
		{
			bCancel = true;
			Fsm.Event("cancel");

		}
		public override void OnExit()
		{
			base.OnExit();
			panel_sound.Value.SetActive(false);
			if (bCancel)
			{
				DataManager.Instance.user_data.Write(Defines.KEY_SOUND_BGM, fBGM.ToString());
				DataManager.Instance.user_data.Write(Defines.KEY_SOUND_SE, fSE.ToString());
				panelOthers.mixer.SetFloat(Defines.KEY_SOUND_BGM, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, fBGM));
				panelOthers.mixer.SetFloat(Defines.KEY_SOUND_SE, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, fSE));
			}
			panelOthers.m_btnSoundControlDecide.onClick.RemoveListener(OnDecide);
			panelOthers.m_btnSoundControlCancel.onClick.RemoveListener(OnCancel);
		}
	}


}
