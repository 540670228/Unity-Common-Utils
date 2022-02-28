using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IResetable
    {
        //�������ö�����еĶ���
        void onReset();
    }


    ///<summary>
    ///�����--Ψһ
    ///<summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> objCache; //�������󻺴��ֵ�

        protected override void Init()
        {
            base.Init();
            //��ʼ���ֵ�
            objCache = new Dictionary<string, List<GameObject>>();
        }


        /// <summary>
        /// �������󣨴Ӷ���ش���/��ȡ����
        /// </summary>
        /// <param name="key">���---���ж���</param>
        /// <param name="prefab">��Ҫ����ʵ����Ԥ�Ƽ�</param>
        /// <param name="pos">����λ��</param>
        /// <param name="rotate">�����Ƕ�</param>
        /// <returns></returns>
        public GameObject CreateObject(string key,GameObject prefab,Vector3 pos , Quaternion rotate)
        {
            GameObject go;
            go = FindUsableObject(key);  //�����Ƿ��п��õĶ��� �����򷵻�null
            //��û�в��ҵ�--û�м�/û�п��ж���
            if(go == null)
            {
                //��Ӷ���
                go = AddObject(key, prefab);
            }   
            
            UseObject(go,pos,rotate); //ʹ�ö��� ����λ����ת������

            return go;
            
        }
        /// <summary>
        /// �����Ƿ��п��õĶ���
        /// </summary>
        private GameObject FindUsableObject(string key)
        {

            //List��FindҲ��ί�У�������ArrayHelper�Լ�����ģ���ͬ��ʹ��
            if (objCache.ContainsKey(key))
            {
                //�����б����õ����壬�����򷵻�null
                return objCache[key].Find(go => !go.activeInHierarchy);
            }
            else return null;
            
        }
        /// <summary>
        /// ����������Ӷ���
        /// </summary>
        /// <param name="key">���</param>
        /// <param name="prefab">Ԥ�Ƽ�</param>
        /// <returns>����ʵ������</returns>
        private GameObject AddObject(string key,GameObject prefab)
        {
            //����Ԥ�Ƽ���ʵ������
            GameObject go = Instantiate(prefab);
            //���ȱ�ټ��򴴽���
            if (!objCache.ContainsKey(key)) objCache.Add(key, new List<GameObject>());
            //����������Ӷ���
            objCache[key].Add(go);

            return go;
        }

        /// <summary>
        /// ʹ�ö������ö����һЩλ�ú���ת��
        /// </summary>
        /// <param name="go">ʵ������</param>
        /// <param name="pos">λ��</param>
        /// <param name="rotate">��ת</param>
        private void UseObject(GameObject go,Vector3 pos,Quaternion rotate)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            //����ִ��������Ҫ�����õ��߼���ʵ����IResetable�ӿڵĽű���
            foreach (var item in go.GetComponents<IResetable>())
                item.onReset();
            
        }

        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="go">ʵ������</param>
        /// <param name="delay">�ӳ�ʱ��(Ĭ�ϲ���)</param>
        public void CollectObject(GameObject go,float delay=0)
        {
            //�ӳٵ���
            StartCoroutine(CollectObjectDelay(go,delay));
        }

        private IEnumerator CollectObjectDelay(GameObject go,float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

        /// <summary>
        /// ���ָ�����Ķ���
        /// </summary>
        /// <param name="key">���</param>
        public void Clear(string key)
        {
            //Destroy
            if(objCache.ContainsKey(key))
            {
                foreach (GameObject obj in objCache[key])
                    Destroy(obj);
                objCache.Remove(key);
            }
        }
        /// <summary>
        /// ������������������
        /// </summary>
        public void ClearAll()
        {
            /*
            foreach(var item in objCache)
            {
                foreach (var obj in item.Value)
                    Destroy(obj);
            }
            objCache.Clear();
            */

            //foreachֻ�����Ԫ�أ��������޸�ɾ����
            //����ͨ��new List<string>(objCahce.Keys)���
            foreach(var item in new List<string>(objCache.Keys))
            {
                //ԭ��������ListԪ�أ�ɾ�����ֵ� ����AɾB ���������BɾB
                Clear(item);
            }
        }

    }

}
