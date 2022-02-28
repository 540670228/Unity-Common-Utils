using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    /// <summary>
    /// 负责提供常用关于Transform组件的功能
    /// </summary>
    public static class TransFormHelper
    {
        /// <summary>
        /// 未知层级，查找后代指定名称的变换组件
        /// </summary>
        /// <param name="parent">当前变换组件</param>
        /// <param name="childName">查找物体的名称</param>
        /// <returns>返回目标物体变换组件</returns>
        public static Transform FindChildByName(this Transform parent, string childName)
        {
            /* 拓展方法 更方便*/
            Transform child = parent.transform.Find(childName);
            if (child != null) return child;

            foreach (Transform childs in parent)
            {
                child = FindChildByName(childs, childName);
                if (child != null) return child;
            }
            return null;
        }

    }
}
