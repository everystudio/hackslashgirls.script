using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterEnemyParam : CsvDataParam
{
	public int enemy_id { get; set; }
	public string name { get; set; }
	public string sprite_name { get; set; }
	public string color { get; set; }
	public int level_hp { get; set; }
	public int level_attack { get; set; }
	public int level_defence { get; set; }
	public int level_speed { get; set; }
	public int size { get; set; }
	public string attribute { get; set; }

	public int drop_prob { get; set; }
	public int drop_item_id { get; set; }
	public int drop_item_id_2 { get; set; }
	public int drop_item_id_3 { get; set; }
	public int exp { get; set; }
	public int boss_level_add { get; set; }

	private const int HP_BASE = 10;
	private const int HP_PITCH = 3;

	private const int ATTACK_BASE = 5;
	private const int ATTACK_PITCH = 3;

	private const int DEFENCE_BASE = 5;
	private const int DEFENCE_PITCH = 5;

	private const int SPEED_BASE = 5;
	private const int SPEED_PITCH = 3;



	public DataEnemyParam Create(bool _bIsBoss)
	{
		DataEnemyParam ret = new DataEnemyParam();

		ret.enemy_id = enemy_id;
		ret.is_battle = false;

		ret.hp_max = HP_BASE + HP_PITCH * level_hp;

		ret.attack = ATTACK_BASE + ATTACK_PITCH * level_attack;
		ret.defence = DEFENCE_BASE + DEFENCE_PITCH * level_defence;
		ret.speed = SPEED_BASE + SPEED_PITCH * level_speed;

		if ( _bIsBoss)
		{
			ret.hp_max *= 2;

			ret.attack += boss_level_add;
			ret.defence += boss_level_add;
		}

		ret.hp = ret.hp_max;

		ret.attribute = attribute;

		ret.enemy_param = this;

		return ret;
	}

	public int GetDropItemId()
	{
		int iRet = 0;

		int[] drop_prob_arr = new int[6]
		{
			drop_prob,		// セットされたアイテム
			1000,			// ドロップCoin
			20,				// ドロップGem
			10,				// ドロップGemの秘伝書
			drop_prob,		// dtop_item_id_2		
			drop_prob,		// dtop_item_id_3
		};

		int []drop_item_id_arr = new int[6]
		{
			drop_item_id,
			1,
			2,
			3,
			drop_item_id_2,
			drop_item_id_3,
		};

		// Gemの秘伝書が3つ以上ある場合はそれ以上ドロップさせない
		DataItemParam gem_book_check = DataManager.Instance.dataItem.list.Find(p => p.item_id == Defines.ITEM_ID_GEM_BOOK && Defines.GEM_BOOK_DROP_LIMIT < p.num);
		if(gem_book_check != null)
		{
			drop_prob_arr[3] = 0;
		}


		int iResultIndex = UtilRand.GetIndex(drop_prob_arr);
		iRet = drop_item_id_arr[iResultIndex];
		/*
		if(iResultIndex== 0 )
		{
			iRet = drop_item_id;
		}
		else
		{
			iRet = iResultIndex;
		}
		*/
		return iRet;
	}

}

public class MasterEnemy :CsvData<MasterEnemyParam> {



}
