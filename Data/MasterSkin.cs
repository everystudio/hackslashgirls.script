using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSkinParam :CsvDataParam
{
	public int skin_id { get; set; }
	public int item_id { get; set; }

	public string skin_name { get; set; }
	public string skin_outline { get; set; }

	public string texture_name { get; set; }
}

public class MasterSkin : CsvData<MasterSkinParam> {

}
