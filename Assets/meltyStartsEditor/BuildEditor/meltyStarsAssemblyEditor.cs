using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

namespace meltyStars.Editor
{
    public class meltyStarsAssemblyEditor
    {
        private static void CreateDirIfNotExists(string dirName)
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }
        public static string CodePath => Path.GetFullPath($"{Application.dataPath}/../meltyStarsHotfix/");
        public static string AssemblyOutputPath => Path.GetFullPath($"{Application.dataPath}/../Temp/meltyStars");
        public static string AssemblyAssetsOutputPath => $"{Application.dataPath}/ResBundles/Assembly";
        public static string GetDllBuildOutputDirByTarget(BuildTarget target)
        {
            return $"{AssemblyOutputPath}/{target}";
        }
        public static string GetDllBytesOutputDirByTarget(BuildTarget target)
        {
            return $"{AssemblyAssetsOutputPath}/{target}";
        }

        [MenuItem("meltyStars/CompileAssembly/ActiveBuildTarget")]
        public static void CompileDllActiveTarget()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }
        [MenuItem("meltyStars/CompileAssembly/Win64")]
        public static void CompileDllWin64()
        {
            var target = BuildTarget.StandaloneWindows64;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileAssembly/Linux64")]
        public static void CompileDllLinux()
        {
            var target = BuildTarget.StandaloneLinux64;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileAssembly/OSX")]
        public static void CompileDllOSX()
        {
            var target = BuildTarget.StandaloneOSX;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileAssembly/Android")]
        public static void CompileDllAndroid()
        {
            var target = BuildTarget.Android;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }

        [MenuItem("meltyStars/CompileAssembly/IOS")]
        public static void CompileDllIOS()
        {
            var target = BuildTarget.iOS;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target);
        }
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <param name="buildPath"></param>
        /// <param name="target"></param>
        private static void CompileAssembly(string buildPath, BuildTarget target)
        {
            //var group = BuildPipeline.GetBuildTargetGroup(target);

            //ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
            //scriptCompilationSettings.group = group;
            //scriptCompilationSettings.target = target;
            //scriptCompilationSettings.options = ScriptCompilationOptions.DevelopmentBuild;
            //CreateDirIfNotExists(buildPath);
            //ScriptCompilationResult scriptCompilationResult = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildPath);
            //foreach (var ass in scriptCompilationResult.assemblies)
            //{
            //    Debug.LogFormat("compile assemblies:{0}", ass);
            //}

            //CopyDllsToAssetsMenu(buildPath, GetDllBytesOutputDirByTarget(target));

            string hotfixCodePath = "meltyStarsHotfix/";
            List<string> allScripts = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(hotfixCodePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories);
            foreach (var script in fileInfos)
            {
                MSLogger.LogWarning(script.FullName);
            }
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
