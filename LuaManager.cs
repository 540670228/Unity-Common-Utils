using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Common
{
    /// <summary>
    /// Lua ������
    /// �ṩ Lua������ ��֤Ψһ��
    /// </summary>
    public class LuaManager : SingletonLazy<LuaManager>
    {
        //ִ��Lua���Եĺ��� �ͷ����� ���� �ض���
        private LuaEnv luaEnv;

        //����Lua�еĴ�G��_G
        public LuaTable Global
        {
            get
            {
                return luaEnv.Global;
            }
        }
        public override void Init()
        {
            base.Init();
            luaEnv = new LuaEnv();
            //������ ƽ������AB���鷳
            luaEnv.AddLoader(MyCustomLoader);
            //Lua�ű��ض��� ����AB��
            //luaEnv.AddLoader(CustomABLoader);
        }



        //Lua�ű������AB����,�ټ������е�Lua�ű���Դ��ִ����
        //AB����ҲҪͨ�������ı��ķ�ʽ����׺Ϊ.lua����ʶ�����.txt��
        private byte[] CustomABLoader(ref string filepath)
        {
            //��AB���м���Lua�ļ�
            //����AB���µ�Lua�ļ�--ͨ��AB��������
            TextAsset text = ABManager.Instance.LoadResource<TextAsset>("lua", filepath +".lua");
            if(text == null)
            {
                Debug.Log("�ض���ʧ�� �ļ���Ϊ" + filepath);
                return null;
            }
                
            //����byte����
            return text.bytes;
        }

        private byte[] MyCustomLoader(ref string filepath)
        {
            //ͨ�������е��߼� ȥ ����Lua�ļ�
            //filepath����require�е��ļ���
            //ƴ��һ��Lua�ļ����ڵ�·��
            string path = Application.dataPath + "/Lua/" + filepath + ".lua";

            //����File ��ȡ�ļ�
            //�ж��ļ��Ƿ����
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.Log("�ض���ʧ�� �ļ���Ϊ:" + filepath);
            }
            return null;
        }

        /// <summary>
        /// ִ��Lua����
        /// </summary>
        /// <param name="str">Lua���</param>
        public void DoString(string str)
        {
            if(luaEnv == null)
            {
                Debug.Log("������Ϊ�գ�����");
                return;
            }
            luaEnv.DoString(str);
        }

        public void DoLuaFile(string fileName)
        {
            if (luaEnv == null)
            {
                Debug.Log("������Ϊ�գ�����");
                return;
            }
            string path = string.Format("require('{0}')", fileName);
            luaEnv.DoString(path);
        }
        /// <summary>
        /// �ͷ�����
        /// </summary>
        public void Tick()
        {
            if (luaEnv == null)
            {
                Debug.Log("������Ϊ�գ�����");
                return;
            }
            luaEnv.Tick();
        }
        /// <summary>
        /// ����Lua������
        /// </summary>
        public void Dispose()
        {
            if (luaEnv == null)
            {
                Debug.Log("������Ϊ�գ�����");
                return;
            }
            luaEnv.Dispose();
            luaEnv = null;
        }

    }
}