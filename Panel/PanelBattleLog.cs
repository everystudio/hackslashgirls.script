using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelBattleLog : MonoBehaviour {

	[SerializeField]
	private LogMessageLine[] log_message_arr;

	public List<LogMessageLine.MessageData> log_queue = new List<LogMessageLine.MessageData>();

#if UNITY_EDITOR
	public string TestMessage;
#endif
	void Start()
	{
		for( int i = 0; i < log_message_arr.Length; i++)
		{
			AddMessage("");
		}
	}

	public void AddMessage( string _strMessage)
	{
		LogMessageLine.MessageData data = new LogMessageLine.MessageData(_strMessage);

		log_queue.Add(data);

		if( log_message_arr.Length < log_queue.Count)
		{
			log_queue.RemoveAt(0);
		}
		for( int i = 0; i < log_queue.Count; i++)
		{
			log_message_arr[i].SetMessage(log_queue[i]);
		}
	}


}
