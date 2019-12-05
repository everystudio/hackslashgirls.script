using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public class Dungeon : CampMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();


			campMain.m_goListRoot.SetActive(true);

			foreach ( MasterDungeonParam master in DataManager.Instance.masterDungeon.list)
			{
				PrefabManager.Instance.MakeScript<Button>(campMain.m_prefBannerDungeon, campMain.m_goListContent);
			}



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
