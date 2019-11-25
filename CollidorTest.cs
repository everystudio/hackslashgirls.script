using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollidorTest : MonoBehaviour {

	public Button push_button;

	void Start()
	{
		push_button.onClick.Invoke();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log(collision.gameObject.tag);
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.SendMessage("ApplyDamage", 10);
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log(collision.gameObject.tag);
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.SendMessage("ApplyDamage", 10);
		}
	}
}
