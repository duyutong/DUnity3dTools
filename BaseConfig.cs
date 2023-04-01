using System.Collections.Generic;

namespace D.Unity3dTools
{
    /// <summary>
    /// 配置表基类
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; protected set; }
        public BaseConfig() { }
        public BaseConfig(Dictionary<string, object> _dataDic) { }
    }
}
