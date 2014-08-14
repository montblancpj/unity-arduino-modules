using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SolenoidController {
    static private SolenoidController s_instance = null;
    private const string SolenoidHeader = "p";

    private SerialHandler m_handler = null;

    private SolenoidController()
    {
        var obj = GameObject.Find("Arduino");
        if(obj) {
            m_handler = obj.GetComponent<SerialHandler>();
        } else {
            Debug.LogError("Arduino Serial Object is not found in this scene.");
        }
    }

    static public SolenoidController Instance 
    {
        get 
        {
            if(s_instance == null)
            {
                s_instance = new SolenoidController();
            }
            return s_instance;
        }
    }

    public void Push(bool val) {
        var value = val ? 1 : 0;
        var data = m_handler.CreateSendData<int>(SolenoidHeader, value);
        Debug.Log("Push " + data);
        m_handler.SendData(data);	
    }
}
