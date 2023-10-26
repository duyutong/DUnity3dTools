using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace D.Unity3dTools.EditorTool
{
    /// <summary>
    /// 删除空脚本的组件
    /// </summary>
    public class ScriptComponentRemover
    {
        /// <summary>
        /// 删除空脚本的组件
        /// </summary>
        [MenuItem("Tools/Remove Missing-MonoBehavior Component")]
        static public void RemoveMissComponent()
        {
            string fullPath = Application.dataPath + "/Art/Prefabs";
            fullPath = fullPath.Replace("/", @"\");
            //List<string> pathList = GetAssetsPathByFullPath(fullPath, "*.prefab", SearchOption.AllDirectories);
            string[] pathList = Directory.GetFiles("Assets/", "*.prefab", SearchOption.AllDirectories);
            int counter = 0;
            for (int i = 0, iMax = pathList.Length; i < iMax; i++)
            {
                EditorUtility.DisplayProgressBar("处理进度", string.Format("{0}/{1}", i + 1, iMax), (i + 1f) / iMax);
                if (CheckMissMonoBehavior(pathList[i]))
                    ++counter;
            }
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("处理结果", "完成修改，修改数量 : " + counter, "确定");
            AssetDatabase.Refresh();
        }

        private static Regex regBlock = new Regex("MonoBehaviour");
        /// <summary>
        /// 删除一个Prefab上的空脚本
        /// </summary>
        /// <param name="path">prefab路径 例Assets/Resources/FriendInfo.prefab</param>
        static bool CheckMissMonoBehavior(string path)
        {
            bool isNull = false;
            string textContent = File.ReadAllText(path);

            // 以"---"划分组件
            string[] strArray = textContent.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray.Length; i++)
            {
                string blockStr = strArray[i];
                if (regBlock.IsMatch(blockStr))
                {
                    // 模块是 MonoBehavior
                    //(?<名称>子表达式)  含义:将匹配的子表达式捕获到一个命名组中
                    Match guidMatch = Regex.Match(blockStr, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type: [0-9]}");
                    if (guidMatch.Success)
                    {
                        string guid = guidMatch.Groups["GuidValue"].Value;
                        if (!File.Exists(AssetDatabase.GUIDToAssetPath(guid)))
                        {
                            isNull = true;
                            textContent = DeleteContent(textContent, blockStr);
                        }
                    }

                    Match fileIdMatch = Regex.Match(blockStr, @"m_Script: {fileID: (?<IdValue>\d+)}");
                    if (fileIdMatch.Success)
                    {
                        string idValue = fileIdMatch.Groups["IdValue"].Value;
                        if (idValue.Equals("0"))
                        {
                            isNull = true;
                            textContent = DeleteContent(textContent, blockStr);
                        }
                    }
                }
            }
            if (isNull)
            {
                // 有空脚本 写回prefab
                File.WriteAllText(path, textContent);
            }
            return isNull;
        }

        // 删除操作
        static string DeleteContent(string input, string blockStr)
        {
            input = input.Replace("---" + blockStr, "");
            Match idMatch = Regex.Match(blockStr, "!u!(.*) &(?<idValue>.*?)\n");
            if (idMatch.Success)
            {
                // 获取 MonoBehavior的fileID
                string fileID = idMatch.Groups["idValue"].Value;
                Regex regex = new Regex("  - (.*): {fileID: " + fileID + "}\n");
                input = regex.Replace(input, "");
            }

            return input;
        }
    }
}
