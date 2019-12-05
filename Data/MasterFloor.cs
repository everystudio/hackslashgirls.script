using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterFloorParam : CsvDataParam
{
	public string dungeon_id { get; set; }
	public int floor_id { get; set; }
	public int start { get; set; }
	public int end { get; set; }
	public int enemy_1 { get; set; }
	public int enemy_2 { get; set; }
	public int enemy_3 { get; set; }
	public int boss { get; set; }


	public int[] GetEnemyIndexProb()
	{
		int[] ret = new int[3];

		// 一応クリア
		for( int i = 0; i < 3; i++)
		{
			ret[i] = 0;
		}

		if (0 < enemy_1)
		{
			ret[0] = 100;
		}
		if (0 < enemy_2)
		{
			ret[1] = 100;
		}
		if (0 < enemy_3)
		{
			ret[2] = 100;
		}
		return ret;
	}

	public int GetEnemyId_fromIndex(int _iIndex)
	{
		if (_iIndex == 0)
		{
			return enemy_1;
		}
		else if (_iIndex == 1)
		{
			return enemy_2;
		}
		else if (_iIndex == 2)
		{
			return enemy_3;
		}
		return 0;
	}

}

public class MasterFloor : CsvData<MasterFloorParam> {

}
