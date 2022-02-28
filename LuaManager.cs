using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Common
{
    /// <summary>
    /// Lua 管理器
    /// 提供 Lua解析器 保证唯一性
    /// </summary>
    public class LuaManager : SingletonLazy<LuaManager>
    {
        //执行Lua语言的函数 释放垃圾 销毁 重定向
        private LuaEnv luaEnv;

        //返回Lua中的大G表_G
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
            //测试用 平常不用AB包麻烦
            luaEnv.AddLoader(MyCustomLoader);
            //Lua脚本重定向 采用AB包
            //luaEnv.AddLoader(CustomABLoader);
        }



        //Lua脚本会放在AB包中,再加载其中的Lua脚本资源来执行它
        //AB包中也要通过加载文本的方式（后缀为.lua不会识别还需加.txt）
        private byte[] CustomABLoader(ref string filepath)
        {
            //从AB包中加载Lua文件
            //加载AB包下的Lua文件--通过AB包管理器
            TextAsset text = ABManager.Instance.LoadResource<TextAsset>("lua", filepath +".lua");
            if(text == null)
            {
                Debug.Log("重定向失败 文件名为" + filepath);
                return null;
            }
                
            //加载byte数组
            return text.bytes;
        }

        private byte[] MyCustomLoader(ref string filepath)
        {
            //通过函数中的逻辑 去 加载Lua文件
            //filepath就是require中的文件名
            //拼接一个Lua文件所在的路径
            string path = Application.dataPath + "/Lua/" + filepath + ".lua";

            //利用File 读取文件
            //判断文件是否存在
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                Debug.Log("重定向失败 文件名为:" + filepath);
            }
            return null;
        }

        /// <summary>
        /// 执行Lua语言
        /// </summary>
        /// <param name="str">Lua语句</param>
        public void DoString(string str)
        {
            if(luaEnv == null)
            {
                Debug.Log("解析器为空，请检查");
                return;
            }
            luaEnv.DoString(str);
        }

        public void DoLuaFile(string fileName)
        {
            if (luaEnv == null)
            {
                Debug.Log("解析器为空，请检查");
                return;
            }
            string path = string.Format("require('{0}')", fileName);
            luaEnv.DoString(path);
        }
        /// <summary>
        /// 释放垃圾
        /// </summary>
        public void Tick()
        {
            if (luaEnv == null)
            {
                Debug.Log("解析器为空，请检查");
                return;
            }
            luaEnv.Tick();
        }
        /// <summary>
        /// 销毁Lua解析器
        /// </summary>
        public void Dispose()
        {
            if (luaEnv == null)
            {
                Debug.Log("解析器为空，请检查");
                return;
            }
            luaEnv.Dispose();
            luaEnv = null;
        }

    }
}