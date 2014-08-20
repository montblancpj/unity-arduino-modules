using UnityEngine;
using System.Collections.Generic;

public class SerialTestClient : MonoBehaviour {
    private ServoController   m_servoController;
    private LEDController    m_ledController;
    private SolenoidController m_solenoidController;

    public int m_servoValue = 0;
    private bool ledOn = false;
    private bool solenoidOn = false;

    // Use this for initialization
    void Start () {
        m_servoController = ServoController.Instance;
        m_ledController = LEDController.Instance;
        m_solenoidController = SolenoidController.Instance;
        SerialHandler.OnTactSwitchDataReceived += onTactSwitchDataReceived;
    }

    // Update is called once per frame
    void Update () {
    }

    void onTactSwitchDataReceived(bool isOn) {
        Debug.Log ("TactSwitch Status: " + isOn);
    }

    void OnGUI() {
        // Servo Update
        GUI.Label(new Rect(10, 10, 100, 20), "Servo");
        var servoRect = new Rect(20, 10, 50, 40);
        var val = (int)GUI.HorizontalSlider(new Rect(10, 30, 200, 40), m_servoValue, 0, 100);
        if(val != m_servoValue) {
            m_servoValue = val;
            m_servoController.SetY((int)(float)m_servoValue * 0.01f);
        }

        // LED Update
        var ledRect = new Rect(10, 70, 50, 50);
        var tmp = GUI.Toggle(ledRect, ledOn, "LED");
        if(ledOn != tmp) {
            Debug.Log(tmp);
            ledOn = tmp;
            m_ledController.SetLed(ledOn);
        }

        // Solenoid Update
        var solenoidRect = new Rect(10, 120, 100, 50);
        var tmpSolenoid = GUI.Toggle(solenoidRect, solenoidOn, "Solenoid");
        if(solenoidOn != tmpSolenoid) {
            Debug.Log(tmpSolenoid);
            solenoidOn = tmpSolenoid;
            m_solenoidController.Push(solenoidOn);
        }
    }
}
