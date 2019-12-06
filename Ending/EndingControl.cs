using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingControl : MonoBehaviour {

	private string floder_name = "test1";

	public void ReturnGame()
	{
		SceneManager.LoadScene("main");
	}

	public TextAsset textMasterItem;
	public TextAsset textMasterDungeon;
	public TextAsset textMasterSkin;

	[HideInInspector]
	public MasterItem masterItem = new MasterItem();
	[HideInInspector]
	public MasterDungeon masterDungeon = new MasterDungeon();
	[HideInInspector]
	public MasterSkin masterSkin = new MasterSkin();

	DataItem dataItem;
	DataKvs user_data;

	public SpriteRenderer bg;
	public OverrideSprite chara;

	public TMPro.TextMeshProUGUI m_txtClearComment;

	IEnumerator Start()
	{

		masterItem.Load(textMasterItem);
		masterDungeon.Load(textMasterDungeon);
		masterSkin.Load(textMasterSkin);

		string data_item = string.Format("{0}/{1}", floder_name, "data_item");
		dataItem = new DataItem();
		dataItem.LoadMulti(data_item);
		dataItem.SetSaveFilename(data_item);
		dataItem.list.Sort((a, b) => a.item_id - b.item_id);

		string strUserData = string.Format("{0}/{1}", floder_name, "user_data");
		user_data = new DataKvs();
		user_data.SetSaveFilename(strUserData);
		user_data.LoadMulti(strUserData);

		yield return null;

		int skin_id = 1;
		if(user_data.HasKey(Defines.KEY_USE_SKIN_ID))
		{
			skin_id = user_data.ReadInt(Defines.KEY_USE_SKIN_ID);
		}

		MasterSkinParam skin = masterSkin.list.Find(p => p.skin_id == skin_id);
		chara.overrideTexture = TextureHolder.Instance.GetTexture(skin.texture_name);


		MasterDungeonParam dungeon = masterDungeon.list.Find(p => p.dungeon_id == user_data.Read(Defines.KEY_DUNGEON_ID));
		bg.sprite = SpriteManager.Instance.Get(dungeon.background);

		m_txtClearComment.text = dungeon.clear_comment;
		//Debug.Log(dungeon.prize_id_1);

		string KeyClearedDungeon = string.Format("cleared_dungeon_{0}", dungeon.dungeon_id);
		if( user_data.HasKey(KeyClearedDungeon) == false)
		{
			user_data.WriteInt(KeyClearedDungeon, 1);
			//Debug.Log(dungeon.prize_id_1);
			if( dungeon.prize_id_1 != 0)
			{
				MasterItemParam master_item_prize1 = masterItem.list.Find(p => p.item_id == dungeon.prize_id_1);

				dataItem.AddItem(master_item_prize1,dungeon.prize_id_1, 1);
			}
			if (dungeon.prize_id_2 != 0)
			{
				MasterItemParam master_item_prize2 = masterItem.list.Find(p => p.item_id == dungeon.prize_id_2);
				dataItem.AddItem(master_item_prize2,dungeon.prize_id_2, 1);
			}
			dataItem.Save();
		}
		user_data.Save();
	}

}
