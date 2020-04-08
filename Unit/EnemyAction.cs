using HutongGames.PlayMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAction  {
	public class EnemyActionBase : FsmStateAction
	{
		protected Enemy enemy;
		public override void OnEnter()
		{
			base.OnEnter();
			enemy = Owner.GetComponent<Enemy>();
		}
	}
	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Startup : EnemyActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (enemy.Initialized)
			{
				Finish();
			}
		}
	}

	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Wait : EnemyActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}
		public override void OnUpdate()
		{
			base.OnUpdate();
			if( enemy.enemy_param.hp != enemy.enemy_param.hp_max)
			{
				Finish();
			}
			else if( enemy.Touched)
			{
				//Debug.Log("touched");
				Finish();
			}
		}

	}

	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class DeadCheck : EnemyActionBase
	{
		public override void OnUpdate()
		{
			//Debug.Log(enemy.enemy_param.hp);
			base.OnUpdate();
			if (enemy.enemy_param.hp <= 0)
			{
				Fsm.Event("dead");
			}
		}
	}

	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Walk : DeadCheck
	{
		public FsmFloat move_delta;
		public FsmFloat limit_pos;

		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			enemy.Move(move_delta.Value * Time.deltaTime);
			//Vector3 moved_pos = 
		}
	}



	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Battle : DeadCheck
	{
		public FsmFloat attack_interval;
		public override void OnEnter()
		{
			base.OnEnter();
			enemy.enemy_param.is_battle = true;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			/*
			if( enemy.enemy_param.hp <= 0)
			{
				Fsm.Event("dead");
			}
			*/

			attack_interval.Value -= Time.deltaTime;

			if (attack_interval.Value < 0.0f)
			{
				Fsm.Event("attack");
			}
		}
	}

	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class AttackMotion : DeadCheck
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

			if (delay_time < 0.0f)
			{
				Finish();
			}
		}
	}
	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Attack : DeadCheck
	{
		public FsmFloat attack_interval;
		public FsmFloat set_attack_interval;

		public FsmFloat attack_wait_time;

		private float wait_time;
		public override void OnEnter()
		{
			base.OnEnter();
			attack_interval.Value = set_attack_interval.Value;
			wait_time = attack_wait_time.Value;
			enemy.Attack();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			wait_time -= Time.deltaTime;

			if (wait_time < 0.0f)
			{
				Finish();
			}
		}
	}

	[ActionCategory("EnemyAction")]
	[HutongGames.PlayMaker.Tooltip("EnemyAction")]
	public class Dead : EnemyActionBase
	{
		public override void OnEnter()
		{
			base.OnEnter();

			GameMain.Instance.BattleLog(string.Format("Defeated {0}", enemy.master_param.name));

			enemy.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			enemy.enemy_param.is_dead = true;

			enemy.CreateDropObject();

			DataManager.Instance.dataChara.AddExp(enemy.master_param.exp);

			Finish();
		}
	}


}
