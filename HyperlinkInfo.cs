using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace D.Unity3dTools
{
    /// <summary>
    /// 超链接内容模块
    /// </summary>
    public class HyperlinkInfo
    {
        public int startIndex;
        public int endIndex;
        public string showInfo;
        public string hyperInfo;
        public readonly List<Rect> boxes = new List<Rect>();
    }
}
