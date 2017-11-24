#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
using System.Collections.Generic;

public static class TGPostBuil
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
#if (UNITY_4 || UNITY_3 || UNITY_2)
        if (target == BuildTarget.iPhone)
#else
        if (target == BuildTarget.iOS)
#endif
        {
            UnityEditor.XCodeEditor.XCProject project = new UnityEditor.XCodeEditor.XCProject(path);
			string[] projmods = System.IO.Directory.GetFiles(
                    System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "TGSDK"), "Plugins"), "TGSDK.projmods", System.IO.SearchOption.AllDirectories);
            if(projmods.Length == 0)
			{
				Debug.LogWarning("[TGPostBuil]TGSDK.projmods not found!");
			}
			foreach (string p in projmods)
            {
                project.ApplyMod(p);
            }

			// dirty fix for "AdSupport.framework"
			PBXFileReference fileReference = project.GetFile( System.IO.Path.GetFileName( "System/Library/Frameworks/AdSupport.framework" ) );	
			if(fileReference != null)
			{
				foreach( KeyValuePair<string, PBXFrameworksBuildPhase> currentObject in project.frameworkBuildPhases ) {
					project.BuildAddFile(fileReference,currentObject,false);
				}
			}

            // project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Release");
            // project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Debug");
			project.AddOtherLinkerFlags("-ObjC");
            project.Save();

        }
    }
}
#endif
