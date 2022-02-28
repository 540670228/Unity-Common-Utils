using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    /// <summary>
    /// �����ṩ���ù���Transform����Ĺ���
    /// </summary>
    public static class TransFormHelper
    {
        /// <summary>
        /// δ֪�㼶�����Һ��ָ�����Ƶı任���
        /// </summary>
        /// <param name="parent">��ǰ�任���</param>
        /// <param name="childName">�������������</param>
        /// <returns>����Ŀ������任���</returns>
        public static Transform FindChildByName(this Transform parent, string childName)
        {
            /* ��չ���� ������*/
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
