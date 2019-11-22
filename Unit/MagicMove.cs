using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMove : MonoBehaviour {

	// Update is called once per frame
	void Update () {


		transform.position += Vector3.right * 10.0f * Time.deltaTime;
	}
}
