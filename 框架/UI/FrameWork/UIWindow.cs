using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Common;

namespace VR.UGUI.FrameWork
{
    /// <summary>
    /// UI窗口(Canvas)基类:定义所有窗口共有成员
    /// </summary>
    public class UIWindow : MonoBehaviour
    {
        private CanvasGroup canvasGroup; //画布组，设置透明通道

        private VRTK_UICanvas vrtkUICanvas; //VRTK的画布管理类

        private Dictionary<string, UIEventListener> uiEventDic; //储存已获取的UIEventListener类

        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            
            vrtkUICanvas = GetComponent<VRTK_UICanvas>();

            uiEventDic = new Dictionary<string, UIEventListener>(); //初始化字典
        }




        /// <summary>
        /// 设置窗口可见性
        /// </summary>
        /// <param name="state">显隐状态</param>
        /// <param name="delay">延迟时间/s(默认参数)</param>
        public void SetVisible(bool state,float delay = 0)
        {
            //Unity建议使用透明通道隐藏（比禁用物体更省性能？）

            //CanvasGroup---设置画布组的透明通道
            /*VRTK UICanvas --- VR中特有，除透明外还有碰撞器刚体等需要处理，
             * 禁用此组件可将其增添的这些组件一并删除*/


            //delay > 0 延迟调用
            //delay ==0 “立即”调用 实则延迟一帧
            StartCoroutine(SetVisibleDelay(state, delay));
            

        }

        private IEnumerator SetVisibleDelay(bool state,float delay)
        {
            //延迟调用
            yield return new WaitForSeconds(delay);
            //设置透明通道值0-1,注意要提前加上Canvas Group
            canvasGroup.alpha = state ? 1 : 0;
            //设置vrtk组件的启用禁用
            vrtkUICanvas.enabled = state;
        }

        /// <summary>
        /// 寻找需要交互UI的UIEventListener
        /// </summary>
        /// <param name="uiName">UI名字</param>
        /// <returns></returns>
        public UIEventListener GetUIEventListener(string uiName)
        {
            //防止多次查找，用字典记录

            //如果没有则查找，有则直接返回
            if (!uiEventDic.ContainsKey(uiName))
            {
                Transform uiTF = transform.FindChildByName(uiName);


                //为了防止UI物体上没有监听器类，应在监听器类中加以处理
                UIEventListener uiEventListener = UIEventListener.GetUIEventListener(uiTF);
                uiEventDic.Add(uiName, uiEventListener);
                return uiEventListener;
            }
            else return uiEventDic[uiName];

            
        }
    }
}
