using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D.Unity3dTools
{
    /// <summary>
    /// 遵守配置表规则前提下，用于类型转换的工具类
    /// </summary>
    public static class ParseUtil
    {
        /// <summary>
        /// 将配置内容转换为整型
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int ToInt(this object self)
        {
            string temp = self.ToString().Split('.')[0];
            return int.Parse(temp);
        }
        /// <summary>
        /// 将配置内容转换为整型列表
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<int> ToIntArray(this object self)
        {
            List<int> array = new List<int>();
            string[] temp = self.ToString().Split(',');
            for (int i = 0; i < temp.Length; i++)
            {
                if (string.IsNullOrEmpty(temp[i])) continue;
                array.Add(int.Parse(temp[i]));
            }
            return array;
        }
        /// <summary>
        /// 将配置内容转换为值为整型的二维列表
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static List<List<int>> ToIntArrays(this object self)
        {
            List<List<int>> arrays = new List<List<int>>();
            string[] temp1 = self.ToString().Split(';');
            for (int i = 0; i < temp1.Length; i++)
            {
                string[] temp2 = temp1[i].Split(',');
                arrays.Add(new List<int>());
                for (int j = 0; j < temp2.Length; j++)
                {
                    int value = 0;
                    if (string.IsNullOrEmpty(temp2[j])) value = 0;
                    arrays[i].Add(value);
                }
            }
            return arrays;
        }
        /// <summary>
        /// 讲配置内容转换为key和value都是整型的字典
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Dictionary<int, int> ToDictionary(this object self)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string[] temp1 = self.ToString().Split(',');
            for (int i = 0; i < temp1.Length; i++)
            {
                string[] temp2 = temp1[i].Split('=');
                for (int j = 0; j < temp2.Length; j++)
                {
                    if (string.IsNullOrEmpty(temp2[j])) continue;
                    dic[i] = int.Parse(temp2[j]);
                }
            }
            return dic;
        }
        /// <summary>
        /// 转换数据类型时，按照配置的类型选择转换所需要调用的函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMethodName(this string type)
        {
            if (type == "List<int>") return "ToIntArray";
            if (type == "List<List<int>>") return "ToIntArrays";
            if (type == "int") return "ToInt";
            if (type == "Dictionary<int,int>") return "ToDictionary";
            return "ToString";
        }
    }
}
