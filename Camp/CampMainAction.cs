using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace CampMainAction {
	public class CampMainActionBase : FsmStateAction
	{
		protected CampMain campMain;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain = Owner.GetComponent<CampMain>();
		}
	}
	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class Startup : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Time.timeScale = 0;
			Finish();
		}

	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class ListClear : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			BannerDungeon[] arr = campMain.m_goListContent.GetComponentsInChildren<BannerDungeon>();
			foreach (BannerDungeon banner in arr)
			{
				GameObject.Destroy(banner.gameObject);
			}


			Finish();
		}

	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class Dungeon : CampMainActionBase
	{
		public FsmString dungeon_id;
		public override void OnEnter()
		{
			base.OnEnter();
			if (campMain.m_goListRoot.activeSelf == false)
			{
				campMain.m_goListRoot.SetActive(true);
				foreach (MasterDungeonParam master in DataManager.Instance.masterDungeon.list)
				{
					BannerDungeon script = PrefabManager.Instance.MakeScript<BannerDungeon>(campMain.m_prefBannerDungeon, campMain.m_goListContent);

					script.Initialize(master);
					script.OnEvent.AddListener(OnClickDungeonBanner);
				}
			}
		}
		private void OnClickDungeonBanner(string arg0)
		{
			dungeon_id.Value = arg0;
			Debug.Log("dungeonbanner:" + arg0);
			Fsm.Event("dungeon");
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class DungeonCheck : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_goCampCheck.SetActive(true);
			campMain.m_txtCheckTitle.text = "注意！";
			campMain.m_txtCheckDetail.text = "現在のダンジョン探索を中断しますがよろしいでしょうか？";
			campMain.m_btnYes.onClick.AddListener(OnYes);
			campMain.m_btnNo.onClick.AddListener(OnNo);
		}
		private void OnYes()
		{
			Fsm.Event("yes");
		}
		private void OnNo()
		{
			Fsm.Event("no");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_btnYes.onClick.RemoveListener(OnYes);
			campMain.m_btnNo.onClick.RemoveListener(OnNo);
			campMain.m_goCampCheck.SetActive(false);
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class DungeonChange : CampMainActionBase
	{
		public FsmString dungeon_id;

		public override void OnEnter()
		{
			base.OnEnter();

			DataManager.Instance.dataChara.hp = 0;
			DataManager.Instance.user_data.WriteString(Defines.KEY_DUNGEON_ID, dungeon_id.Value);
			DataManager.Instance.floor_restart = 1;

			Finish();
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}




	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class Close : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			DataManager.Instance.gameSpeedControl.Play();
			campMain.gameObject.SetActive(false);
			Finish();
		}

	}



}
