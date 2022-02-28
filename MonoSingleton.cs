using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///�ű�������,����ΪΨһ�ű�����ʵ��
    ///<summary>

    public class MonoSingleton<T> : MonoBehaviour where T:MonoSingleton<T> //ע���Լ��ΪT����Ϊ�䱾�������
    {
        /*
        �����ֱ������ҪΨһ�����Ľű��д���ʵ����Awake��ʼ���Ĺ�����Ҫ���������
        1.�����ظ�
        2.��Awake�����ʼ���������ű���Awake�е�������ܻ�ΪNull���쳣���
         */

        //���1��ʹ�÷��ʹ���ʵ��   ���2��ʹ�ð�����أ����������ű�����ʱ��get�м��أ�

        private static T instance; //����˽�ж����¼ȡֵ����ֻ��ֵһ�α����θ�ֵ

        public static T Instance
        {
            //ʵ�ְ������
            get
            {
                //���Ѿ���ֵ����ֱ�ӷ��ؼ���
                if (instance != null) return instance;

                instance = FindObjectOfType<T>();

                //Ϊ�˷�ֹ�ű���δ�ҵ������ϣ��Ҳ������쳣������������д������������ȥ
                if (instance == null)
                {
                    //���������������ڴ���ʱ���������Ͻű���Awake������T��Awake(T��Awakeʵ�����Ǽ̳еĸ���ģ�
                    //���Դ�ʱ����Ϊinstance��ֵ�������Awake�и�ֵ����ȻҲ���ʼ����������init()
                    /*instance = */
                    new GameObject("Singleton of "+typeof(T)).AddComponent<T>();
                }
                else instance.Init(); //��֤Initִֻ��һ��

                return instance;

            }
        }

        private void Awake()
        {
            //���������ű���Awake�е��ô�ʵ���������Awake�����г�ʼ��instance
            instance = this as T;
            //��ʼ��
            Init();
        }

        //����Գ�Ա���г�ʼ���������Awake���Ի����Null����������������һ��init������������ÿɲ��ã�
        protected virtual void Init()
        {

        }
    }

}
