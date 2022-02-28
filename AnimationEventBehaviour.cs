using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGDemo.Character
{
    ///<summary>
    ///动画事件类
    ///<summary>
    public class AnimationEventBehaviour : MonoBehaviour
    {
        //策划？： 为动画片段添加事件，指向OnCancelAnim、OnMeleeAttack


        //程序： 在脚本中播放动画，动画中需要执行的逻辑，注册attackHandler事件

        //攻击等其他逻辑用事件，等待其他类为其注册方法 ， 取消动画函数通用直接实现即可

        private Animator anim;

        //定义委托类型
        public delegate void OnAttackHandler();


        //定义事件
        public event OnAttackHandler OnAttack;

        private void Start()
        {
            anim = GetComponent<Animator>();
            
        }

        //Unity引擎调用
        private void OnMeleeAttack()
        {
            /*if (OnAttack != null) OnAttack();
            else return;*/
            if (OnAttack != null) OnAttack();
            
        }

        //Unity引擎调用
        /// <summary>
        /// 所有动画结束时都挂了这个事件
        /// </summary>
        /// <param name="animName">播放的动画名称</param>
        public void CancelAnim(string animName)
        {
            //结束事件
            anim.SetBool(animName, false); 
        }


    }

}
