using UnityEngine;
using System.Collections.Generic;

public class SerialTestClient : MonoBehaviour {
	private ServoController   m_servoController;
	private LEDController	  m_ledController;
	private SolenoidController m_solenoidController;
	
	public int m_servoValue = 0;
	private bool servoUp = false;
	private bool ledOn = false;
	private bool solenoidOn = false;

	// Use this for initialization
	void Start () {
		m_servoController = ServoController.Instance;
		m_ledController = LEDController.Instance;
		m_solenoidController = SolenoidController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	}
				
  void OnGUI() {
		// New Servo Update
		var servoRect = new Rect(10, 10, 50, 40);
		var tmpServo = GUI.Toggle(servoRect, servoUp, "Servo");
		if(servoUp != tmpServo) {
			servoUp = tmpServo;
			if(servoUp)	m_servoController.Up();
			else m_servoController.Down();
		}


		// LED Update
		var ledRect = new Rect(10, 50, 50, 50);
		var tmp = GUI.Toggle(ledRect, ledOn, "LED");
		if(ledOn != tmp) {
			Debug.Log(tmp);
			ledOn = tmp;
			m_ledController.SetLed(ledOn);
		}

		// Solenoid Update
		var solenoidRect = new Rect(10, 100, 100, 50);
		var tmpSolenoid = GUI.Toggle(solenoidRect, solenoidOn, "Solenoid");
		if(solenoidOn != tmpSolenoid) {
			Debug.Log(tmpSolenoid);
			solenoidOn = tmpSolenoid;
			m_solenoidController.Push(solenoidOn);
		}
  }
}
