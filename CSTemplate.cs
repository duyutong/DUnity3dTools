using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Unity3dTools.EditorTool
{
    /// <summary>
    /// 自动生成cs文件时使用的预制文字
    /// </summary>
    public class CSTemplate
    {
        /// <summary>
        /// json生成的配置表类
        /// </summary>
        public const string classStr =
    @"
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using D.Unity3dTools;
/// <summary>
/// #ClassDes#
/// <summary>
public class #ClassName#:BaseConfig
{
	#ProContext#

    public #ClassName#() { }
    public #ClassName#(Dictionary<string, object> _dataDic)
    {#InitContext#
        id = ID;
    }
} ";
        /// <summary>
        /// 配置表类的属性部分
        /// </summary>
        public const string proStr =
        @"
    /// <summary>
    /// #ProDes#
    /// </summary>
    public #ProType# #ProName# { get; protected set; }";

        /// <summary>
        /// 配置表类的Init函数内容
        /// </summary>
        public const string classInitStr =
            @"
        #ProName# = _dataDic[""#ProName#""].#MethodName#();";
        /// <summary>
        /// 配置表加载类
        /// </summary>
        public const string loaderClassStr =
    @"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;
using D.Unity3dTools.EditorTool;
public class ConfigLoader
{
    public static string jsonPath  
    {
        get 
        {
            PathLibrary pathLibrary = JsonMapper.ToObject<PathLibrary>(""File.ReadAllText(#libraryPath#)"");
            return pathLibrary.jsonPath;
        }
    }
    #LoaderMember#
}";
        /// <summary>
        /// 加载对应配置类方法的预置文字
        /// </summary>
        public const string loaderMember =
        @"
    #region #ClassName#
    private static Dictionary<int, #ClassName#> config#ClassName#Table = new Dictionary<int, #ClassName#>();
    public static #ClassName# Get#ClassName#Config(int _id)
    {
        if (config#ClassName#Table.Count == 0) config#ClassName#Table = Load#ClassName#Config();
        if (!config#ClassName#Table.ContainsKey(_id)) return null;
        return config#ClassName#Table[_id];
    }
    private static Dictionary<int, #ClassName#> Load#ClassName#Config()
    {
        Dictionary<int, #ClassName#> result = new Dictionary<int, #ClassName#>();
        JsonData _data = JsonMapper.ToObject(File.ReadAllText(jsonPath + ""/#ClassName#.json""));
        for (int i = 0; i<_data.Count; i++)
        {
            int index = i;
            Dictionary<string, object> pairs = new Dictionary<string, object>();
            foreach (string key in _data[index].Keys) pairs.Add(key, _data[index][key]);
            #ClassName# confItem = new #ClassName#(pairs);
            result.Add(confItem.id, confItem);
        }
        return result;
    }
    #endregion
";
    }
}
