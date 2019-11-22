using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingControl : MonoBehaviour {

	public void ReturnGame()
	{
		SceneManager.LoadScene("main");
	}

}
