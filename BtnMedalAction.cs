using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

namespace BtnMedalAction
{
	public class BtnMedalActionBase : FsmStateAction
	{
		protected BtnMedal btnMedal;
		public override void OnEnter()
		{
			base.OnEnter();
			btnMedal = Owner.GetComponent<BtnMedal>();
		}
	}
	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class startup : BtnMedalActionBase
	{

		public override void OnUpdate()
		{
			base.OnUpdate();
			if(DataManager.Instance.Initialized)
			{
				Finish();
			}
		}

	}


	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class wait : BtnMedalActionBase
	{
		private DateTime check_datetime;
		public override void OnEnter()
		{
			base.OnEnter();
			btnMedal.m_panelMedalCheck.gameObject.SetActive(false);


			btnMedal.m_btn.interactable = false;

			if (!DataManager.Instance.user_data.HasKey(Defines.KEY_LAST_REWARD_TIME))
			{
				check_datetime = new DateTime(2020, 1, 1);
			}
			else {
				check_datetime = System.DateTime.Parse(DataManager.Instance.user_data.Read(Defines.KEY_LAST_REWARD_TIME));
			}
			btnMedal.m_goAtoBase.SetActive(true);
			btnMedal.m_txtLimitTime.gameObject.SetActive(true);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			TimeSpan ts = NTPTimer.Instance.now - check_datetime;

			int last_second = (60 * 5) - (int)(ts.TotalSeconds);

			//Debug.Log(string.Format("sec:{0} ts:{1}", ts.Seconds, ts.TotalSeconds));

			if (0 < last_second)
			{
				btnMedal.m_txtLimitTime.text = string.Format("あと{0}:{1:00}", last_second / 60, last_second % 60);
			}
			else
			{
				Finish();
			}
		}
		public override void OnExit()
		{
			base.OnExit();

			btnMedal.m_goAtoBase.SetActive(false);
			btnMedal.m_txtLimitTime.gameObject.SetActive(false);
		}
	}

	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class reward_load : BtnMedalActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			RewardAd.Instance.rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
			RewardAd.Instance.RequestRewardBasedVideo();

#if UNITY_EDITOR
			Finish();
#endif
		}

		private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
		{
			Finish();
		}


		public override void OnExit()
		{
			base.OnExit();
			RewardAd.Instance.rewardBasedVideo.OnAdLoaded -= HandleRewardBasedVideoLoaded;
		}
	}

	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class reward_standby : BtnMedalActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			btnMedal.m_btn.interactable = true;
			btnMedal.m_btn.onClick.RemoveAllListeners();
			btnMedal.m_btn.onClick.AddListener(() =>
			{
				Finish();
			});

		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}

	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class reward_check : BtnMedalActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			btnMedal.m_panelMedalCheck.gameObject.SetActive(true);
			btnMedal.m_panelMedalCheck.Show(btnMedal.medal_item_id);
			btnMedal.m_panelMedalCheck.m_btnUse.onClick.AddListener(() =>
			{
				Fsm.Event("use");
			});
			btnMedal.m_panelMedalCheck.m_btnCancel.onClick.AddListener(() =>
			{
				Fsm.Event("cancel");
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			btnMedal.m_panelMedalCheck.m_btnUse.onClick.RemoveAllListeners();
			btnMedal.m_panelMedalCheck.m_btnCancel.onClick.RemoveAllListeners();
			btnMedal.m_panelMedalCheck.gameObject.SetActive(false);
		}
	}




	[ActionCategory("BtnMedalAction")]
	[HutongGames.PlayMaker.Tooltip("BtnMedalAction")]
	public class reward_play : BtnMedalActionBase
	{
		private bool rewarded;
		public override void OnEnter()
		{
			base.OnEnter();

			rewarded = false;
			btnMedal.m_btn.interactable = false;

			RewardAd.Instance.rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
			RewardAd.Instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
			RewardAd.Instance.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;

			if (RewardAd.Instance.rewardBasedVideo.IsLoaded())
			{
				RewardAd.Instance.rewardBasedVideo.Show();
			}

#if UNITY_EDITOR
			rewarded = true;
			Finish();
#endif

		}

		private void HandleRewardBasedVideoRewarded(object sender, Reward e)
		{
			rewarded = true;
		}

		private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
		{
			Finish();
		}

		private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
		{

		}

		public override void OnExit()
		{
			base.OnExit();
			RewardAd.Instance.rewardBasedVideo.OnAdStarted -= HandleRewardBasedVideoStarted;
			RewardAd.Instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
			RewardAd.Instance.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;

			if(rewarded)
			{
				DataManager.Instance.user_data.Write(
					Defines.KEY_LAST_REWARD_TIME,
					NTPTimer.Instance.now.ToString("yyyy/MM/dd HH:mm:ss"));

				DataManager.Instance.dataItem.AddItem(btnMedal.medal_item_id, 20);
				DataManager.Instance.dataItem.Save();
				btnMedal.ShowUpdate();
			}

		}
	}




}