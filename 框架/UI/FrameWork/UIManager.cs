using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace VR.UGUI.FrameWork
{
    /// <summary>
    /// 负责统一管理（记录，查找，增加）所有窗口
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        //Manager单例类 继承MonoSingleton

        //记录采用类型字符串记录所有窗口，父类引用统一概念
        private Dictionary<string, UIWindow> uiWindowDic;


        /// <summary>
        /// 单例类提供的初始化方法（可防止Awake顺序的冲突）
        /// </summary>
        protected override void Init()
        {
            base.Init();
            //初始化字典
            uiWindowDic = new Dictionary<string, UIWindow>();
            //记录并隐藏所有窗口--后续根据游戏逻辑启用窗口
            RecordUIWindows();
            
        }

        /// <summary>
        /// 记录所有窗口
        /// </summary>
        private void RecordUIWindows()
        {
            //获取所有窗口(查找父类型以查找所有子类对象，概念统一）
            UIWindow[] uiWindows = FindObjectsOfType<UIWindow>();
            foreach (UIWindow uiWindow in uiWindows)
            {
                AddUIWindow(uiWindow);
                //隐藏窗口
                uiWindow.SetVisible(false);
            }
        }

        /// <summary>
        /// 增加新窗口，用于记录动态创建的窗口
        /// </summary>
        /// <param name="uiWindow">窗口对象</param>
        public void AddUIWindow(UIWindow uiWindow)
        {
            //根据子类窗口对象获取的子类类名
            string windowTypeName = uiWindow.GetType().Name;
            
            //记录到字典中
            uiWindowDic.Add(windowTypeName, uiWindow);
        }

        /// <summary>
        /// 查找获取窗口对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>窗口对象</returns>
        public T GetUIWindow<T>() where T:class
        {
            //获取窗口对象的类名
            string windowTypeName = typeof(T).Name;
            //找不到返回null
            if (!uiWindowDic.ContainsKey(windowTypeName)) return null;
            //返回窗口对象，注意转型和约束
            return uiWindowDic[windowTypeName] as T;
        }
    }
}
