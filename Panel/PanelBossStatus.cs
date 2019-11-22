using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelBossStatus : MonoBehaviour {

	public GameObject m_goRoot;

	public TextMeshProUGUI m_txtBossName;
	public EnergyBar m_barBossHp;

	public DataEnemyParam m_dataEnemyParam;
	public MasterEnemyParam m_masterEnemyParam;

	public void Initialize( DataEnemyParam _data  , MasterEnemyParam _master)
	{
		m_goRoot.SetActive(true);

		m_dataEnemyParam = _data;
		m_masterEnemyParam = _master;

		m_txtBossName.text = m_masterEnemyParam.name;

		m_barBossHp.SetValueMax(m_dataEnemyParam.hp_max);
	}

	public void Disable()
	{
		m_goRoot.SetActive(false);
	}

	void Update()
	{
		if( m_goRoot.activeSelf && m_dataEnemyParam != null )
		{
			m_barBossHp.SetValueCurrent(m_dataEnemyParam.hp);
		}
	}


}
