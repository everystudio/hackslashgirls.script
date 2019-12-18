using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using System;

namespace CharaControlAction {


	public class CharaControlActionBase : FsmStateAction
	{
		protected CharaControl charaControl;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl = Owner.GetComponent<CharaControl>();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if( DataManager.Instance.dataChara != null)
			{
				if(DataManager.Instance.dataChara.hp <= 0)
				{
					Fsm.Event("dead");
				}
			}
		}

	}
	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Initialize : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Finish();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Standby : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			charaControl.IsStandby = true;
			charaControl.HandleRequestGameStart.AddListener(OnRequestGameStart);
		}

		private void OnRequestGameStart()
		{
			Finish();
		}
		public override void OnExit()
		{
			base.OnExit();
			charaControl.IsStandby = false;
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class GameStart : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			charaControl.ResetPosition();
			DataManager.Instance.dataChara.is_dead = false;

			Fsm.Event("move");

		}
		public override void OnExit()
		{
			base.OnExit();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Wait : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.HandleRequestMove.AddListener(OnRequestMove);
		}
		public override void OnExit()
		{
			base.OnExit();
			charaControl.HandleRequestMove.RemoveListener(OnRequestMove);
		}
		private void OnRequestMove()
		{
			Fsm.Event("move");
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Move : CharaControlActionBase
	{
		public FsmFloat move_delta;
		public FsmFloat limit_pos;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.IsBattle = false;
		}
		public override void OnExit()
		{
			base.OnExit();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();

			Vector3 moved_pos = charaControl.Move(move_delta.Value * Time.deltaTime );

			if(limit_pos.Value <= moved_pos.x)
			{
				Fsm.Event("goal");
			}
		}
	}
	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Goal : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.HandleRequestGoal.Invoke();
			Finish();
		}
	}


	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Eat : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Drink : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class TouchEnemy : CharaControlActionBase
	{
		public FsmGameObject hit_enemy;
		public override void OnEnter()
		{
			base.OnEnter();
			if (hit_enemy.Value != null)
			{
				EnemyBase enemy = hit_enemy.Value.GetComponent<EnemyBase>();
				if( enemy != null)
				{
					enemy.Touched = true;
				}
			}
			Finish();
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class SetAttackInterval : CharaControlActionBase
	{
		public FsmFloat attack_interval;
		public FsmBool is_initialize;
		public override void OnEnter()
		{
			base.OnEnter();
			if(is_initialize .Value == false)
			{
				attack_interval.Value = 1.0f;
			}
			else
			{
				attack_interval.Value = 0.0f;
			}

			float interval_rate = 1.0f;

			int now_stamina = DataManager.Instance.dataChara.hunger;
			if ( 90 < now_stamina)
			{
				interval_rate = 0.5f;
			}
			else if( 75 < now_stamina)
			{
				interval_rate = 0.75f;
			}
			else if( 50 < now_stamina)
			{
				interval_rate = 1.0f;
			}
			else
			{
				interval_rate = Mathf.Lerp(5.0f, 1.0f, (float)now_stamina / 100.0f);
			}

			if (75 < now_stamina)
			{
				interval_rate = 0.0f;
			}
			else {
				interval_rate = Mathf.Lerp(2.0f, 0.0f, (float)now_stamina / 100.0f);
			}

			attack_interval.Value += (1.0f * interval_rate);
			Finish();
		}
	}


	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Battle : CharaControlActionBase
	{
		public FsmFloat attack_interval;
		public override void OnEnter()
		{
			//Debug.Log(attack_interval.Value);
			base.OnEnter();

			charaControl.IsBattle = true;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			attack_interval.Value -= Time.deltaTime;

			if (attack_interval.Value < 0.0f)
			{
				Fsm.Event("attack");
			}
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class AttackMotion : CharaControlActionBase
	{
		public FsmFloat set_delay_time;
		private float delay_time;
		public override void OnEnter()
		{
			base.OnEnter();
			delay_time = set_delay_time.Value;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			delay_time -= Time.deltaTime;

			if( delay_time < 0.0f)
			{
				Finish();
			}
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Attack : CharaControlActionBase
	{
		public FsmFloat attack_wait_time;

		private float wait_time;
		public override void OnEnter()
		{
			base.OnEnter();
			wait_time = attack_wait_time.Value;

			SEControl.Instance.Play("Punch 1_1");

			charaControl.Attack("attack_1" , DataManager.Instance.dataChara.attack, DataManager.Instance.dataChara.attribute_attack);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			wait_time -= Time.deltaTime;

			if( wait_time < 0.0f)
			{
				Finish();
			}
		}
	}

	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class BattleCheck : CharaControlActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			if( charaControl.m_gameMain.IsBattleEnemy())
			{
				Fsm.Event("battle");
			}
			else
			{
				Finish();
			}
		}
	}
	[ActionCategory("CharaControlAction")]
	[HutongGames.PlayMaker.Tooltip("CharaControlAction")]
	public class Dead : CharaControlActionBase
	{
		private float delay_time;
		public override void OnEnter()
		{
			base.OnEnter();
			charaControl.IsBattle = false;

			delay_time = 2.0f;
			charaControl.HandleRequestDead.Invoke();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			delay_time -= Time.deltaTime;

			if(delay_time < 0.0f )
			{
				DataManager.Instance.dataChara.is_dead = true;
				Finish();
			}

		}

	}


}











