using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    public delegate void TactSwitchDataReceivedEventHandler(bool isOn);
    static public event TactSwitchDataReceivedEventHandler OnTactSwitchDataReceived;

    private SerialPort m_serial;

    private Thread m_thread;
    private bool m_isRunning = false;

    private string m_message;
    private bool m_isNewMessageReceived = false;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        if (m_isNewMessageReceived) {
            if (OnTactSwitchDataReceived != null && OnTactSwitchDataReceived.GetInvocationList().Length > 0) {
                if(m_message.StartsWith("1")) {
                    OnTactSwitchDataReceived(true);
                } else if(m_message.StartsWith("0")) {
                    OnTactSwitchDataReceived(false);
                } else {
                    Debug.LogWarning("Tact Switch Data is something wrong. " + m_message);
                }
            }
        }
    }

    void OnDestroy()
    {
        Close();
    }

    public void SendData(string data)
    {
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
                try  {
                    Debug.Log("Try to open serial, " + port + ":" + baudrate);
                    m_serial.Open();
                    m_serial.DtrEnable = true;
                    m_serial.RtsEnable = true;
                    m_serial.ReadTimeout = 50;

                    m_isRunning = true;
                    m_thread = new Thread(Read);
                    m_thread.Start();
                } catch(System.IO.IOException) {
                    IOErrorHandler();
                } catch(System.Exception e) {
                    IOErrorHandler();
                }
            }
        }

    }

    void Close()
    {
        m_isRunning = false;

        if (m_thread != null && m_thread.IsAlive) {
            m_thread.Join();
        }

        if (m_serial != null) {
            m_serial.Close();
        }
    }

    void Read()
    {
        while (m_isRunning && m_serial != null && m_serial.IsOpen) {
            try {
                if (m_serial.BytesToRead > 0) {
                    m_message = m_serial.ReadLine();
                    m_isNewMessageReceived = true;
                }
            } catch (System.Exception e) {
                Debug.LogWarning(e.Message);
            }
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

