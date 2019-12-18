using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharaControl : MonoBehaviour {

	public bool IsStandby;

	public UnityEvent HandleRequestGameStart = new UnityEvent();
	public UnityEvent HandleRequestMove = new UnityEvent();
	public UnityEvent HandleRequestRestart = new UnityEvent();



	public UnityEvent HandleRequestGoal = new UnityEvent();
	public UnityEvent HandleRequestDead = new UnityEvent();

	public List<EnemyBase> target_enemy_list = new List<EnemyBase>();

	[SerializeField]
	private List<AttackBase> attack_effect_list;
	[SerializeField]
	private GameObject pos_attack_effect;

	[SerializeField]
	public GameMain m_gameMain;

	[SerializeField]
	private GameObject m_goRootDamageText;
	[SerializeField]
	private GameObject m_prefDamageText;

	[SerializeField]
	private RecoverNum m_prefRecoverNum;	// 回復と空腹度で使って
	[SerializeField]
	private GameObject m_prefHealEffect;
	[SerializeField]
	private GameObject m_prefEatEffect;

	public OverrideSprite override_sprite;


	public bool IsBattle;

	public void ResetPosition()
	{
		gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
	}

	public Vector3 Move( float _fDelta)
	{
		gameObject.transform.localPosition += new Vector3(_fDelta, 0.0f, 0.0f);

		return gameObject.transform.localPosition;
	}

	public void Attack(string _strAttackName , int _iPower , string _strAttribute )
	{
		foreach( AttackBase attack_base in attack_effect_list)
		{
			if( attack_base.effect_name == _strAttackName)
			{
				AttackBase script = PrefabManager.Instance.MakeScript<AttackBase>(attack_base.gameObject, pos_attack_effect);

				//Debug.Log(script.gameObject.transform.localScale);
				script.transform.localScale = attack_base.gameObject.transform.localScale;
				script.transform.localPosition = Vector3.zero;

				script.Attack(_iPower, _strAttribute);

				Destroy(script.gameObject, 2.0f);
			}
		}
	}
	public bool Magic( MasterItemParam _master)
	{
		AttackBase attack_base = attack_effect_list.Find(p => p.effect_name == _master.effect_name);
		if(attack_base == null)
		{
			return false;
		}

		//AttackBase script = PrefabManager.Instance.MakeScript<AttackBase>(attack_base.gameObject, pos_attack_effect);
		AttackBase script = PrefabManager.Instance.MakeScript<AttackBase>(attack_base.gameObject, GameMain.Instance.m_goStage);

		//Debug.Log(script.gameObject.transform.localScale);
		script.transform.localScale = attack_base.gameObject.transform.localScale;
		script.transform.position = pos_attack_effect.transform.position;

		script.Attack(DataManager.Instance.dataChara.magic + _master.param , _master.attribute);

		SEControl.Instance.Play(_master.sound_name);

		Destroy(script.gameObject, 1.0f);

		script.gameObject.AddComponent<MagicMove>();

		return true;
	}

	public void Damage(int _iAttack, string _strAttribute)
	{
		// 減るHPがないときはダメージを受けない

		//Debug.Log(string.Format("hp={0} is_dead={1}", DataManager.Instance.dataChara.is_dead, DataManager.Instance.dataChara.hp));
		if(DataManager.Instance.dataChara.hp < 0 || DataManager.Instance.dataChara.is_dead)
		{
			return;
		}

		const float DAMAGE_VECTOR_BASE = -0.2f;
		const float DAMAGE_VECTOR_SWING = 0.1f;

		/*
		float fDamage = (float)(_iAttack * _iAttack) / (float)DataManager.Instance.dataChara.defence * (DAMAGE_RATE + (UtilRand.GetRange(DAMAGE_SWING) - DAMAGE_SWING * 0.5f));
		fDamage *= Defines.GetAttributeRate(DataManager.Instance.dataChara.attribute_defence, _strAttribute);
		if (fDamage < 1.0f)
		{
			fDamage = 1.0f;
		}
		int iDamageResult = (int)fDamage;
		*/

		int iDamageResult = CalcDamage.Damage(_iAttack, _strAttribute, DataManager.Instance.dataChara.defence, DataManager.Instance.dataChara.attribute_defence);

		//Debug.Log(string.Format("chara attack={0} defence={1}", _iAttack, DataManager.Instance.dataChara.defence));

		Defines.ATTRIBUTE_CONDITION condition = Defines.GetAttributeCondition(DataManager.Instance.dataChara.attribute_defence, _strAttribute);
		Color damage_color = Defines.GetAttributeDamageColor(condition);


		//Debug.Log("player damage :" + iDamageResult.ToString());
		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject obj = Instantiate(m_prefDamageText, Vector3.zero, rot, m_goRootDamageText.transform) as GameObject;

		obj.SetActive(true);
		obj.transform.localPosition = Vector3.zero;
		//obj.GetComponent<Animator>().SetInteger("damage_level", 1);
		obj.GetComponent<TMPro.TextMeshPro>().text = iDamageResult.ToString();
		obj.GetComponent<TMPro.TextMeshPro>().color= damage_color;

		Vector2 dir = new Vector2(
			DAMAGE_VECTOR_BASE - (UtilRand.GetRange(DAMAGE_VECTOR_SWING) + DAMAGE_VECTOR_SWING * 0.5f),
			1.0f + (UtilRand.GetRange(DAMAGE_VECTOR_SWING) + DAMAGE_VECTOR_SWING * 0.5f));
		obj.GetComponent<Rigidbody2D>().AddForce( 200f * dir );
		Destroy(obj, 2);
		DataManager.Instance.dataChara.hp -= iDamageResult;
		GameMain.Instance.BattleLog(string.Format("<color=red>{0}</color>のダメージを受けた！", iDamageResult));
	}

	public bool Heal( int _iHeal)
	{
		bool bRet = DataManager.Instance.dataChara.hp < DataManager.Instance.dataChara.hp_max;

		// HPが0以下の場合も回復出来ず
		if(DataManager.Instance.dataChara.hp <= 0)
		{
			bRet = false;
		}

		if(bRet == false )
		{
			return false;
		}

		const float HEAL_RATE = 1.0f;
		const float HEAL_SWING = 0.05f;
		float fHeal = (float)(_iHeal) * (HEAL_RATE + (UtilRand.GetRange(HEAL_SWING) - HEAL_SWING * 0.5f));
		int iHeal = (int)fHeal;

		DataManager.Instance.dataChara.hp += iHeal;
		if( DataManager.Instance.dataChara.hp_max < DataManager.Instance.dataChara.hp)
		{
			DataManager.Instance.dataChara.hp = DataManager.Instance.dataChara.hp_max;
		}

		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject num = Instantiate(m_prefRecoverNum.gameObject , gameObject.transform.position + new Vector3(0.0f, 0.0f, 0.0f), rot, GameMain.Instance.m_goStage.transform) as GameObject;
		num.SetActive(true);

		num.GetComponent<RecoverNum>().Initialize(iHeal);

		GameObject effect = Instantiate(m_prefHealEffect, gameObject.transform.position + new Vector3(0.0f,0.75f ,0.0f), rot, GameMain.Instance.m_goStage.transform) as GameObject;
		effect.SetActive(true);

		Destroy(num, 2.0f);
		Destroy(effect, 2.0f);

		return true;
	}

	public bool Eat(int _iEat)
	{
		bool bRet = DataManager.Instance.dataChara.hunger < DataManager.Instance.dataChara.hunger_max;
		if( bRet == false)
		{
			return false;
		}

		const float EAT_RATE = 1.0f;
		const float EAT_SWING = 0.05f;
		float fEat = (float)(_iEat) * (EAT_RATE + (UtilRand.GetRange(EAT_SWING) - EAT_SWING * 0.5f));
		int iEat = (int)fEat;

		if(DataManager.Instance.dataChara.hunger_max < DataManager.Instance.dataChara.hunger + iEat)
		{
			DataManager.Instance.dataChara.hunger = DataManager.Instance.dataChara.hunger_max;
		}
		else
		{
			DataManager.Instance.dataChara.hunger += iEat;
		}

		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject num = Instantiate(m_prefRecoverNum.gameObject, gameObject.transform.position + new Vector3(0.0f, 0.0f, 0.0f), rot, GameMain.Instance.m_goStage.transform) as GameObject;
		num.SetActive(true);
		RecoverNum script =　num.GetComponent<RecoverNum>();
		script.Initialize(iEat);
		script.m_txtNum.color = Color.yellow;
		GameObject effect = Instantiate(m_prefEatEffect, gameObject.transform.position + new Vector3(0.0f, 0.75f, 0.0f), rot, GameMain.Instance.m_goStage.transform) as GameObject;
		effect.SetActive(true);
		Destroy(num, 2.0f);
		Destroy(effect, 2.0f);

		return true;
	}


}
