using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDungeonParam : CsvDataParam
{
	public string dungeon_id {get; set; }//文字列です
	public string dungeon_label { get; set; }

	public int floor_max { get; set; }

	public string outline { get; set; }

	public int itme_id_ex_coin { get; set; }

	public int retry { get; set; }
	public string background { get; set; }

}

public class MasterDungeon : CsvData<MasterDungeonParam> {

}
