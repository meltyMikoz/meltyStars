using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

namespace meltyStars.Editor
{
    public class meltyStarsDllHelper
    {
        private static void CreateDirIfNotExists(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }

        public static string DllCompileOutputPath => Path.GetFullPath($"{Application.dataPath}/../Temp/meltyStars");
        public static string DllBytesOutputPath => $"{Application.dataPath}/ResBundles/DllBytes";
        public static string GetDllBuildOutputDirByTarget(BuildTarget target)
        {
            return $"{DllCompileOutputPath}/{target}";
        }
        public static string GetDllBytesOutputDirByTarget(BuildTarget target)
        {
            return $"{DllBytesOutputPath}/{target}";
        }

        [MenuItem("meltyStars/CompileDll/ActiveBuildTarget")]
        public static void CompileDllActiveTarget()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }
        [MenuItem("meltyStars/CompileDll/Win64")]
        public static void CompileDllWin64()
        {
            var target = BuildTarget.StandaloneWindows64;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileDll/Linux64")]
        public static void CompileDllLinux()
        {
            var target = BuildTarget.StandaloneLinux64;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileDll/OSX")]
        public static void CompileDllOSX()
        {
            var target = BuildTarget.StandaloneOSX;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileDll/Android")]
        public static void CompileDllAndroid()
        {
            var target = BuildTarget.Android;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileDll/IOS")]
        public static void CompileDllIOS()
        {
            var target = BuildTarget.iOS;
            CompileDll(GetDllBuildOutputDirByTarget(target), target);
        }
        /// <summary>
        /// 编译Dll
        /// </summary>
        /// <param name="buildPath"></param>
        /// <param name="target"></param>
        private static void CompileDll(string buildPath, BuildTarget target)
        {
            var group = BuildPipeline.GetBuildTargetGroup(target);

            ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            scriptCompilationSettings.group = group;
            scriptCompilationSettings.target = target;
            scriptCompilationSettings.options = ScriptCompilationOptions.DevelopmentBuild;
            CreateDirIfNotExists(buildPath);
            ScriptCompilationResult scriptCompilationResult = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildPath);
            foreach (var ass in scriptCompilationResult.assemblies)
            {
                Debug.LogFormat("compile assemblies:{0}", ass);
            }

            CopyDllsToAssetsMenu(buildPath, GetDllBytesOutputDirByTarget(target));
        }
        /// <summary>
        /// 在这里修改热更新dll列表
        /// </summary>
        /// <param name="path"></param>
        private static void CopyDllsToAssetsMenu(string buildPath, string bytesDir)
        {
            CreateDirIfNotExists(bytesDir);
            List<string> hotfixDlls = new List<string>()
            {
                "meltyStars.Hotfix.dll"
            };
            hotfixDlls.ForEach(dll =>
            {
                string dllOrigin = $"{buildPath}/{dll}";
                string dllBytes = $"{bytesDir}/{dll}.bytes";
                File.Copy(dllOrigin, dllBytes, true);
                //拷贝一份到StreamingAssets
                File.Copy(dllOrigin, $"{Application.streamingAssetsPath}/{dll}.bytes", true);
            });
            AssetDatabase.Refresh();
        }

    }
}
