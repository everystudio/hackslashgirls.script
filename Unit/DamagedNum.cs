using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedNum : MonoBehaviour {

	[SerializeField]
	private GameObject m_goFloor;

	[SerializeField]
	private Rigidbody2D rigid_body2d;

	void Start()
	{
		transform.SetParent(m_goFloor.transform);
	}
	/*
	public bool fire;
	public float power;
	void Update()
	{
		if( fire)
		{
			fire = false;
			rigid_body2d.AddForce(new Vector2(-0.1f, 1.0f) * power);
		}
	}
	*/

}
