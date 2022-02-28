using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// AB��������
    /// </summary>
    public class ABManager : MonoSingleton<ABManager>
    {
        //AB������---���AB���޷��ظ����ص����� Ҳ���������Ч�ʡ�
        private Dictionary<string, AssetBundle> abCache;

        private AssetBundle mainAB = null; //����������¼

        private AssetBundleManifest mainManifest = null; //�����������ļ�---���Ի�ȡ������

        //����ƽ̨�µĻ���·��
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
        //����ƽ̨�µ���������
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
            //��ʼ���ֵ�
            abCache = new Dictionary<string, AssetBundle>();
        }

        #region ͬ�����ص���������
        /// <summary>
        /// ͬ��������Դ---���ͼ���
        /// </summary>
        /// <param name="abName">ab��������</param>
        /// <param name="resName">��Դ����</param>
        public T LoadResource<T>(string abName,string resName)where T:Object
        {
            //����Ŀ���
            AssetBundle ab = LoadABPackage(abName);

            //������Դ
            return ab.LoadAsset<T>(resName);
        }

        public Object LoadResource(string abName,string resName)
        {
            //����Ŀ���
            AssetBundle ab = LoadABPackage(abName);

            //������Դ
            return ab.LoadAsset(resName);
        }

        public Object LoadResource(string abName, string resName,System.Type type)
        {
            //����Ŀ���
            AssetBundle ab = LoadABPackage(abName);

            //������Դ
            return ab.LoadAsset(resName,type);
        }

        #endregion
        private AssetBundle LoadABPackage(string abName)
        {
            AssetBundle ab;
            //����ab�������ǻ�ȡ��������
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(basePath + mainABName);
                mainManifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //��ȡ����������Ϣ
            string[] dependencies = mainManifest.GetAllDependencies(abName);
            for (int i = 0; i < dependencies.Length; i++)
            {
                //������ڻ��������
                if (!abCache.ContainsKey(dependencies[i]))
                {
                    ab = AssetBundle.LoadFromFile(basePath + dependencies[i]);
                    abCache.Add(dependencies[i], ab);
                }
            }
            //����Ŀ���
            if (abCache.ContainsKey(abName)) return abCache[abName];
            else
            {
                ab = AssetBundle.LoadFromFile(basePath + abName);
                abCache.Add(abName, ab);
                return ab;
            }


        }

        /// <summary>
        /// �ṩ�첽����----ע�� �������AB����ͬ�����أ�ֻ�Ǽ�����Դ���첽
        /// </summary>
        /// <param name="abName">ab������</param>
        /// <param name="resName">��Դ����</param>
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
            //ί�е��ô����߼�
            finishLoadObjectHandler(abr.asset);
        }
        //����Type�첽������Դ
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
            //ί�е��ô����߼�
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
            //ί�е��ô����߼�
            finishLoadObjectHandler(abr.asset as T);
        }


        //������ж��
        public void UnLoad(string abName)
        {
            if(abCache.ContainsKey(abName))
            {
                abCache[abName].Unload(false);
                abCache.Remove(abName);
            }
        }

        //���а�ж��
        public void UnLoadAll()
        {
            AssetBundle.UnloadAllAssetBundles(false);
            abCache.Clear();
            mainAB = null;
            mainManifest = null;
        }
    }
}