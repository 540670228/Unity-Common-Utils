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
            //3.写入文件
            
            if(!Directory.Exists("Assets/StreamingAssets"))
            {
                Directory.CreateDirectory("Assets/StreamingAssets");
            }
            File.AppendAllLines("Assets/StreamingAssets/ConfigMap", mapArr);
        }
        //刷新
        AssetDatabase.Refresh();
        
    }

    private static string[] getMapping(string type)
    {
        //生成资源配置文件
        //1.查找Resources目录下所有预制件的完整路径
        //返回值为GUID 资源编号 ， 参数1 指明类型，参数2 在哪些路径下查找
        string[] resFiles = AssetDatabase.FindAssets($"t:{type}", new string[] { "Assets/Resources" });
        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            //2.生成对应关系  名称=路径
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            string filePath = resFiles[i].Replace("Assets/Resources/", string.Empty)
                .Replace($".{type}", string.Empty);
            resFiles[i] = fileName + "=" + filePath;
        }
        return resFiles;
    }


}
