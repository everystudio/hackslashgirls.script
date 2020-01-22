using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataItemParam : CsvDataParam
{
	public int item_id { get; set; }
	//public int category { get; set; } // 引っ張って来れるけど、面倒なのでデータに入れる

	#region 消費アイテム系
	public int num { get; set; }
	#endregion

	#region 装備アイテム
	public int serial { get; set; }
	public int craft_count { get; set; }
	#endregion

	public int shortcut_index { get; set; }
	public int shortcutable { get; set; }
	public string attribute { get; set; }
	public int craft_item_id { get; set; }

	public bool Use()
	{
		bool bRet = false;
		MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == item_id);
		switch(item_id)
		{
			case 10101:
			case 10102:
			case 10103:
			case 10104:
				if ( GameMain.Instance.charaControl.Heal(master.param))
				{
					SEControl.Instance.Play("Magic Heal 03");
					bRet = true;
				}
				break;

			case 10201:
			case 10202:
			case 10203:
			case 10204:
				if (  GameMain.Instance.charaControl.Eat(master.param))
				{
					SEControl.Instance.Play("eat");
					bRet = true;
				}
				break;


			case 11001:
				int get_num = PanelBookCheck.GetBookGemNum();

				bRet = GameMain.Instance.ShowAd(get_num);
				if( bRet == false)
				{
					GameMain.Instance.panelCaution.Show("注意", "現在秘伝書は使えません。\nしばらく時間をあけてください");
				}
				break;
		}
		if(IsMagicItem())
		{
			bRet = GameMain.Instance.charaControl.Magic(master);
		}


		return bRet;
	}

	private bool IsMagicItem()
	{
		return item_id / MasterItem.LargeCategory == MasterItem.CategoryMagic;
	}
	public string GetNumLabel(string _strFormat = "[{1}{0}]" , MasterItemParam _master = null )
	{
		string strNum = "";
		if (item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			strNum = string.Format(_strFormat, num);
		}
		else
		{
			if(_master == null)
			{
				_master = DataManager.Instance.masterItem.list.Find(p => p.item_id == item_id);
			}
			strNum = string.Format(_strFormat, craft_count < _master.limit ? craft_count.ToString() : "★" , "+");
		}
		return strNum;
	}
}

public class DataItem : CsvData<DataItemParam> {

	public int NextSerial()
	{
		int iRet = 0;
		foreach( DataItemParam param in list)
		{
			if(iRet < param.serial)
			{
				iRet = param.serial;
			}
		}
		return iRet + 1;
	}

	public static bool IsCategoryConsumable(int _iItemId )
	{
		return _iItemId / MasterItem.LargeCategory == MasterItem.CategoryConsumable;
	}

	public bool AddItem(MasterItemParam _master , int _iItemId, int _iNum)
	{
		bool bRet = false;

		bool bCountup = false;

		if (_master.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			bCountup = true;
		}
		else if (_master.item_id / MasterItem.LargeCategory == MasterItem.CategorySkin)
		{
			bCountup = true;
		}



		if (bCountup)
		{
			DataItemParam param = list.Find(p => p.item_id == _iItemId);
			if (param != null)
			{
				param.num += _iNum;
				bRet = true;
			}
			else
			{
				DataItemParam add = new DataItemParam();
				add.item_id = _iItemId;
				add.num = _iNum;
				add.serial = NextSerial();
				add.shortcutable = _master.shortcutable;
				add.attribute = _master.attribute;
				add.craft_item_id = _master.craft_item_id;
				this.list.Add(add);
				this.list.Sort((a, b) => a.item_id - b.item_id);

				bRet = true;
			}
		}
		else
		{
			// こっちはまとめ買いできちゃダメっすね
			for (int i = 0; i < _iNum; i++)
			{
				DataItemParam add = new DataItemParam();
				add.item_id = _iItemId;
				add.num = 1;
				add.serial = NextSerial();
				add.craft_count = 0;
				add.shortcutable = _master.shortcutable;
				add.attribute = _master.attribute;
				add.craft_item_id = _master.craft_item_id;
				this.list.Add(add);
				this.list.Sort((a, b) => a.item_id - b.item_id);
				bRet = true;
			}

		}
		return bRet;
	}

	public bool AddItem( int _iItemId , int _iNum)
	{
		//bool bRet = false;

		MasterItemParam master = DataManager.Instance.masterItem.list.Find(p => p.item_id == _iItemId);

		return AddItem(master, _iItemId, _iNum);
		/*
		bool bCountup = false;

		if(master.item_id / MasterItem.LargeCategory <= MasterItem.CategoryMagic)
		{
			bCountup = true;
		}
		else if(master.item_id / MasterItem.LargeCategory == MasterItem.CategorySkin)
		{
			bCountup = true;
		}



		if( bCountup)
		{
			DataItemParam param = list.Find(p => p.item_id == _iItemId);
			if(param != null)
			{
				param.num += _iNum;
				bRet = true;
			}
			else
			{
				DataItemParam add = new DataItemParam();
				add.item_id = _iItemId;
				add.num = _iNum;
				add.serial = NextSerial();
				add.shortcutable = master.shortcutable;
				add.attribute = master.attribute;
				add.craft_item_id = master.craft_item_id;
				DataManager.Instance.dataItem.list.Add(add);
				DataManager.Instance.dataItem.list.Sort((a, b) => a.item_id - b.item_id);

				bRet = true;
			}
		}
		else
		{
			// こっちはまとめ買いできちゃダメっすね
			for( int i = 0; i < _iNum; i++)
			{
				DataItemParam add = new DataItemParam();
				add.item_id = _iItemId;
				add.num = 1;
				add.serial = NextSerial();
				add.craft_count = 0;
				add.shortcutable = master.shortcutable;
				add.attribute = master.attribute;
				add.craft_item_id = master.craft_item_id;
				DataManager.Instance.dataItem.list.Add(add);
				DataManager.Instance.dataItem.list.Sort((a, b) => a.item_id - b.item_id);
				bRet = true;
			}

		}
		return bRet;
		*/
	}
}








