using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class EditorScripts : MonoBehaviour
{

    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
#if UNITY_IOS

        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            string mainTargetGuid;

            string unityFrameworkTargetGuid;

            PBXProject pbxProject = new PBXProject();

            var unityMainTargetGuidMethod = pbxProject.GetType().GetMethod("GetUnityMainTargetGuid");
            var unityFrameworkTargetGuidMethod = pbxProject.GetType().GetMethod("GetUnityFrameworkTargetGuid");

            pbxProject.ReadFromFile(projectPath);

            if (unityMainTargetGuidMethod != null && unityFrameworkTargetGuidMethod != null)
            {
                mainTargetGuid = (string)unityMainTargetGuidMethod.Invoke(pbxProject, null);
                unityFrameworkTargetGuid = (string)unityFrameworkTargetGuidMethod.Invoke(pbxProject, null);
                pbxProject.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
            }

            else
            {
                mainTargetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
                unityFrameworkTargetGuid = mainTargetGuid;
                pbxProject.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");
                pbxProject.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
            }

            
            pbxProject.WriteToFile(projectPath);
        }
#endif
    }
}

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public static class PlayModeStateChangedExample
{
    // register an event handler when the class is initialized
    static PlayModeStateChangedExample()
    {
#if !UNITY_STANDALONE
        EditorApplication.playModeStateChanged += LogPlayModeState;
#else
        EditorApplication.playModeStateChanged -= LogPlayModeState;
#endif

    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            string title = "Editor execution not available";
            string content = "ManoMotion SDK does not support in Editor functionality on Android platform. try windows x64 platform. Please compile your project for either iOS or Android. For more help, please visit our documentation.";

            string ok_button = "OK";
            string cancel_button = "Continue anyway";
            string alt_button = "Take me there";
            int ans = EditorUtility.DisplayDialogComplex(title, content, ok_button, cancel_button, alt_button);
            switch (ans)
            {
                case 0://ok
                    UnityEditor.EditorApplication.isPlaying = false;

                    break;
                case 1://cancel
                    break;
                case 2://continue
                    Application.OpenURL("https://www.manomotion.com/documentation/#quickstart-4");

                    UnityEditor.EditorApplication.isPlaying = false;

                    break;
                default:
                    break;
            }
        }
    }
}