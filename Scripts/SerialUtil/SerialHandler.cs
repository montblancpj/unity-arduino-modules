using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class SerialHandler : MonoBehaviour
{
    private SerialPort m_serial;

    void Awake()
    {
        Open();
    }

    void OnDestroy()
    {
        Close();
    }

    public void SendData(string data)
    {
		if (!m_serial.IsOpen) return;

        try
        {
            m_serial.Write(data);
        }
        catch (System.IO.IOException)
        {
            IOErrorHandler();
        }
    }

    void Open()
    {
        var settingJson = Utilities.ReadJSON("Arduino/setting.json");
        string port = settingJson["serial"]["name"].Value;
        int baudrate = settingJson["serial"]["baudrate"].AsInt;
        m_serial = new SerialPort(port, baudrate, Parity.None, 8, StopBits.One);
        if (m_serial != null) {
            if(m_serial.IsOpen) {
                Debug.LogError("Failed to open Serial Port, already open!");
                m_serial.Close();
            } else {
                try	{
                    Debug.Log("Try to open serial, " + port + ":" + baudrate);
                    m_serial.Open();
                    m_serial.DtrEnable = true;
                    m_serial.RtsEnable = true;
                    m_serial.ReadTimeout = 50;
                } catch(System.IO.IOException) {
                    IOErrorHandler();
                }
            }
        }
    }

    void Close()
    {
        if (m_serial != null) {
            m_serial.Close();
        }
    }

    public string CreateSendData<T>(string header, T data)
    {
        return header + data.ToString() + "\0";
    }

    void IOErrorHandler()
    {
        Debug.LogError("IOException!!!!");
        Close();
    }
}

