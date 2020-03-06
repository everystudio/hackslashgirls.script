using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardAd : Singleton<RewardAd> {

	public RewardBasedVideoAd rewardBasedVideo;

	public bool ad_load_error;

    public void Start()
	{
		ad_load_error = false;

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		// Called when an ad is shown.
		rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
		// Called when the ad starts to play.
		rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// Called when the ad is closed.
		rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		// Called when the ad click caused the user to leave the application.
		rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

		//this.RequestRewardBasedVideo();
	}

	private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs e)
	{
		//throw new NotImplementedException();
	}

	private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
	{
		//throw new NotImplementedException();
		//this.RequestRewardBasedVideo();
	}

	private void HandleRewardBasedVideoRewarded(object sender, Reward e)
	{
		//throw new NotImplementedException();
	}

	private void HandleRewardBasedVideoStarted(object sender, EventArgs e)
	{
		//throw new NotImplementedException();
		// 広告の再生開始
	}

	private void HandleRewardBasedVideoOpened(object sender, EventArgs e)
	{
		//throw new NotImplementedException();
		// 広告が開かれた
	}

	private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
	{
		//throw new NotImplementedException();
		ad_load_error = true;
	}

	private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
	{
		// 広告の読み込み開始
		//throw new NotImplementedException();
	}



	public void RequestRewardBasedVideo()
	{
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		adUnitId = "ca-app-pub-5869235725006697/9209051002";
#else
            string adUnitId = "unexpected_platform";
#endif

		if (ad_load_error)
		{
			Debug.LogError("ad load error");
			return;
		}

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice("B58A62380C00BF9DC7BA75C756B5F550")
			.AddTestDevice("30ec665ef7c68238905003e951174579")
			.Build();
		// Load the rewarded video ad with the request.
		this.rewardBasedVideo.LoadAd(request, adUnitId);
	}
}
