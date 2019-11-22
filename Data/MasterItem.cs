using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterItemParam : CsvDataParam
{
	public int item_id { get; set; }
	public string name { get; set; }

	//public int category { get; set; }

	public string detail { get; set; }

	public int param { get; set; }
	public int limit { get; set; }

	public int shop_id { get; set; }
	public int price { get; set; }

	public int shortcutable { get; set; }
	public int next_item_id { get; set; }

	public string attribute{get; set;}
	public int craft_item_id { get; set; }

	public string sprite_holder { get; set; }
	public string sprite_name { get; set; }
	public string sprite_color { get; set; }

	public string effect_name { get; set; }
	public string sound_name { get; set; }

	public string GetItemName(int _iCraftCount)
	{
		if( limit <= _iCraftCount)
		{
			return string.Format("{0}★", name);
		}
		else if( _iCraftCount == 0)
		{
			return name;
		}
		else
		{
			return string.Format("{0}+{1}", name, _iCraftCount);
		}
	}

	public Color GetSpriteColor()
	{
		switch( sprite_color)
		{
			case "green":
			case "tree":
				return Color.green;
			case "blue":
			case "water":
				return Color.cyan;
			case "orange":
			case "red":
			case "fire":
				return new Color(1.0f, 100.0f / 256.0f, 100.0f / 256.0f);
			case "purple":
			case "dark":
				return new Color(82.0f / 256.0f, 0, 1.0f);
			case "yellow":
			case "light":
				return Color.yellow;
		}
		return Color.white;
	}


}

public class MasterItem : CsvData<MasterItemParam> {
	public const int CategoryConsumable = 1;
	public const int CategoryMagic = 2;
	public const int CategoryWeapon = 3;
	public const int CategoryArmor = 4;
	public const int CategoryBracelet = 5;
	public const int CategoryCloak = 6;
	public const int CategoryHelmet = 7;

	public const int LargeCategory = 10000;

	public static string GetEquipIndexName( int _iIndex)
	{
		return string.Format("equip_index_{0}", _iIndex);
	}

	public static bool AbleBulkBuy( int _iItemId )
	{
		return (_iItemId / MasterItem.LargeCategory <= MasterItem.CategoryMagic);
	}

}
