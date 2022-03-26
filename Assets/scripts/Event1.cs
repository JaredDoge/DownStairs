using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Event1 : MonoBehaviour
    {
    	//delegate
        public delegate void myDelegate(int num);
        public myDelegate m_delegate;
        //event
        public event myDelegate m_event;
        //EventHandler
        public event EventHandler m_EventHandle;
        //Action
        public event Action<int> m_action;

        void Start()
        {
            m_delegate += MyEventFun;
            m_delegate(1);

            m_delegate = (d) =>{Debug.Log("m_delegate : " + d);};
            m_delegate(2);

            m_event += MyEventFun;
            m_event(3);

			//發佈者中可直接=
            m_event = (d) =>{Debug.Log("m_event : " + d);};
            m_event(4);

            m_EventHandle += MyEventFun;
            m_EventHandle(5, new EventArgs());

            m_EventHandle += (o, e) =>{ Debug.Log("m_EventHandle: " + Convert.ToInt32(o) + "\t " + e.ToString());};
            m_EventHandle(6, new EventArgs());
            m_action += MyEventFun;
            m_action(7);

            m_action = (d) =>{Debug.Log("m_action : " + d);};
            m_action(8);
        }

        public void MyEventFun(int num)
        {
            Debug.Log("my func1: " + num);
        }

        public void MyEventFun(object sender, EventArgs e)
        {
            Debug.Log("my func2: " + Convert.ToInt32(sender) + "\t " + e.ToString());
        }
    }