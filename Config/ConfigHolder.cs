using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigHolder : ScriptableObject
{
	public string version;
	public string app_environment;
	public List<string> message_list = new List<string>();
}
