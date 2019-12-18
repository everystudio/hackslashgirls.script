using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataCharaParam : CsvDataParam
{
	public int serial { get; set; }		// 今回は使わない・今後使う予定もなさそうだけど一応

	public bool is_dead { get; set; }

	public int level { get; set; }
	public long exp { get; set; }
	public long exp_pre { get; set; }
	public long exp_next { get; set; }

	public class OnChangeCurrentMax : UnityEvent<int, int>
	{

	}

	public OnChangeCurrentMax OnChangeHp = new OnChangeCurrentMax();
	public int hp{ get
		{
			return m_hp;
		}
		set
		{
			if( value < 0)
			{
				m_hp = 0;
			}
			else
			{
				m_hp = value;
			}
			DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_HP, m_hp);
		}
	}
	private int m_hp;
	public int hp_max { get { return hp_max_chara + hp_max_equip; } }
	public int hp_max_chara { get; set; }
	public int hp_max_equip { get; set; }

	public int hunger
	{
		get
		{
			return m_hunger;
		}
		set
		{
			if( value < 0)
			{
				m_hunger = 0;
			}
			else
			{
				m_hunger = value;
			}
			DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_HUNGER, m_hunger);
		}
	}
	private int m_hunger;
	public int hunger_max { get { return hunger_max_chara + hunger_max_equip; } }
	public int hunger_max_chara { get; set; }
	public int hunger_max_equip { get; set; }

	public void HungerDamage( int _iHungry)
	{
		if( 0 < hunger)
		{
			hunger -= _iHungry;
			if (hunger == 0)
			{
				GameMain.Instance.BattleLog("<color=#FF0>空腹で倒れそうだ、早く何か食べなくては！</color>");
			}
		}
		/*
		else
		{
			hp -= 1;
		}
		*/
	}

	public int attack { get { return attack_chara + attack_equip; } }
	public int defence { get { return defence_chara + defence_equip; } }
	public int speed { get { return speed_chara + speed_equip; } }
	public int magic { get { return magic_chara + magic_equip; } }

	public string attribute_attack { get; set; }
	public string attribute_defence { get; set; }

	public int attack_chara { get; set; }
	public int attack_equip { get; set; }

	public int defence_chara { get; set; }
	public int defence_equip { get; set; }

	public int speed_chara { get; set; }
	public int speed_equip { get; set; }

	public int magic_chara { get; set; }
	public int magic_equip { get; set; }

	public DataItemParam[] equip_data_arr = new DataItemParam[5];
	public MasterItemParam[] equip_master_arr = new MasterItemParam[5];

	public DataItemParam equip_1_data { get; set; }
	public MasterItemParam equip_1_master { get; set; }
	public DataItemParam equip_2_data { get; set; }
	public MasterItemParam equip_2_master { get; set; }
	public DataItemParam equip_3_data { get; set; }
	public MasterItemParam equip_3_master { get; set; }
	public DataItemParam equip_4_data { get; set; }
	public MasterItemParam equip_4_master { get; set; }
	public DataItemParam equip_5_data { get; set; }
	public MasterItemParam equip_5_master { get; set; }

	public const int EXP_BASE = 10;
	public const int EXP_PITCH = 5;

	private long get_exp_level( int _iLevel)
	{
		return EXP_BASE + EXP_PITCH * _iLevel;
	}
	public long GetRequireNextLevelExp_Total(int _iNext)
	{
		long lRet = 0;
		long lLevel1 = get_exp_level(0);
		long lLevelPre = get_exp_level(_iNext );
		lRet = (long)((_iNext+1) * (lLevel1 + lLevelPre)) / (long)2;
		return lRet;
	}

	public bool AddExp( long _lAddExp)
	{
		bool bRet = false;
		exp += _lAddExp;

		if( exp_next <= exp)
		{
			level += 1;
			exp_pre = GetRequireNextLevelExp_Total(level - 1);
			exp_next = GetRequireNextLevelExp_Total(level);

			LevelUp();
			DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_LEVEL, level);

			bRet = true;
		}
		DataManager.Instance.user_data.Write(Defines.KEY_CHARA_EXP, exp.ToString());

		return bRet;
	}

	public const int HP_MAX_BASE = 95;
	public const int HP_MAX_PITCH = 5;

	public const int ATTACK_BASE = 29;
	public const int ATTACK_PITCH = 1;
	public const int DEFENCE_BASE = 29;
	public const int DEFENCE_PITCH = 1;

	public const int SPEED_BASE = 10;
	public const int SPEED_PITCH = 0;

	public int get_hp_max_level( int _iLevel)
	{
		int ret = 0;
		ret = HP_MAX_BASE + HP_MAX_PITCH * _iLevel;
		return ret;
	}

	public int get_attack_level(int _iLevel)
	{
		return ATTACK_BASE + ATTACK_PITCH * _iLevel;
	}
	public int get_defence_level(int _iLevel)
	{
		return DEFENCE_BASE + DEFENCE_PITCH * _iLevel;
	}
	public int get_speed_level(int _iLevel)
	{
		return SPEED_BASE + SPEED_PITCH * _iLevel;
	}

	public int GetHpRate()
	{
		return (int)(((float)hp / (float)hp_max)*100);
	}
	public int GetStaminaRate()
	{
		return (int)(((float)hunger / (float)hunger_max) * 100);
	}

	// 現在のレベルに合わせてパラメータを調整する
	public void LevelUp()
	{
		GameMain.Instance.BattleLog(string.Format("レベルが{0}に上がった", level));
		// 経験値関係は補正されているとします

		int temp_hp_max = hp_max_chara;
		hp_max_chara = get_hp_max_level(level);

		if( temp_hp_max < hp_max_chara)
		{
			int diff = hp_max_chara - temp_hp_max;
			hp += diff;
		}
		//hunger_max_chara = 100;

		attack_chara = get_attack_level(level);
		defence_chara = get_defence_level(level);
		speed_chara = get_speed_level(level);
	}

	private void level_build( int _iLevel)
	{
		level = _iLevel;
		exp = GetRequireNextLevelExp_Total(level - 1);

		exp_pre = exp;
		exp_next = GetRequireNextLevelExp_Total(level);

		hp_max_chara = get_hp_max_level(level);
		hunger_max_chara = 100;

		attack_chara = get_attack_level(level);
		defence_chara = get_defence_level(level);
		speed_chara = get_speed_level(level);

		RefreshEquip();
	}

	public void Build( int _iLevel )
	{
		level_build(_iLevel);

		hp = hp_max;
		hunger = hunger_max;

		DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_LEVEL, level);
		DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_HP, hp);
		DataManager.Instance.user_data.WriteInt(Defines.KEY_CHARA_HUNGER, hunger);

		DataManager.Instance.user_data.Write(Defines.KEY_CHARA_EXP, exp.ToString());
	}

	public void Restore( int _iLevel)
	{
		level_build(_iLevel);

		exp = long.Parse(DataManager.Instance.user_data.Read(Defines.KEY_CHARA_EXP));
		//Debug.Log(DataManager.Instance.user_data.ReadInt(Defines.KEY_CHARA_HP));

		hp = DataManager.Instance.user_data.ReadInt(Defines.KEY_CHARA_HP);
		hunger = DataManager.Instance.user_data.ReadInt(Defines.KEY_CHARA_HUNGER);

		//Debug.Log(string.Format("hp={0} hunger={1}", hp, hunger));
	}




	public void RefreshEquip()
	{
		clear_equip();

		attribute_attack = "none";
		attribute_defence = "none";
		//equip(1);
		for( int i = 0; i < 5; i++)
		{
			equip(i);
			equip_update(equip_data_arr[i], equip_master_arr[i]);
		}
		//equip_update(equip_1_data, equip_1_master);
		//equip_update(equip_2_data, equip_2_master);
		//equip_update(equip_3_data, equip_3_master);
		//equip_update(equip_4_data, equip_4_master);
		//equip_update(equip_5_data, equip_5_master);
	}
	private void equip( int _iIndex)
	{
		int item_serial = DataManager.Instance.user_data.ReadInt(MasterItem.GetEquipIndexName(_iIndex));
		DataItemParam equipData = null;
		MasterItemParam equipMaster = null;
		
		if(item_serial == 0)
		{
			equipData = null;
			equipMaster = null;
		}
		else
		{
			equipData = DataManager.Instance.dataItem.list.Find(p => p.serial == item_serial);
			equipMaster = DataManager.Instance.masterItem.list.Find(p => p.item_id == equipData.item_id);
		}

		equip_data_arr[_iIndex] = equipData;
		equip_master_arr[_iIndex] = equipMaster;

	}

	private void equip_update( DataItemParam _data , MasterItemParam _master)
	{
		if( _data == null || _master == null)
		{
			return;
		}
		int iUpParam = _master.param + _data.craft_count;
		switch( _master.item_id / MasterItem.LargeCategory)
		{
			case MasterItem.CategoryWeapon:
				attack_equip += iUpParam;
				attribute_attack = _master.attribute;
				break;
			case MasterItem.CategoryArmor:
				defence_equip += iUpParam;
				attribute_defence = _master.attribute;
				break;
			case MasterItem.CategoryBracelet:
				hunger_max_equip += iUpParam;
				break;
			case MasterItem.CategoryCloak:
				hp_max_equip += iUpParam;
				break;
			case MasterItem.CategoryHelmet:
				magic_equip += iUpParam;
				break;
			default:
				//Debug.Log("none");
				break;
		}
	}
	private void clear_equip()
	{
		hp_max_equip = 0;
		hunger_max_equip = 0;
		attack_equip = 0;
		defence_equip = 0;
		magic_equip = 0;
	}




}

public class DataChara : CsvData<DataCharaParam> {

}
