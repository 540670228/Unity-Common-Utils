using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPGDemo.Character
{
    ///<summary>
    ///�����¼���
    ///<summary>
    public class AnimationEventBehaviour : MonoBehaviour
    {
        //�߻����� Ϊ����Ƭ������¼���ָ��OnCancelAnim��OnMeleeAttack


        //���� �ڽű��в��Ŷ�������������Ҫִ�е��߼���ע��attackHandler�¼�

        //�����������߼����¼����ȴ�������Ϊ��ע�᷽�� �� ȡ����������ͨ��ֱ��ʵ�ּ���

        private Animator anim;

        //����ί������
        public delegate void OnAttackHandler();


        //�����¼�
        public event OnAttackHandler OnAttack;

        private void Start()
        {
            anim = GetComponent<Animator>();
            
        }

        //Unity�������
        private void OnMeleeAttack()
        {
            /*if (OnAttack != null) OnAttack();
            else return;*/
            if (OnAttack != null) OnAttack();
            
        }

        //Unity�������
        /// <summary>
        /// ���ж�������ʱ����������¼�
        /// </summary>
        /// <param name="animName">���ŵĶ�������</param>
        public void CancelAnim(string animName)
        {
            //�����¼�
            anim.SetBool(animName, false); 
        }


    }

}
