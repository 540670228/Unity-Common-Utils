using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IResetable
    {
        //用以重置对象池中的对象
        void onReset();
    }


    ///<summary>
    ///对象池--唯一
    ///<summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> objCache; //创建对象缓存字典

        protected override void Init()
        {
            base.Init();
            //初始化字典
            objCache = new Dictionary<string, List<GameObject>>();
        }


        /// <summary>
        /// 创建对象（从对象池创建/读取对象）
        /// </summary>
        /// <param name="key">类别---自行定义</param>
        /// <param name="prefab">需要创建实例的预制件</param>
        /// <param name="pos">创建位置</param>
        /// <param name="rotate">创建角度</param>
        /// <returns></returns>
        public GameObject CreateObject(string key,GameObject prefab,Vector3 pos , Quaternion rotate)
        {
            GameObject go;
            go = FindUsableObject(key);  //查找是否有可用的对象 若无则返回null
            //若没有查找到--没有键/没有空闲对象
            if(go == null)
            {
                //添加对象
                go = AddObject(key, prefab);
            }   
            
            UseObject(go,pos,rotate); //使用对象 设置位置旋转和启用

            return go;
            
        }
        /// <summary>
        /// 查找是否有可用的对象
        /// </summary>
        private GameObject FindUsableObject(string key)
        {

            //List的Find也是委托，类似于ArrayHelper自己定义的，可同样使用
            if (objCache.ContainsKey(key))
            {
                //返回有被禁用的物体，若无则返回null
                return objCache[key].Find(go => !go.activeInHierarchy);
            }
            else return null;
            
        }
        /// <summary>
        /// 向对象池中添加对象
        /// </summary>
        /// <param name="key">类别</param>
        /// <param name="prefab">预制件</param>
        /// <returns>返回实例对象</returns>
        private GameObject AddObject(string key,GameObject prefab)
        {
            //创建预制件的实例对象
            GameObject go = Instantiate(prefab);
            //如果缺少键则创建键
            if (!objCache.ContainsKey(key)) objCache.Add(key, new List<GameObject>());
            //向对象池中添加对象
            objCache[key].Add(go);

            return go;
        }

        /// <summary>
        /// 使用对象（配置对象的一些位置和旋转）
        /// </summary>
        /// <param name="go">实例对象</param>
        /// <param name="pos">位置</param>
        /// <param name="rotate">旋转</param>
        private void UseObject(GameObject go,Vector3 pos,Quaternion rotate)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            //遍历执行所有需要被重置的逻辑（实现了IResetable接口的脚本）
            foreach (var item in go.GetComponents<IResetable>())
                item.onReset();
            
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">实例对象</param>
        /// <param name="delay">延迟时间(默认参数)</param>
        public void CollectObject(GameObject go,float delay=0)
        {
            //延迟调用
            StartCoroutine(CollectObjectDelay(go,delay));
        }

        private IEnumerator CollectObjectDelay(GameObject go,float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }

        /// <summary>
        /// 清楚指定类别的对象
        /// </summary>
        /// <param name="key">类别</param>
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
        /// 清除对象池中所有内容
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

            //foreach只允许读元素，不允许修改删除等
            //可以通过new List<string>(objCahce.Keys)解决
            foreach(var item in new List<string>(objCache.Keys))
            {
                //原理，遍历的List元素，删除的字典 遍历A删B 不允许遍历B删B
                Clear(item);
            }
        }

    }

}
