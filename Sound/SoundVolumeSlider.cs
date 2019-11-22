using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeSlider : MonoBehaviour {


	[SerializeField]
	private UnityEngine.Audio.AudioMixer mixer;
	[SerializeField]
	private UnityEngine.UI.Slider slider;

	public string group_name;

	public float volume
	{
		set
		{
			mixer.SetFloat(group_name, Mathf.Lerp(Defines.SOUND_VOLME_MIN, Defines.SOUND_VOLUME_MAX, value));
			DataManager.Instance.user_data.Write(group_name, value.ToString());
		}
	}
	
	void OnEnable()
	{
		slider.value = DataManager.Instance.user_data.ReadFloat(group_name);
	}
}
