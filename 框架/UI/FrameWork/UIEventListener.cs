using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// UI事件监听器类 ：管理所有UGUI事件，提供事件参数类
///                 附加到需要交互的UI元素上，用于监听用户的操作。
///                 类似 EventTrigger
/// </summary>
public class UIEventListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    //定义委托数据类型
    public delegate void PointerEventHandler(PointerEventData eventData);
    private Dictionary<string, UIEventListener> uiEventDic; //储存已获取的UIEventListener类

    //声明事件
    public event PointerEventHandler PointerClick;
    public event PointerEventHandler PointerDown;
    public event PointerEventHandler PointerUp;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(PointerClick != null)
            PointerClick(eventData);
    }

    //实现UI的所有交互事件，提供事件参数类（可以得知是谁触发的）
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown != null)
            PointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUp != null)
            PointerUp(eventData);
    }

    /// <summary>
    /// 获取UI组件上的UIEventListener，若获取不到则为其添加
    /// </summary>
    /// <param name="uiTF">目标UI</param>
    /// <returns>UIEventListener对象</returns>
    public static UIEventListener GetUIEventListener(Transform uiTF)
    {
        

        UIEventListener eventListener = uiTF.GetComponent<UIEventListener>();
        //若找不到组件，则创建并附加给指定UI对象
        if (eventListener == null) eventListener = uiTF.gameObject.AddComponent<UIEventListener>();

        return eventListener;
    }
}
