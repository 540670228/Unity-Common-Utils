using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// AB包管理类
    /// </summary>
    public class ABManager : MonoSingleton<ABManager>
    {
        //AB包缓存---解决AB包无法重复加载的问题 也有利于提高效率。
        private Dictionary<string, AssetBundle> abCache;

        private AssetBundle mainAB = null; //主包单独记录

        private AssetBundleManifest mainManifest = null; //主包中配置文件---用以获取依赖包

        //各个平台下的基础路径
        private string basePath { get
            {

#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
                return Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
                return Application.dataPath + "!/assets/";
#endif
            }
        }
        //各个平台下的主包名称
        private string mainABName
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return "StandaloneWindows";
#elif UNITY_IPHONE
                return "IOS";
#elif UNITY_ANDROID
                return "Android";
#endif
            }
        }

        protected override void Init()
        {
            base.Init();
            //初始化字典
            abCache = new Dictionary<string, AssetBundle>();
        }

        #region 同步加载的三个重载
        /// <summary>
        /// 同步加载资源---泛型加载
        /// </summary>
        /// <param name="abName">ab包的名称</param>
        /// <param name="resName">资源名称</param>
        public T LoadResource<T>(string abName,string resName)where T:Object
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset<T>(resName);
        }

        public Object LoadResource(string abName,string resName)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset(resName);
        }

        public Object LoadResource(string abName, string resName,System.Type type)
        {
            //加载目标包
            AssetBundle ab = LoadABPackage(abName);

            //返回资源
            return ab.LoadAsset(resName,type);
        }

        #endregion
        private AssetBundle LoadABPackage(string abName)
        {
            AssetBundle ab;
            //加载ab包，考虑获取依赖包。
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(basePath + mainABName);
                mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //获取依赖包的信息
            string[] dependencies = mainManifest.GetAllDependencies(abName);
            for (int i = 0; i < dependencies.Length; i++)
            {
                //如果不在缓存则加入
                if (!abCache.ContainsKey(dependencies[i]))
                {
                    ab = AssetBundle.LoadFromFile(basePath + dependencies[i]);
                    abCache.Add(dependencies[i], ab);
                }
            }
            //加载目标包
            if (abCache.ContainsKey(abName)) return abCache[abName];
            else
            {
                ab = AssetBundle.LoadFromFile(basePath + abName);
                abCache.Add(abName, ab);
                return ab;
            }


        }

        /// <summary>
        /// 提供异步加载----注意 这里加载AB包是同步加载，只是加载资源是异步
        /// </summary>
        /// <param name="abName">ab包名称</param>
        /// <param name="resName">资源名称</param>
        public void LoadResourceAsync(string abName,string resName, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes(ab,resName,finishLoadObjectHandler));
        }

        private IEnumerator LoadRes(AssetBundle ab,string resName, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }
        //根据Type异步加载资源
        public void LoadResourceAsync(string abName, string resName,System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes(ab, resName,type, finishLoadObjectHandler));
        }
        private IEnumerator LoadRes(AssetBundle ab, string resName,System.Type type, System.Action<Object> finishLoadObjectHandler)
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync(resName,type);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset);
        }

        public void LoadResourceAsync<T>(string abName, string resName, System.Action<Object> finishLoadObjectHandler)where T:Object
        {
            AssetBundle ab = LoadABPackage(abName);
            StartCoroutine(LoadRes<T>(ab, resName, finishLoadObjectHandler));
        }

        private IEnumerator LoadRes<T>(AssetBundle ab, string resName, System.Action<Object> finishLoadObjectHandler)where T:Object
        {
            if (ab == null) yield break;
            AssetBundleRequest abr = ab.LoadAssetAsync<T>(resName);
            yield return abr;
            //委托调用处理逻辑
            finishLoadObjectHandler(abr.asset as T);
        }


        //单个包卸载
        public void UnLoad(string abName)
        {
            if(abCache.ContainsKey(abName))
            {
                abCache[abName].Unload(false);
                abCache.Remove(abName);
            }
        }

        //所有包卸载
        public void UnLoadAll()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abCache.Clear();
            mainAB = null;
            mainManifest = null;
        }
    }
}