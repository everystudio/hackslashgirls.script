using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
public class Processor : IPreprocessBuild, IPostprocessBuild
{

	// ビルド前処理
	public void OnPreprocessBuild(UnityEditor.BuildTarget target, string path)
	{
		if( target == BuildTarget.Android)
		{
			PlayerSettings.Android.keystoreName = "./everystudio_everystudio_everypass.keystore";
			PlayerSettings.Android.keystorePass = "everystudio";

			PlayerSettings.Android.keyaliasName = "everystudio";
			PlayerSettings.Android.keyaliasPass = "everypass";

			BuildScript.apk_filename = "android.apk";
		}
		else if( target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
		{
			EditDirectory.Delete(Application.dataPath + "/../" + Path.Combine(BuildScript.BUILD_PROJECT_DIRECTORY, BuildScript.GetPlatformFolderForAssetBundles(target)));
		}

		// 通常ビルド時にはマスターデータは作らない
		// 開発中にしてください（マスターだけ編集した場合にめんどっちいな）
		//Labyrinthgirls.MakeMasterData.Make();
		//AssetbundleCharaTexture.SelectionAsset();

		ConfigHolderEditor.SetupBuild();


		/*
		// 引数取得
		string[] args = System.Environment.GetCommandLineArgs();

		int i, len = args.Length;
		for (i = 0; i < len; ++i)
		{
			switch (args[i])
			{
				case "/branch":
				case "/debug_message":
					break;
			case "target_env":
				asset.environment = args[i + 1];
				break;
				case "build_number":
					PlayerSettings.iOS.buildNumber = args[i + 1];
					break;
			}
		}
				*/


	}

	// ビルド後処理
	[PostProcessBuild(1)]
	public void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{

#if UNITY_IOS
		// Get plist
		string plistPath = pathToBuiltProject + "/Info.plist";
		PlistDocument plist = new PlistDocument();
		plist.ReadFromString(File.ReadAllText(plistPath));

		// Get root
		PlistElementDict rootDict = plist.root;

		// Set encryption usage boolean
		string encryptKey = "ITSAppUsesNonExemptEncryption";
		rootDict.SetBoolean(encryptKey, false);

		// remove exit on suspend if it exists.
		string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
		if (rootDict.values.ContainsKey(exitsOnSuspendKey))
		{
			rootDict.values.Remove(exitsOnSuspendKey);
		}

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
#endif

	}

	// 実行順
	public int callbackOrder { get { return 0; } }
}