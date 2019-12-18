using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : DataManagerBase<DataManager>
{
	string floder_name = "test1";
	public const float LONG_TAP_TIME = 0.5f;

	public TextAsset textMasterItem;
	public TextAsset textMasterEnemy;
	public TextAsset textMasterFloor;
	public TextAsset textMasterDungeon;
	public TextAsset textMasterMedalPrize;
	public TextAsset textMasterSkin;
	public TextAsset textMasterAccessary;

	[SerializeField]
	private UnityEngine.Audio.AudioMixer mixer;

	[HideInInspector]
	public MasterItem masterItem = new MasterItem();
	[HideInInspector]
	public MasterEnemy masterEnemy = new MasterEnemy();
	[HideInInspector]
	public MasterFloor masterFloor = new MasterFloor();
	[HideInInspector]
	public MasterDungeon masterDungeon = new MasterDungeon();
	[HideInInspector]
	public MasterMedalPrize masterMedalPrize = new MasterMedalPrize();
	[HideInInspector]
	public MasterSkin masterSkin = new MasterSkin();
	[HideInInspector]
	public MasterAccessary masterAccessary = new MasterAccessary();

	public DataCharaParam dataChara = new DataCharaParam();

	public GameSpeedControl gameSpeedControl;

	[HideInInspector]
	public DataKvs dataQuest;
	[HideInInspector]
	public DataItem dataItem;

	public ConfigHolder config_holder;

	public void SaveAll()
	{
		dataItem.Save();
		user_data.Save();
	}

	public string GetShortcutKey( int _iIndex)
	{
		return string.Format("shortcut_{0:00}", _iIndex);
	}

	public int floor_current
	{
		get
		{
			return Instance.user_data.ReadInt(Defines.KEY_CHARA_FLOOR_CURRENT);
		}
		set
		{
			Instance.user_data.WriteInt(Defines.KEY_CHARA_FLOOR_CURRENT, value);

		}
	}
	public int floor_best
	{
		get
		{
			// 旧バージョン補正
			return Instance.user_data.ReadInt( string.Format("{0}{1}", Defines.KEY_CHARA_FLOOR_BEST , Defines.CurrentDungeonID) );
		}
		set
		{
			Instance.user_data.WriteInt(string.Format("{0}{1}", Defines.KEY_CHARA_FLOOR_BEST, Defines.CurrentDungeonID), value);
		}
	}
	public int GetBestFloor(string _strDungeonId )
	{
		string key = string.Format("{0}{1}", Defines.KEY_CHARA_FLOOR_BEST, _strDungeonId);
		if (Instance.user_data.HasKey(key))
		{
			return Instance.user_data.ReadInt(key);
		}
		return 0;
	}

	public int floor_restart
	{
		get
		{
			if(false == Instance.user_data.HasKey(Defines.KEY_RESTART_FLOOR))
			{
				Debug.Log("not key restart");
				Instance.user_data.WriteInt(Defines.KEY_RESTART_FLOOR, 1);
			}
			return Instance.user_data.ReadInt(Defines.KEY_RESTART_FLOOR);
		}
		set
		{
			Instance.user_data.WriteInt(Defines.KEY_RESTART_FLOOR, value);
		}
	}

	public bool Initialized = false;

	public bool UpdateFloor( int _iFloor , bool _bSave )
	{
		bool bRet = false;
		if( Instance.user_data.ReadInt(Defines.KEY_CHARA_FLOOR_CURRENT) != _iFloor){
			bRet = true;
			Instance.user_data.WriteInt(Defines.KEY_CHARA_FLOOR_CURRENT, _iFloor);
		}
		if(Instance.floor_best <_iFloor )
		{
			//Instance.user_data.WriteInt(Defines.KEY_CHARA_FLOOR_BEST, _iFloor);
			Instance.floor_best = _iFloor;
			bRet = true;
		}
		if (_bSave)
		{
			Instance.user_data.Save();
		}
		return bRet;
	}

	public void LoadDungeonData(string _strDungeon)
	{

	}



	public override void Initialize()
	{
		base.Initialize();

		masterItem.Load(textMasterItem);
		masterEnemy.Load(textMasterEnemy);
		masterFloor.Load(textMasterFloor);
		masterDungeon.Load(textMasterDungeon);
		masterMedalPrize.Load(textMasterMedalPrize);
		masterSkin.Load(textMasterSkin);
		masterAccessary.Load(textMasterAccessary);

		string data_item = string.Format("{0}/{1}", floder_name, "data_item");
		dataItem = new DataItem();
		dataItem.SetSaveFilename(data_item);
		if (false == dataItem.LoadMulti(data_item))
		{
			List<MasterItemParam> initiali_data_list = masterItem.list.FindAll(p => p.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic);
			foreach( MasterItemParam param in initiali_data_list)
			{
				if( 0 < param.item_id /10)
				{
					dataItem.AddItem(param.item_id, 0);
				}
			}
			dataItem.Save();
		}
		dataItem.list.Sort((a, b) => a.item_id - b.item_id);

		string strUserData = string.Format("{0}/{1}", floder_name, "user_data");
		user_data.SetSaveFilename(strUserData);
		if ( false == user_data.LoadMulti(strUserData))
		{
			// ユーザーデータの初期化処理
			floor_current = 1;
			floor_best = 1;
			floor_restart = 1;

			user_data.WriteInt(Defines.KeyCoin, 0);
			user_data.WriteInt(Defines.KeyGem, 0);


			user_data.Write(Defines.KEY_SOUND_BGM, 1.0f.ToString());
			user_data.Write(Defines.KEY_SOUND_SE, 1.0f.ToString());

			user_data.WriteInt(Defines.KEY_GAMESPEEDMETER, 1);

			user_data.Save();
		}
		if (user_data.HasKey(Defines.KEY_SOUND_BGM) == false)
		{
			user_data.Write(Defines.KEY_SOUND_BGM, 1.0f.ToString());
		}
		if (user_data.HasKey(Defines.KEY_SOUND_SE) == false)
		{
			user_data.Write(Defines.KEY_SOUND_SE, 1.0f.ToString());
		}

		if(user_data.HasKey(Defines.KEY_CHARA_LEVEL))
		{
			Debug.Log("chara_restore");
			dataChara.Restore(user_data.ReadInt(Defines.KEY_CHARA_LEVEL));
		}
		else
		{
			// リスタートフロアをセットする
			//user_data.WriteInt(Defines.KEY_CHARA_FLOOR_CURRENT, Instance.floor_restart);
			Instance.floor_current = Instance.floor_restart;
			dataChara.Build(1);

			user_data.Save();
		}


		SetSpeedMeter(user_data.ReadInt(Defines.KEY_GAMESPEEDMETER) );

		if(user_data.HasKey(Defines.KEY_GAMESPEED_LEVEL))
		{
			int game_speed_level = user_data.ReadInt(Defines.KEY_GAMESPEED_LEVEL);
			//Debug.Log(game_speed_level);
			gameSpeedControl.SetSpeedLevel(game_speed_level);
		}

		//Debug.Log(user_data.list.Count);
		foreach ( CsvKvsParam p in user_data.list)
		{
			//Debug.Log(p.key);
		}
		Debug.Log("datamanager.initialized");

		Initialized = true;

	}

	public void SetSpeedMeter(int value)
	{
		gameSpeedControl.gameObject.SetActive(true);
		gameSpeedControl.SetMeter(value);
		user_data.WriteInt(Defines.KEY_GAMESPEEDMETER, value);
		user_data.Save();

	}

	void Start()
	{
		// スタートより早くセットすると何者かに上書きされます。死ね
		mixer.SetFloat(Defines.KEY_SOUND_BGM, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUND_BGM)));
		mixer.SetFloat(Defines.KEY_SOUND_SE, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, user_data.ReadFloat(Defines.KEY_SOUND_SE)));

		// タイムスケールで倍速対応できるのありがたい
		Time.timeScale = 1.0f;

	}
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus == false )
		{
			if( PlayerPrefs.HasKey("sleep_time"))
			{
				System.TimeSpan span = TimeManager.Instance.GetDiffNow(PlayerPrefs.GetString("sleep_time"));
				int iAddCoin = 0;
				int iAddGem = 0;
				if( 1 <= span.TotalDays)
				{
					iAddCoin = 30 * 100 * DataManager.Instance.floor_best;
					iAddGem = 30 * 1 * DataManager.Instance.floor_best;
				}
				else if( 1 <= span.TotalHours)
				{
					iAddCoin = (int)span.TotalHours * 100 * DataManager.Instance.floor_best;
					iAddGem = (int)span.TotalHours * 1 * DataManager.Instance.floor_best;

				}
				else if( 1<= span.TotalMinutes)
				{
					iAddCoin = ((int)span.TotalHours * 100 * DataManager.Instance.floor_best)/60;
					iAddGem = ((int)span.TotalHours * 1 * DataManager.Instance.floor_best)/60;
				}
				if (0 < iAddCoin)
				{
					GameMain.Instance.BattleLog(string.Format("放置ボーナスで<color=#FF0>{0}</color>コインゲット" , iAddCoin));
				}
				if( 0 < iAddGem)
				{
					GameMain.Instance.BattleLog(string.Format("放置ボーナスで<color=#ff00ff>{0}</color>Gemゲット", iAddGem));
				}
				PlayerPrefs.DeleteKey("sleep_time");
				PlayerPrefs.Save();
			}
		}
		else
		{
			string now = TimeManager.StrGetTime();
			PlayerPrefs.SetString("sleep_time", now);
			PlayerPrefs.Save();
		}
	}
}
