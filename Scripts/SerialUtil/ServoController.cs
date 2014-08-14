using UnityEngine;
using System.Collections;

public class ServoController {
    static private ServoController s_instance = null;

    private const string ServoHeader = "s";
    private const int MinDeg = 0;
    private const int MaxDeg = 62;

    private SerialHandler m_handler = null;
    private float m_lastDeg = -1;

    private ServoController()
    {
        var obj = GameObject.Find("Arduino");
        if(obj) {
            m_handler = obj.GetComponent<SerialHandler>();
        } else {
            Debug.LogError("Arduino Serial Object is not found in this scene.");
        }
    }

    static public ServoController Instance 
    {
        get 
        {
            if(s_instance == null)
            {
                s_instance = new ServoController();
            }
            return s_instance;
        }
    }

    /// <summary>
    /// Sets the y.
    /// </summary>
    /// <param name="y">The y coordinate. 0.0 - 1.0</param>
    public void SetY(float y)
    {
        int deg = (int)(MaxDeg - (MaxDeg - MinDeg) * y);
        if(m_lastDeg != deg) {
            if(m_handler != null) {
                var data = m_handler.CreateSendData<int>(ServoHeader, deg);
                m_handler.SendData(data);
            }
            m_lastDeg = deg;
        }
    }

    [System.Obsolete("Use SetY")]
    public void Down()
    {
        if(m_handler != null) {
            var data = m_handler.CreateSendData<int>(ServoHeader, MinDeg);
            m_handler.SendData(data);
        }
    }

    [System.Obsolete("Use SetY")]
    public void Up ()
    {
        if(m_handler != null) {
            var data = m_handler.CreateSendData<int>(ServoHeader, MaxDeg);
            m_handler.SendData(data);
        }
    }
}
