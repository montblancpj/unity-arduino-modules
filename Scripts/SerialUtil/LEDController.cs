using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LEDController {
    static private LEDController s_instance = null;
    private const string LedHeader = "l";

    private SerialHandler m_handler = null;

    private LEDController()
    {
        var obj = GameObject.Find("Arduino");
        if(obj) {
            m_handler = obj.GetComponent<SerialHandler>();
        } else {
            Debug.LogError("Arduino Serial Object is not found in this scene.");
        }
    }

    static public LEDController Instance 
    {
        get 
        {
            if(s_instance == null)
            {
                s_instance = new LEDController();
            }
            return s_instance;
        }
    }

    public void SetLed(bool val) {
        string bright = val ? "254" : "0";
        var data = m_handler.CreateSendData<string>(LedHeader, bright);
        m_handler.SendData(data);	
    }
}
