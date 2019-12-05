using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
namespace GameMainAction {

	public class GameMainActionBase : FsmStateAction
	{
		protected GameMain gameMain;
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain = Owner.GetComponent<GameMain>();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Startup : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (DataManager.Instance.Initialized)
			{
				// 旧バージョン補正
				if(DataManager.Instance.user_data.HasKey(Defines.KEY_CHARA_FLOOR_BEST))
				{
					Debug.LogWarning("replace_floor_best");
					DataManager.Instance.user_data.WriteInt(string.Format("{0}{1}", Defines.KEY_CHARA_FLOOR_BEST, "normal" ),
						DataManager.Instance.user_data.ReadInt(Defines.KEY_CHARA_FLOOR_BEST));
					DataManager.Instance.user_data.Remove(Defines.KEY_CHARA_FLOOR_BEST);
				}

				if( false == DataManager.Instance.user_data.HasKey(Defines.KEY_DUNGEON_ID))
				{
					DataManager.Instance.user_data.Write(Defines.KEY_DUNGEON_ID, "normal");
				}





				Finish();
			}
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class FadeOutIn : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.FadeOut();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if( gameMain.m_fadeScreen.fade_state == FadeScreen.STATE.CLOSE)
			{
				Finish();
			}
		}
		public override void OnExit()
		{
			base.OnExit();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class DungeonSetup : GameMainActionBase
	{
		public FsmString dungeon_id;

		public override void OnEnter()
		{
			base.OnEnter();

			if (DataManager.Instance.user_data.HasKey(Defines.KEY_DUNGEON_ID))
			{
				dungeon_id.Value = DataManager.Instance.user_data.Read(Defines.KEY_DUNGEON_ID);
			}
			else
			{
				dungeon_id.Value = "normal";
			}

			if ( dungeon_id.Value != Defines.CurrentDungeonID)
			{
				Defines.CurrentDungeonID = dungeon_id.Value;
				MasterDungeonParam master_dungeon = DataManager.Instance.masterDungeon.list.Find(p => p.dungeon_id == dungeon_id.Value);

				Debug.Log(dungeon_id.Value);
				// 背景切り替え
				gameMain.SetBackground(master_dungeon);

			}
			gameMain.FadeIn();

			Finish();
		}

	}



	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Start : GameMainActionBase
	{
		public FsmInt current_floor;
		public FsmInt best_floor;

		public FsmString dungeon_id;

		public override void OnEnter()
		{
			base.OnEnter();

			int iCurrentFloor = DataManager.Instance.floor_current;
			current_floor.Value = iCurrentFloor;

			MasterDungeonParam master_dungeon = DataManager.Instance.masterDungeon.list.Find(p => p.dungeon_id == dungeon_id.Value);

			if(master_dungeon.floor_max < iCurrentFloor)
			{
				iCurrentFloor = 1;
				current_floor.Value = iCurrentFloor;
			}

			if (iCurrentFloor < 1)
			{
				iCurrentFloor = 1;
			}
			bool bUpdate = DataManager.Instance.UpdateFloor(iCurrentFloor, false);

			best_floor.Value = DataManager.Instance.floor_best;

			if ( bUpdate)
			{
				DataManager.Instance.user_data.Save();
			}
			gameMain.ClearDropObjects();

			// 敵キャラとかの作成
			create_enemies(current_floor.Value , dungeon_id.Value );
		}

		private void create_enemies(int _iCurrentFloor , string _strDungeonId )
		{
			gameMain.ClearEnemy();

			MasterFloorParam floor_param = DataManager.Instance.masterFloor.list.Find(p =>
				p.dungeon_id == _strDungeonId &&
				(p.start <= _iCurrentFloor && _iCurrentFloor <= p.end));

			int[] enemy_index_prob = floor_param.GetEnemyIndexProb();

			int enemy_num = UtilRand.GetRand(10, 3);

			int[] pos_index = new int[enemy_num];
			int[] pos_index_prob = new int[10];
			for( int i = 0; i < 10; i++)
			{
				pos_index_prob[i] = 100;
			}
			for( int i = 0; i < enemy_num; i++)
			{
				int iResult = UtilRand.GetIndex(pos_index_prob);
				pos_index[i] = iResult;
				pos_index_prob[iResult] = 0;

				//Debug.Log(iResult);
			}



			for ( int i = 0; i < enemy_num; i++)
			{
				//float x = UtilRand.GetRange(8.5f, 3.5f);
				float x = 3.5f + ((8.5f - 3.5f) / 10.0f) * pos_index[i];

				int index = UtilRand.GetIndex(enemy_index_prob);

				int enemy_id = floor_param.GetEnemyId_fromIndex(index);
				MasterEnemyParam enemy = DataManager.Instance.masterEnemy.list.Find(p => p.enemy_id == enemy_id);

				gameMain.CreateEnemy(enemy, x , false);
			}

			if( _iCurrentFloor % 10 == 0)
			{
				MasterEnemyParam boss = DataManager.Instance.masterEnemy.list.Find(p => p.enemy_id == floor_param.boss);
				DataEnemyParam data = gameMain.CreateEnemy(boss, 9.0f , true );
				gameMain.panelBossStatus.Initialize(data, boss);
			}
			else
			{
				gameMain.panelBossStatus.Disable();
			}


		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (gameMain.charaControl.IsStandby)
			{
				gameMain.charaControl.HandleRequestGameStart.Invoke();
				Finish();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			gameMain.FadeIn();
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Game : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.charaControl.HandleRequestGoal.AddListener(OnCharaGoal);
			gameMain.charaControl.HandleRequestDead.AddListener(OnCharaDead);
		}

		private void OnCharaDead()
		{
			Fsm.Event("dead");
		}

		private void OnCharaGoal()
		{
			Fsm.Event("goal");
		}

		public override void OnExit()
		{
			base.OnExit();
			gameMain.charaControl.HandleRequestGoal.RemoveListener(OnCharaGoal);
			gameMain.charaControl.HandleRequestDead.RemoveListener(OnCharaDead);
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Next : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Goal : GameMainActionBase
	{
		public FsmInt current_floor;
		public FsmInt best_floor;
		public FsmInt review_floor;

		public float delay_time;
		public override void OnEnter()
		{
			base.OnEnter();
			int iNextFloor = current_floor.Value + 1;

			if( 1000 < iNextFloor)
			{
				/*
				if (1001 <= DataManager.Instance.floor_best)
				{
					SceneManager.LoadScene("ending");

				}
				*/
				DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_FLOOR_CURRENT, iNextFloor);
				DataManager.Instance.user_data.Save();
				Fsm.Event("ending");
			}
			else
			{
				bool bIsBestFloor = DataManager.Instance.floor_best < iNextFloor;

				bool bUpdate = DataManager.Instance.UpdateFloor(iNextFloor, false);

				current_floor.Value = iNextFloor;
				if (bUpdate)
				{
					best_floor.Value = iNextFloor;
				}

				if (bIsBestFloor)
				{
					int iAddGem = 10;
					DataManager.Instance.user_data.AddInt(Defines.KeyGem, iAddGem);
					GameMain.Instance.BattleLog(string.Format("<color=#0FF>フロア更新ボーナス</color>：Gem<color=#F00>{0}</color>個獲得", iAddGem));
				}
				DataManager.Instance.SaveAll();

				int iRet = DataManager.Instance.user_data.ReadInt(Defines.KEY_STOP_100);
				//Debug.Log(iRet);

				if( bIsBestFloor && iNextFloor == review_floor.Value)
				{
					Fsm.Event("review");
				}
				else if (0 < iRet && (iNextFloor - 1) % 100 == 0)
				{
					Fsm.Event("stop");
				}
				else
				{
					Finish();
				}
			}

		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Stop : GameMainActionBase
	{
		public FsmGameObject panel_sleep;
		public FsmGameObject btn_close;

		public override void OnEnter()
		{
			base.OnEnter();
			gameMain.Sleep(true);
			panel_sleep.Value.SetActive(true);
			btn_close.Value.GetComponent<Button>().onClick.AddListener(() =>
			{
				Finish();
			});
		}

		public override void OnExit()
		{
			base.OnExit();
			panel_sleep.Value.SetActive(false);
			btn_close.Value.GetComponent<Button>().onClick.RemoveAllListeners();
			gameMain.Sleep(false);
		}
	}

	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Review : GameMainActionBase
	{

		public override void OnEnter()
		{
			base.OnEnter();
			DataManager.Instance.gameSpeedControl.SetIndex(0);

			DataManager.Instance.user_data.AddInt(Defines.KeyGem, 1000);
			DataManager.Instance.SaveAll();
#if UNITY_IOS


			if (Device.RequestStoreReview())
			{
				Finish();
			}
			else
			{
				Finish();
			}
#else
			Finish();
#endif
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}


	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Dead : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			DataManager.Instance.user_data.Remove(Defines.KEY_CHARA_LEVEL);
			DataManager.Instance.SaveAll();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if( DataManager.Instance.dataChara.is_dead)
			{
				Finish();
			}
		}
		public override void OnExit()
		{
			base.OnExit();
		}
	}
	[ActionCategory("GameMainAction")]
	[HutongGames.PlayMaker.Tooltip("GameMainAction")]
	public class Restart : GameMainActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			DataManager.Instance.dataChara.Build(1);

			DataManager.Instance.floor_current = DataManager.Instance.floor_restart;

			DataManager.Instance.SaveAll();

			Finish();
		}
	}
	



}
