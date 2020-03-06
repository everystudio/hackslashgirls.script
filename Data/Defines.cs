using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defines  {
	public const string KeyCoin = "coin";
	public const string KeyGem = "gem";

	public const string KEY_SOUND_BGM = "BGM";
	public const string KEY_SOUND_SE = "SE";
	public const float SOUND_VOLUME_MAX = 0.0f;
	public const float SOUND_VOLME_MIN = -80.0f;

	public const string KEY_GAMESPEED_LEVEL = "game_speed_level";
	public const string KEY_GAMESPEEDMETER = "game_speed_meter";
	public const string KEY_LAST_REWARD_TIME = "KEY_LAST_REWARD_TIME";

	public const string KEY_DUNGEON_ID = "dungeon_id";

	public const string KEY_STOP_100 = "stop_100";
	public const string KEY_NOT_SLEEP = "not_sleep";
	public const string KEY_USE_SKIN_ID = "use_skin_id";

	// プレイヤーデータ
	public const string KEY_CHARA_LEVEL = "chara_level";
	public const string KEY_CHARA_HP = "chara_hp";
	public const string KEY_CHARA_HUNGER = "chara_hunger";
	public const string KEY_CHARA_EXP = "chara_exp";
	public const string KEY_CHARA_FLOOR_CURRENT = "floor_current";
	public const string KEY_CHARA_FLOOR_BEST = "floor_best";

	public static string CurrentDungeonID = "dummy";		// 初回で切り替え作業をさせる

	public const string KEY_RESTART_FLOOR = "floor_restart";

	public const string ATTRIBUTE_NONE = "none";
	public const string ATTRIBUTE_FIRE = "fire";
	public const string ATTRIBUTE_WATER = "water";
	public const string ATTRIBUTE_TREE = "tree";
	public const string ATTRIBUTE_LIGHT = "light";
	public const string ATTRIBUTE_DARK = "dark";

	public const string CRAFT_BULK_COUNT = "craft_bulk_count";

	public const float ATTRIBUTE_RATE_POSITIVE = 2.0f;
	public const float ATTRIBUTE_RATE_FLAT = 1.0f;
	public const float ATTRIBUTE_RATE_NEGATIVE = 0.5f;

	public const int GEM_BOOK_DROP_LIMIT = 3;
	public const int ITEM_ID_DROP_COIN = 1;
	public const int ITEM_ID_DROP_GEM = 2;
	public const int ITEM_ID_DROP_GEM_BOOK = 3;

	public const int ITEM_ID_GEM_BOOK = 11001;

	public const int EQUIP_ITEM_LIMIT = 200;

	public enum ATTRIBUTE_CONDITION
	{
		NEUTRAL		= 0,
		POSITIVE	=1,
		NEGATIVE	=2,
	}
	public static ATTRIBUTE_CONDITION GetAttributeCondition(string _strTarget, string _strSelf)
	{
		switch (_strTarget)
		{
			case ATTRIBUTE_FIRE:
				if (_strSelf == ATTRIBUTE_WATER)
				{
					return ATTRIBUTE_CONDITION.POSITIVE;
				}
				else if (_strSelf == ATTRIBUTE_TREE)
				{
					return ATTRIBUTE_CONDITION.NEGATIVE;
				}
				break;
			case ATTRIBUTE_WATER:
				if (_strSelf == ATTRIBUTE_TREE)
				{
					return ATTRIBUTE_CONDITION.POSITIVE;
				}
				else if (_strSelf == ATTRIBUTE_FIRE)
				{
					return ATTRIBUTE_CONDITION.NEGATIVE;
				}
				break;
			case ATTRIBUTE_TREE:
				if (_strSelf == ATTRIBUTE_FIRE)
				{
					return ATTRIBUTE_CONDITION.POSITIVE;
				}
				else if (_strSelf == ATTRIBUTE_WATER)
				{
					return ATTRIBUTE_CONDITION.NEGATIVE;
				}
				break;
			case ATTRIBUTE_LIGHT:
				if (_strSelf == ATTRIBUTE_DARK)
				{
					return ATTRIBUTE_CONDITION.POSITIVE;
				}
				break;
			case ATTRIBUTE_DARK:
				if (_strSelf == ATTRIBUTE_LIGHT)
				{
					return ATTRIBUTE_CONDITION.POSITIVE;
				}
				break;
			default:
				break;
		}
		return ATTRIBUTE_CONDITION.NEUTRAL;
	}

	public static Color GetAttributeDamageColor(ATTRIBUTE_CONDITION _condition)
	{
		if (_condition == ATTRIBUTE_CONDITION.POSITIVE)
		{
			return Color.red;
		}
		else if (_condition == ATTRIBUTE_CONDITION.NEGATIVE)
		{
			return Color.blue;
		}
		else
		{
			return Color.white;
		}
	}

	public static float GetAttributeRate(string _strTarget , string _strSelf)
	{
		ATTRIBUTE_CONDITION condition = GetAttributeCondition(_strTarget, _strSelf);

		if( condition == ATTRIBUTE_CONDITION.POSITIVE)
		{
			return ATTRIBUTE_RATE_POSITIVE;
		}
		else if( condition == ATTRIBUTE_CONDITION.NEGATIVE)
		{
			return ATTRIBUTE_RATE_NEGATIVE;
		}
		else
		{
			return ATTRIBUTE_RATE_FLAT;
		}
	}




}
