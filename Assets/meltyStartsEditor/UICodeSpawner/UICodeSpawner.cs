using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using UnityEditor;

namespace MeltyStars
{
    public class UICodeSpawner
    {
        public static void SpawnUICode(GameObject gameObject, string namespaceName, string uiPath, bool spawnForWinodw, bool spawnForWindowView)
        {
            string windowName = gameObject.name;
            string directoryPath = $"{Application.dataPath}/{uiPath}/{windowName}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (spawnForWinodw)
                SpawnUICodeWindow(windowName, namespaceName, directoryPath);
            if (spawnForWindowView)
                SpawnUICodeWindowView(gameObject, windowName, namespaceName, directoryPath);
            AssetDatabase.Refresh();
        }
        private static void SpawnUICodeWindow(string windowName, string namespaceName, string directoryPath)
        {
            string filePath = $"{directoryPath}/{windowName}.cs";
            string viewName = $"{windowName}View";
            string originStr = Resources.Load<TextAsset>("UIWindowTemplate").text;
            string replacedStr = originStr.Replace("#NameSpaceName#", namespaceName)
                                          .Replace("#ClassName#", windowName)
                                          .Replace("#UIWindowAssetName#", windowName)
                                          .Replace("#UIWindowViewName#", viewName);
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.Write(replacedStr);
                sw.Flush();
            }
        }
        private static void SpawnUICodeWindowView(GameObject gameObject, string windowName, string namespaceName, string directoryPath)
        {
            string filePath = $"{directoryPath}/{windowName}View.cs";
            string viewName = $"{windowName}View";
            string originStr = Resources.Load<TextAsset>("UIWindowViewTemplate").text;
            string replacedStr = originStr.Replace("#NameSpaceName#", namespaceName)
                                          .Replace("#ClassName#", viewName);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            var uiBehaviours = GetUIBehaviours(gameObject);
            foreach (var pathKV in uiBehaviours)
            {
                foreach (var tuple in pathKV.Value)
                {
                    string propertyName = $"{tuple.Item1}_{tuple.Item2.ToString().Substring(15)}";
                    stringBuilder.AppendLine($"        public {tuple.Item2.ToString()} {propertyName}");
                    stringBuilder.AppendLine("        {");
                    stringBuilder.AppendLine("            get");
                    stringBuilder.AppendLine("            {");
                    string privateName = $"m_{propertyName.ToLower()}";
                    stringBuilder.AppendLine($"                if(!{privateName})");
                    stringBuilder.AppendLine($"                    {privateName} = uiTransform?.Find(\"{pathKV.Key}\").GetComponent<{tuple.Item2.ToString()}>();");
                    stringBuilder.AppendLine($"                return {privateName};");
                    stringBuilder.AppendLine("            }");
                    stringBuilder.AppendLine("        }");
                }
            }
            stringBuilder.AppendLine();
            foreach (var pathKV in uiBehaviours)
            {
                foreach (var tuple in pathKV.Value)
                {
                    string propertyName = $"{tuple.Item1}_{tuple.Item2.ToString().Substring(15)}";
                    string privateName = $"m_{propertyName.ToLower()}";
                    stringBuilder.AppendLine($"        private {tuple.Item2.ToString()} {privateName};");
                }
            }
            replacedStr = replacedStr.Replace("#BindCode#", stringBuilder.ToString());
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.Write(replacedStr);
                sw.Flush();
            }
        }
        private static Dictionary<string, List<Tuple<string, Type>>> GetUIBehaviours(GameObject gameObject)
        {
            Dictionary<string, List<Tuple<string, Type>>> uiBehaviours = new Dictionary<string, List<Tuple<string, Type>>>();
            GetUIBehavioursCore(gameObject.transform, "", true, ref uiBehaviours);
            return uiBehaviours;
        }
        private static void GetUIBehavioursCore(Transform root, string path, bool isOrigin, ref Dictionary<string, List<Tuple<string, Type>>> uiBehaviours)
        {
            if (!isOrigin)
                path += $"{root.name}";
            if (root.name.StartsWith("MS"))
            {
                var behaviours = root.GetComponents<UnityEngine.EventSystems.UIBehaviour>();
                if (behaviours.Length != 0)
                {
                    var behavioursList = new List<Tuple<string, Type>>();
                    foreach (var uibehaviour in behaviours)
                    {
                        behavioursList.Add(new Tuple<string, Type>(uibehaviour.name, uibehaviour.GetType()));
                    }
                    uiBehaviours.Add(path.ToString(), behavioursList);
                }
            }
            for (int i = 0; i < root.transform.childCount; i++)
            {
                GetUIBehavioursCore(root.transform.GetChild(i), isOrigin ? path : path + "/", false, ref uiBehaviours);
            }
        }
    }
}
