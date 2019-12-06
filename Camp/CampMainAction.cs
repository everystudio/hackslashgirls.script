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

			BannerMedalPrize[] arr2= campMain.m_goListContent.GetComponentsInChildren<BannerMedalPrize>();
			foreach (BannerMedalPrize banner in arr2)
			{
				GameObject.Destroy(banner.gameObject);
			}

			BannerSkin[] arr3 = campMain.m_goListContent.GetComponentsInChildren<BannerSkin>();
			foreach (BannerSkin banner in arr3)
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
	public class MedalPrizeList : CampMainActionBase
	{
		public FsmInt medal_prize_id;
		public override void OnEnter()
		{
			base.OnEnter();
			if (campMain.m_goListRoot.activeSelf == false)
			{
				campMain.m_goListRoot.SetActive(true);
				foreach (MasterMedalPrizeParam master in DataManager.Instance.masterMedalPrize.list)
				{
					BannerMedalPrize script = PrefabManager.Instance.MakeScript<BannerMedalPrize>(campMain.m_prefBannerMedal, campMain.m_goListContent);

					script.Initialize(master);
					script.OnMedalPrizeId.AddListener(OnClickBanner);
				}
			}
		}

		private void OnClickBanner(int arg0)
		{
			medal_prize_id.Value = arg0;
			Fsm.Event("prize");
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class PrizeCheck : CampMainActionBase
	{
		public FsmInt medal_prize_id;
		public override void OnEnter()
		{
			base.OnEnter();
			campMain.m_medalPrizeBuyCheck.gameObject.SetActive(true);
			MasterMedalPrizeParam prize = DataManager.Instance.masterMedalPrize.list.Find(p => p.medal_prize_id == medal_prize_id.Value);
			campMain.m_medalPrizeBuyCheck.Initialize(prize);
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_medalPrizeBuyCheck.gameObject.SetActive(false);
		}
	}



	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class PrizeBuy : CampMainActionBase
	{
		public FsmInt medal_prize_id;
		public override void OnEnter()
		{
			base.OnEnter();

			MasterMedalPrizeParam prize = DataManager.Instance.masterMedalPrize.list.Find(p => p.medal_prize_id == medal_prize_id.Value);

			DataItemParam medal_param = DataManager.Instance.dataItem.list.Find(p => p.item_id == prize.item_id_medal);

			medal_param.num -= prize.medal_num;

			DataManager.Instance.dataItem.AddItem(prize.prize_item_id, 1);

			DataManager.Instance.dataItem.Save();

			campMain.m_goListRoot.SetActive(false);
			Finish();
		}
	}


	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class SkinList : CampMainActionBase
	{
		public FsmInt skin_id;
		public override void OnEnter()
		{
			base.OnEnter();
			if (campMain.m_goListRoot.activeSelf == false)
			{
				campMain.m_goListRoot.SetActive(true);
				foreach (MasterSkinParam master in DataManager.Instance.masterSkin.list)
				{
					BannerSkin script = PrefabManager.Instance.MakeScript<BannerSkin>(campMain.m_prefBannerSkin, campMain.m_goListContent);

					script.Initialize(master);
					script.OnClickSkin.AddListener(OnClickBanner);
				}
			}
		}

		private void OnClickBanner(int arg0)
		{
			skin_id.Value = arg0;
			Fsm.Event("skin");
		}
	}

	[ActionCategory("CampMainAction")]
	[HutongGames.PlayMaker.Tooltip("CampMainAction")]
	public class SkinCheck : CampMainActionBase
	{
		public FsmInt skin_id;
		private MasterSkinParam m_masterSkinParam;
		public override void OnEnter()
		{
			base.OnEnter();

			m_masterSkinParam = DataManager.Instance.masterSkin.list.Find(p => p.skin_id == skin_id.Value);
			MasterItemParam master_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == m_masterSkinParam.item_id);

			campMain.m_txtSkinName.text = m_masterSkinParam.skin_name;
			campMain.m_imgSkinIcon.sprite = SpriteManager.Instance.Get(master_item.sprite_name);

			campMain.m_btnSkinYes.onClick.AddListener(OnYes);
			campMain.m_btnSkinNo.onClick.AddListener(OnNo);

			campMain.m_goSkinCheck.SetActive(true);

		}

		private void OnYes()
		{
			GameMain.Instance.ChangeCharaTexture(m_masterSkinParam.texture_name);
			Fsm.Event("yes");
		}

		private void OnNo()
		{
			Fsm.Event("no");
		}

		public override void OnExit()
		{
			base.OnExit();
			campMain.m_btnSkinYes.onClick.RemoveListener(OnYes);
			campMain.m_btnSkinNo.onClick.RemoveListener(OnNo);

			campMain.m_goSkinCheck.SetActive(false);

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
