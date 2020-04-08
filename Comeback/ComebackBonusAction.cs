using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComebackBonusAction {
	public class ComebackBonusActionBase : FsmStateAction
	{
		protected ComebackBonus comebackBonus;
		public override void OnEnter()
		{
			base.OnEnter();
			comebackBonus = Owner.GetComponent<ComebackBonus>();
		}
	}
	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class initialize : ComebackBonusActionBase
	{

	}
	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class TimerSet : ComebackBonusActionBase
	{
		public FsmString saved;

		public override void OnEnter()
		{
			base.OnEnter();

			if( PlayerPrefs.HasKey("last_play_date"))
			{
				saved.Value = PlayerPrefs.GetString("last_play_date");
			}
			else
			{
				saved.Value = TimeManager.StrGetTime();
			}

			Finish();

		}

	}


	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class CheckDiff : ComebackBonusActionBase
	{
		public FsmString saved;

		public override void OnEnter()
		{
			base.OnEnter();

			//Debug.Log(TimeManager.StrGetTime());

			System.TimeSpan span = TimeManager.Instance.GetDiffNow(saved.Value);
			//Debug.Log(span.TotalSeconds);

			double use_minutes = span.TotalMinutes * -1;

			// チェックは2分以上の放置
			if (2 <= use_minutes)
			{
				PlayerPrefs.SetString("last_play_date", TimeManager.StrGetTime());
				PlayerPrefs.Save();
				Fsm.Event("bonus");
			}
			else
			{
				Fsm.Event("wait");
			}
		}
	}

	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class ComebackWait : ComebackBonusActionBase
	{
		public FsmString saved;
		public override void OnEnter()
		{
			base.OnEnter();
			PlayerPrefs.SetString("last_play_date", TimeManager.StrGetTime());
			PlayerPrefs.Save();
			saved.Value = TimeManager.StrGetTime();
			Finish();
		}

	}




	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class Bonus : ComebackBonusActionBase
	{
		public FsmString saved;

		public override void OnEnter()
		{
			base.OnEnter();

			System.TimeSpan span = TimeManager.Instance.GetDiffNow(saved.Value);

			//Debug.Log(span.TotalSeconds);

			double days = span.TotalDays * -1;
			double hours = span.TotalHours* -1;
			double minutes = span.TotalMinutes * -1;

			int iAddCoin = 0;
			int iAddGem = 0;

			int iBestFloor = DataManager.Instance.floor_best;

			if (1 <= days)
			{
				//Debug.Log(days);
				iAddCoin = 30 * 100 * iBestFloor;

				//iAddGem = DataManager.Instance.floor_best / 10;
				if(100< DataManager.Instance.floor_best)
				{
					iAddGem = iBestFloor;
				}
				else
				{
					iAddGem = (iBestFloor - 100) / 10;
					iAddGem += 100;
				}
			}
			else if (1 <= hours)
			{
				//Debug.Log(hours);
				iAddCoin = (int)hours * 100 * iBestFloor;

				iAddGem = (int)hours * 1 * iBestFloor / 240;
				if ( 100 < iBestFloor)
				{
					iAddGem = (int)hours * 1 * 100 / 24;
					iAddGem += (int)hours * 1 * (iBestFloor - 100) / 24;
				}
				else
				{
					iAddGem = (int)hours * 1 * iBestFloor / 24;
				}
			}
			else if (1 <= minutes)
			{
				//Debug.Log(minutes);
				iAddCoin = ((int)minutes * 100 * iBestFloor) / 60;

				iAddGem = ((int)minutes * 1 * iBestFloor) / (240 * 60);
				if ( 100 < iBestFloor)
				{
					iAddGem = ((int)minutes * 1 * 100) / (24 * 60);
					iAddGem+= ((int)minutes * 1 * (iBestFloor-100)) / (240 * 60);
				}
				else
				{
					iAddGem = ((int)minutes * 1 * iBestFloor) / (24 * 60);
				}
			}

			bool bShow = false;

			comebackBonus.m_goCoin.SetActive(false);
			comebackBonus.m_goGem.SetActive(false);

			//Debug.Log(iAddCoin);
			//Debug.Log(iAddGem);

			if (0 < iAddCoin)
			{
				bShow = true;

				string str = string.Format("Comeback Bonus:<color=#FF0>{0}</color> Coins get", iAddCoin);
				comebackBonus.m_txtCoin.text = string.Format("x {0}" , iAddCoin);
				GameMain.Instance.BattleLog(str);
				comebackBonus.m_goCoin.SetActive(true);

				DataManager.Instance.user_data.AddInt(Defines.KeyCoin, iAddCoin);

			}
			if (0 < iAddGem)
			{
				bShow = true;

				comebackBonus.m_goGem.SetActive(true);
				string str = string.Format("Comeback Bonus:<color=#ff00ff>{0}</color> Gems get", iAddGem);
				comebackBonus.m_txtGem.text = string.Format("x {0}", iAddGem);
				DataManager.Instance.user_data.AddInt(Defines.KeyGem, iAddGem);

				GameMain.Instance.BattleLog(str);
			}

			if( bShow)
			{
				comebackBonus.m_goShowRoot.SetActive(true);
			}


			Finish();
		}
	}

	[ActionCategory("ComebackBonusAction")]
	[HutongGames.PlayMaker.Tooltip("ComebackBonusAction")]
	public class BonusClose : ComebackBonusActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			comebackBonus.m_goShowRoot.SetActive(false);
			Finish();
		}
	}


}
