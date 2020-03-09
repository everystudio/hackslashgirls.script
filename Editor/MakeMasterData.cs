using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace CorridorGirls
{
	public class MakeMasterData
	{


		[MenuItem("Tools/Make MasterData")]
		public static void MakeData()
		{
			EveryStudioLibrary.Editor.EditorCoroutine.start(makeData());
		}

		[MenuItem("Tools/Make MasterData_test")]
		public static void MakeDataTest()
		{
			EveryStudioLibrary.Editor.EditorCoroutine.start(makeData());
		}

		private static IEnumerator makeDataTest()
		{
			/*
			MasterMissionDetail masterMissionDetail = new MasterMissionDetail();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMissionDetail.SpreadSheet(DataManagerGame.SS_ID, "mission_detail", () => {
				masterMissionDetail.SaveEditor("07data/master", "master_mission_detail");
				foreach (MasterMissionDetailParam p in masterMissionDetail.list)
				{
					//Debug.Log(p.message);
				}
			}));
			*/
			yield return null;
		}

		static IEnumerator makeData()
		{
			#region ゲーム
			MasterDungeon masterDungeon = new MasterDungeon();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterDungeon.SpreadSheet(DataManager.SS_MASTER, "dungeon", () => {
				masterDungeon.SaveEditor("07data/master", "master_dungeon");
			}));
			MasterEnemy masterEnemy = new MasterEnemy();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterEnemy.SpreadSheet(DataManager.SS_MASTER, "enemy", () => {
				masterEnemy.SaveEditor("07data/master", "master_enemy");
			}));
			MasterFloor masterFloor = new MasterFloor();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterFloor.SpreadSheet(DataManager.SS_MASTER, "floor", () => {
				masterFloor.SaveEditor("07data/master", "master_floor");
			}));

			MasterItem masterItem = new MasterItem();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterItem.SpreadSheet(DataManager.SS_MASTER, "item", () => {
				masterItem.SaveEditor("07data/master", "master_item");
			}));

			MasterMedalPrize masterMedalPrize = new MasterMedalPrize();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterMedalPrize.SpreadSheet(DataManager.SS_MASTER, "medal_prize", () => {
				masterMedalPrize.SaveEditor("07data/master", "master_medal_prize");
			}));

			MasterSkin masterSkin = new MasterSkin();
			yield return EveryStudioLibrary.Editor.EditorCoroutine.start(masterSkin.SpreadSheet(DataManager.SS_MASTER, "skin", () => {
				masterSkin.SaveEditor("07data/master", "master_skin");
			}));

			#endregion

			#region キャンプ


			#endregion
			#region チュートリアル用
			#endregion

		}

	}
}
