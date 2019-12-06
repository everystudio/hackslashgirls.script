using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDungeonParam : CsvDataParam
{
	public string dungeon_id {get; set; }//文字列です
	public string dungeon_label { get; set; }

	public int floor_max { get; set; }

	public string outline { get; set; }

	public int item_id_medal { get; set; }

	public int retry { get; set; }
	public string background { get; set; }


	public string clear_comment { get; set; }
	public int prize_id_1 { get; set; }
	public int prize_id_2 { get; set; }


}

public class MasterDungeon : CsvData<MasterDungeonParam> {

}
