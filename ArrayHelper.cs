using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///����������
    ///<summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// ������������(��ȣ��ĵ���Ԫ��(�ж��ʱ���ص�һ����
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <param name="array">����</param>
        /// <param name="condition">�ȽϷ�����ί�У�</param>
        /// <returns>����Ŀ�����</returns>
        public static T Find<T>(this T[] array, Func<T,bool> condition)
        {
            for(int i=0;i<array.Length;i++)
            {
                if(condition(array[i]))
                {
                    return array[i];
                }
            }

            return default(T);

        }
        
        /// <summary>
        /// ������������(��ȣ��Ķ��Ԫ��
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <param name="array">����</param>
        /// <param name="condition">�ȽϷ�����ί�У�</param>
        /// <returns>����Ŀ�����</returns>
        public static T[] FindAll<T>(this T[] array, Func<T,bool> condition)
        {
            List<T> list = new List<T>();

            for(int i=0;i<array.Length;i++)
            {
                if(condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }

            return list.ToArray();

        }

        /// <summary>
        /// �����������������������ֵ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <typeparam name="Q">�Ƚ����ݵ���������</typeparam>
        /// <param name="array">����</param>
        /// <param name="condition">�Ƚ����ݷ���</param>
        /// <returns></returns>
        public static T GetMax<T,Q>(this T[] array , Func<T,Q> condition)where Q:IComparable
        {
            //������Ϊ���򷵻�Ĭ�ϣ�null)
            if (array.Length == 0) return default(T);
            T maxT = array[0];
            for(int i=0;i<array.Length;i++)
            {
                if (condition(array[i]).CompareTo(condition(maxT)) > 0)
                    maxT = array[i];
            }

            return maxT;
        }


        /// <summary>
        /// ����������������������Сֵ
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <typeparam name="Q">�Ƚ����ݵ���������</typeparam>
        /// <param name="array">����</param>
        /// <param name="condition">�Ƚ����ݷ���</param>
        /// <returns></returns>
        public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            //������Ϊ���򷵻�Ĭ�ϣ�null)
            if (array.Length == 0) return default(T);
            T minT = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]).CompareTo(condition(minT)) < 0)
                    minT = array[i];
            }

            return minT;
        }


        /// <summary>
        /// ������������
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <typeparam name="Q">�Ƚ�����</typeparam>
        /// <param name="array">����������</param>
        /// <param name="condition">�������ݷ���</param>
        public static void Sort_ascending<T,Q>(this T[] array , Func<T,Q> condition) where Q:IComparable
        {
            //ð������
            for(int i=0;i<array.Length-1;i++)
            {
                for(int j=i+1;j<array.Length;j++)
                {
                    if(condition(array[j]).CompareTo(condition(array[i])) > 0)
                    {
                        T temp = array[i];
                        array[j] = array[j];
                        array[i] = array[j];
                    }
                }
            }
        }


        /// <summary>
        /// ���齵������
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <typeparam name="Q">�Ƚ�����</typeparam>
        /// <param name="array">����������</param>
        /// <param name="condition">�������ݷ���</param>
        public static void Sort_descending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            //ð������
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (condition(array[j]).CompareTo(condition(array[i])) < 0)
                    {
                        T temp = array[i];
                        array[j] = array[j];
                        array[i] = array[j];
                    }
                }
            }
        }


        /// <summary>
        /// ��ÿ��T��ѡ��Q ����Q[]  ---���������е��������л�����е��˵Ķ����ű�
        /// </summary>
        /// <typeparam name="T">Ԫ������</typeparam>
        /// <typeparam name="Q">��Ҫ��ȡ������</typeparam>
        /// <param name="array">ԭ����</param>
        /// <param name="condition">������Ҫ��ȡ������</param>
        /// <returns>��ȡ�����͵�����</returns>
        public static Q[] Select<T,Q>(this T[] array , Func<T,Q> condition)
        {
            Q[] res = new Q[array.Length];

            for(int i=0;i<array.Length;i++)
            {
                res[i] = condition(array[i]);
            }

            return res;
        }
    }

}
