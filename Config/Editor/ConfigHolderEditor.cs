using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConfigHolderEditor : ScriptableObject
{

	public readonly static string ASSET_PATH = "Assets/10config/configholoder.asset";

	[MenuItem("Tools/ConfigHolder/1.Create")]
	private static void Create()
	{
		var asset = AssetDatabase.LoadAssetAtPath<ConfigHolder>(ASSET_PATH);
		if (asset == null)
		{
			asset = CreateInstance<ConfigHolder>();
			AssetDatabase.CreateAsset(asset, ASSET_PATH);
		}
		AssetDatabase.Refresh();
	}

	private static void _setVersion(ref ConfigHolder _asset)
	{
		_asset.version = string.Format("Version:{0}", PlayerSettings.bundleVersion);
	}
	private static void _clearMessageList(ConfigHolder _asset)
	{
		_asset.message_list.Clear();
	}
	private static void _addMessage(ConfigHolder _asset, string _strMessage)
	{
		_asset.message_list.Add(_strMessage);
	}



	[MenuItem("Tools/ConfigHolder/2.SetVersion")]
	private static void SetVersion()
	{
		var asset = AssetDatabase.LoadAssetAtPath<ConfigHolder>(ASSET_PATH);
		_setVersion(ref asset);
		EditorUtility.SetDirty(asset);
		AssetDatabase.Refresh();

	}

	[MenuItem("Tools/ConfigHolder/3.ClearMessageList")]
	private static void ClearMessageList()
	{
		var asset = AssetDatabase.LoadAssetAtPath<ConfigHolder>(ASSET_PATH);
		_clearMessageList(asset);
		EditorUtility.SetDirty(asset);
		AssetDatabase.Refresh();
	}

	public static void SetupBuild()
	{
		var asset = AssetDatabase.LoadAssetAtPath<ConfigHolder>(ASSET_PATH);

		_setVersion(ref asset);
		_clearMessageList(asset);

		// 引数取得
		string[] args = System.Environment.GetCommandLineArgs();

		int i, len = args.Length;
		for (i = 0; i < len; ++i)
		{
			switch (args[i])
			{
				case "/branch":
				case "/debug_message":
					_addMessage(asset, args[i + 1]);
					break;
				case "target_env":
					asset.app_environment = args[i + 1];

					if (args[i + 1] == "development")
					{
						PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, PlayerSettings.applicationIdentifier + ".development");
					}
					break;
				case "build_number":
					PlayerSettings.iOS.buildNumber = args[i + 1];
					break;
			}
		}

		EditorUtility.SetDirty(asset);
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();

	}








}
