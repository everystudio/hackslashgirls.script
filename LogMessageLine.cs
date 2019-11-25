using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogMessageLine : MonoBehaviour {
	[SerializeField]
	TextMeshProUGUI uiText;

	[SerializeField]
	[Range(0.001f, 0.3f)]
	float message_speed;

	public int m_ilength;
	public float m_fDeltaTime;

	public class MessageData
	{
		public MessageData(string _strMessage)
		{
			message = _strMessage;
			index = 0;
		}

		public string message;
		public int index;
		public string GetCurrentMessage()
		{
			return message.Substring(0, index);
		}
		public int GetLength() { return message.Length; }

		public int UpdateIndex( int _iIndex , bool _bRedirect = false)
		{
			int iAdd = 1;

			string next_char = message.Substring(_iIndex, 1);

			if (next_char == "<")
			{
				int iSearchIndex = message.Substring(_iIndex, message.Length - _iIndex).IndexOf(">", 0);

				if (iSearchIndex == -1)
				{
					Debug.LogError("書式が間違っています");
				}
				else
				{
					iAdd = iSearchIndex +1;

					// リダイレクト前に文字列の終了じゃないかどうかを判定する
					if ((_iIndex + iAdd) < message.Length )
					{
						iAdd += UpdateIndex(_iIndex+iAdd , true);
					}
				}
			}
			if (_bRedirect == false)
			{
				index += iAdd;
			}
			return iAdd;
		}
	}

	public MessageData message_data;

	/*
#if UNITY_EDITOR
	public bool is_debug;
	public string test_message;
	void Start()
	{
		if( is_debug)
		{
			MessageData debug_data = new MessageData(test_message);
			SetMessage(debug_data);
		}
	}
#endif
*/

	public void SetMessage(MessageData _data )
	{
		message_data = _data;
		message_data.message = _data.message;
		message_data.index = _data.index;
		uiText.text = message_data.GetCurrentMessage();
		m_ilength = message_data.GetLength();
		m_fDeltaTime = 0.0f;
	}

	void Update()
	{
		if(message_data == null || m_ilength == message_data.index)
		{
			return;
		}

		m_fDeltaTime += Time.deltaTime;
		if (message_speed <= m_fDeltaTime)
		{
			m_fDeltaTime -= message_speed;
			if (0 < message_data.UpdateIndex(message_data.index,false))
			{
				//Debug.Log(m_iIndex);
				//Debug.Log(message.Length);
				uiText.text = message_data.GetCurrentMessage();
			}
		}
	}



}
