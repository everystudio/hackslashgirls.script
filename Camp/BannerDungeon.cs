using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerDungeon : MonoBehaviour {

	public UnityEventString OnEvent = new UnityEventString();
	private string m_strDungeonId;
	public TextMeshProUGUI m_txtName;
	public TextMeshProUGUI m_txtOutline;
	public TextMeshProUGUI m_txtFloor;
	public Button btn;

	public void Initialize(MasterDungeonParam _master)
	{
		//Debug.Log(_master.dungeon_label);
		m_strDungeonId = _master.dungeon_id;
		m_txtName.text = _master.dungeon_label;
		m_txtOutline.text = _master.outline;

		int iBest = DataManager.Instance.GetBestFloor(_master.dungeon_id);

		m_txtFloor.text = string.Format("{0}/{1}", iBest, _master.floor_max);

		btn.onClick.AddListener(() =>
		{
			OnEvent.Invoke(m_strDungeonId);
		});
	}

}
