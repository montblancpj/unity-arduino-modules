using UnityEngine;
using System.Collections;

public class ServoController {
	private const string ServoHeader = "s";
	private const int MinDeg = 0;
	private const int MaxDeg = 67;

	static private ServoController s_instance = null;

	private SerialHandler m_handler = null;

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

	public void Down() {
		if(m_handler != null) {
			var data = m_handler.CreateSendData<int>(ServoHeader, MinDeg);
			m_handler.SendData(data);
		}
	}

	public void Up () {
		if(m_handler != null) {
			var data = m_handler.CreateSendData<int>(ServoHeader, MaxDeg);
			m_handler.SendData(data);
		}
	}
}
