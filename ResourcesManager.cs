using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    ///<summary>
    ///资源加载管理类
    ///<summary>
    public class ResourcesManager
    {
        //负责储存预制件的名字和路径映射
        private static Dictionary<string, string> prefabConfigMap;

        //静态构造函数
        //作用：初始化类的静态数据成员
        //时机：类被加载（第一次调用）时调用一次
        static ResourcesManager()
        {
            //加载文件
            string fileContent = ConfigReader.GetConfigFile("ConfigMap.txt");

            prefabConfigMap = new Dictionary<string, string>();

            //解析文件（string ----> prefabConfigMap)
            ConfigReader.Reader(fileContent, BuildMap);
        }

        /// <summary>
        /// 负责处理解析每行字符串的功能
        /// </summary>
        /// <param name="line">每行字符串</param>
        private static void BuildMap(string line)
        {
            string[] keyValue = line.Split('=');
            prefabConfigMap.Add(keyValue[0], keyValue[1]);

        }





        /// <summary>
        /// 加载预制件
        /// </summary>
        /// <typeparam name="T">加载资源类型</typeparam>
        /// <param name="prefabName">预制件名称</param>
        /// <returns></returns>
        public static T Load<T>(string prefabName)where T:Object
        {
            //从字典中获取路径加载预制件
            if (prefabConfigMap.ContainsKey(prefabName))
            {
                return Resources.Load<T>(prefabConfigMap[prefabName]);
            }
            else return default(T);
        }


        
    }

}
