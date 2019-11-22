using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUserParam : CsvDataParam
{
	public int level { get; set; }
	public int exp { get; set; }

	public int floor_current { get; set; }
	public int floor_max { get; set; }

	public int coin { get { return coin; } set
		{
			if (coin != value)
			{
				coin = value;
				OnChangeCoin.Invoke(coin);
			}
		}
	}
	public int gem { get; set; }

	public int atk { get; set; }
	public int def { get; set; }

	public int hp { get; set; }
	public int hp_max { get; set; }
	public int hunger { get; set; }
	public int hunger_max { get; set; }

	public int weapon_serial { get; set; }
	public int armor_serial { get; set; }
	public int bracelet_serial { get; set; }
	public int cloak_serial { get; set; }
	public int helmet_serial { get; set; }

	public UnityEventInt OnChangeCoin = new UnityEventInt();
	public UnityEventInt OnChangeGem = new UnityEventInt();

}

public class DataUser :  CsvData<DataUserParam> {



}
