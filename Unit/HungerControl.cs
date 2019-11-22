using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerControl : MonoBehaviour {
	[SerializeField]
	private float m_fInterval;
	public float m_fHungerTimer;

	private DataCharaParam dataChara
	{
		get
		{
			if(m_dataChara == null)
			{
				m_dataChara = DataManager.Instance.dataChara;
			}
			return m_dataChara;
		}
	}
	private DataCharaParam m_dataChara;

	void Start()
	{
		m_fHungerTimer = m_fInterval;
	}

	// Update is called once per frame
	void Update () {

		m_fHungerTimer -= Time.deltaTime;
		if (m_fHungerTimer < 0.0f)
		{
			dataChara.HungerDamage(1);
			m_fHungerTimer = m_fInterval;
		}


		
	}
}
