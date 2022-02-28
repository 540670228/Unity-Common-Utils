using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 单例类懒加载
    /// </summary>
    public class SingletonLazy<T> where T:SingletonLazy<T>
    {

        private static T instance;
        private static object locker = new object();
        public static T Instance { get
            {
                //双重检查 避免多次访问线程锁和多线程访问冲突问题
                if(instance == null)
                {
                    lock(locker)
                    {
                        if(instance==null)
                        {
                            instance = Activator.CreateInstance(typeof(T), true) as T;
                            instance.Init(); //保证只调用一次
                        }
                    }
                }
                return instance;

            } }

        /// <summary>
        /// 可选初始化函数
        /// </summary>
        public virtual void Init()
        {

        }



    }
}
