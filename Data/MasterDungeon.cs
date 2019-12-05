using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDungeonParam : CsvDataParam
{
	public string dungeon_name;
	public string dungeon_label;

	public int floor_max;

	public string outline;

	public int itme_id_ex_coin;
}

public class MasterDungeon : CsvData<MasterDungeonParam> {

}
