using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    ///<summary>
    ///��Դ���ع�����
    ///<summary>
    public class ResourcesManager
    {
        //���𴢴�Ԥ�Ƽ������ֺ�·��ӳ��
        private static Dictionary<string, string> prefabConfigMap;

        //��̬���캯��
        //���ã���ʼ����ľ�̬���ݳ�Ա
        //ʱ�����౻���أ���һ�ε��ã�ʱ����һ��
        static ResourcesManager()
        {
            //�����ļ�
            string fileContent = ConfigReader.GetConfigFile("ConfigMap.txt");

            prefabConfigMap = new Dictionary<string, string>();

            //�����ļ���string ----> prefabConfigMap)
            ConfigReader.Reader(fileContent, BuildMap);
        }

        /// <summary>
        /// ���������ÿ���ַ����Ĺ���
        /// </summary>
        /// <param name="line">ÿ���ַ���</param>
        private static void BuildMap(string line)
        {
            string[] keyValue = line.Split('=');
            prefabConfigMap.Add(keyValue[0], keyValue[1]);

        }





        /// <summary>
        /// ����Ԥ�Ƽ�
        /// </summary>
        /// <typeparam name="T">������Դ����</typeparam>
        /// <param name="prefabName">Ԥ�Ƽ�����</param>
        /// <returns></returns>
        public static T Load<T>(string prefabName)where T:Object
        {
            //���ֵ��л�ȡ·������Ԥ�Ƽ�
            if (prefabConfigMap.ContainsKey(prefabName))
            {
                return Resources.Load<T>(prefabConfigMap[prefabName]);
            }
            else return default(T);
        }


        
    }

}
