using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GenrateResConfig : Editor
{
    [MenuItem("Tools/Resources/Generate ResConfig File")]
    public static void Generate()
    {
        List<string> typeList = new List<string>();
        typeList.Add("prefab");
        foreach(string type in typeList)
        {
            string[] mapArr = getMapping(type);
            //3.д���ļ�
            
            if(!Directory.Exists("Assets/StreamingAssets"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets");
            }
            File.AppendAllLines("Assets/StreamingAssets/ConfigMap", mapArr);
        }
        //ˢ��
        AssetDatabase.Refresh();
        
    }

    private static string[] getMapping(string type)
    {
        //������Դ�����ļ�
        //1.����ResourcesĿ¼������Ԥ�Ƽ�������·��
        //����ֵΪGUID ��Դ��� �� ����1 ָ�����ͣ�����2 ����Щ·���²���
        string[] resFiles = AssetDatabase.FindAssets($"t:{type}", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            //2.���ɶ�Ӧ��ϵ  ����=·��
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty)
                .Replace($".{type}", string.Empty);
            resFiles[i] = fileName + "=" + filePath;
        }
        return resFiles;
    }


}