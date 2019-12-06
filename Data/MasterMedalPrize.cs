using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterMedalPrizeParam : CsvDataParam
{
	public int medal_prize_id { get; set; }
	public int prize_item_id { get; set; }
	public int limit_num { get; set; }

	public int item_id_medal{ get; set; }
	public int medal_num { get; set; }

}

public class MasterMedalPrize : CsvData<MasterMedalPrizeParam>{


}
