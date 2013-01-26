using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CIEditor {

	static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "CIBuild1";
    static string TARGET_DIR = "build";

    [MenuItem ("Custom/CI/Build Mac OS X")]
    static void PerformMacOSXBuild ()
    {
             string target_dir = APP_NAME + ".app";
             GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.StandaloneOSXIntel,BuildOptions.None);
    }
	
	[MenuItem ("Custom/CI/Build iOS")]
    static void PerformIOSBuild ()
    {
             string target_dir = APP_NAME + "";
             GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.iPhone,BuildOptions.None);
    }
	
	[MenuItem ("Custom/CI/Build Web Player")]
    static void PerformWebBuild ()
    {
		//this is an actual dir where the html and unity3d file will go
             string target_dir = "ping";
             GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.WebPlayer ,BuildOptions.None);
    }

	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {   
		if(!Directory.Exists(TARGET_DIR)){
			Directory.CreateDirectory(TARGET_DIR);
		}
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
            if (res.Length > 0) {
                    throw new Exception("BuildPlayer failure: " + res);
            }
    }
}
