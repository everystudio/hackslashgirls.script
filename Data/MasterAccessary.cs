using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterAccessaryParam : CsvDataParam
{
	public int accessary_id { get; set; }
	public int item_id { get; set; }
	public string label { get; set; }

	public int use_item_id { get; set; }
	public int hp_rate { get; set; }
	public int stamina_rate { get; set; }

	public float interval { get; set; }

	public string log_format { get; set; }

	public int situation { get; set; }
}

public class MasterAccessary : CsvData<MasterAccessaryParam> {

}
