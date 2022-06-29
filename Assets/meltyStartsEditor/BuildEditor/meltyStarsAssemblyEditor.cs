using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEngine;
using System.IO;
using UnityEditor.Compilation;
using System.Linq;

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
        public static string HotfixCodePath => Path.GetFullPath($"{Application.dataPath}/../meltyStarsHotfix/");
        public static string AssemblyOutputPath => Path.GetFullPath($"{Application.dataPath}/../Temp/meltyStars");
        public static string AssemblyAssetsOutputPath => $"{Application.dataPath}/ResBundles/Assembly";
        public static string GetDllBuildOutputDirByTarget(BuildTarget target)
        {
            return $"{AssemblyOutputPath}\\{target}";
        }
        public static string GetDllBytesOutputDirByTarget(BuildTarget target)
        {
            return $"{AssemblyAssetsOutputPath}\\{target}";
        }

        [MenuItem("meltyStars/CompileAssembly/Debug/ActiveBuildTarget")]
        public static void CompileDllActiveTargetDebug()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }
        [MenuItem("meltyStars/CompileAssembly/Debug/Win64")]
        public static void CompileDllWin64Debug()
        {
            var target = BuildTarget.StandaloneWindows64;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }

        [MenuItem("meltyStars/CompileAssembly/Debug/Linux64")]
        public static void CompileDllLinuxDebug()
        {
            var target = BuildTarget.StandaloneLinux64;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }

        [MenuItem("meltyStars/CompileAssembly/Debug/OSX")]
        public static void CompileDllOSXDebug()
        {
            var target = BuildTarget.StandaloneOSX;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }

        [MenuItem("meltyStars/CompileAssembly/Debug/Android")]
        public static void CompileDllAndroidDebug()
        {
            var target = BuildTarget.Android;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }

        [MenuItem("meltyStars/CompileAssembly/Debug/IOS")]
        public static void CompileDllIOSDebug()
        {
            var target = BuildTarget.iOS;
            CompileAssembly(GetDllBuildOutputDirByTarget(target), target, CodeOptimization.Debug);
        }
        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <param name="buildPath"></param>
        /// <param name="target"></param>
        private static void CompileAssembly(string buildPath, BuildTarget target, CodeOptimization codeOptimization)
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

            List<string> allScripts = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(HotfixCodePath);
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.cs", SearchOption.AllDirectories);
            foreach (var script in fileInfos)
            {
                allScripts.Add(script.FullName);
            }
            string dllPath = Path.Combine(GetDllBuildOutputDirByTarget(target), "Hotfix.dll");
            MSLogger.LogError(dllPath);
            CreateDirIfNotExists(GetDllBuildOutputDirByTarget(target));

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, allScripts.ToArray());
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(target);

            //assemblyBuilder.additionalReferences = null;
            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;
            assemblyBuilder.buildTarget = target;
            assemblyBuilder.buildStarted  += path => MSLogger.LogInfo($"Start Building Assembly : {path}......");
            assemblyBuilder.buildFinished += (path, complierMessages) => 
            {
                var warnings = complierMessages.Where(message => message.type == CompilerMessageType.Warning).ToList();
                var errors = complierMessages.Where(message => message.type == CompilerMessageType.Error).ToList();
                warnings.ForEach(warning => MSLogger.LogWarning(warning.message));
                errors.ForEach(error => MSLogger.LogError(error.message));
            };
            if (!assemblyBuilder.Build())
            {
                MSLogger.LogError($"Build Assembly Failed : {assemblyBuilder.assemblyPath}");
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
