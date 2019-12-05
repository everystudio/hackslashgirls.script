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
		public override void OnEnter()
		{
			base.OnEnter();


			campMain.m_goListRoot.SetActive(true);

			foreach ( MasterDungeonParam master in DataManager.Instance.masterDungeon.list)
			{
				BannerDungeon script = PrefabManager.Instance.MakeScript<BannerDungeon>(campMain.m_prefBannerDungeon, campMain.m_goListContent);

				script.Initialize(master);
				script.OnEvent.AddListener(OnClickDungeonBanner);
			}



		}

		private void OnClickDungeonBanner(string arg0)
		{
			Debug.Log("dungeonbanner:" + arg0);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
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
