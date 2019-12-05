using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampMain : MonoBehaviour {

	public GameObject m_prefBannerDungeon;
	public GameObject m_prefBannerMedal;
	public GameObject m_prefBannerSkin;

	public GameObject m_goListRoot;
	public GameObject m_goListContent;

	void Start()
	{
		m_prefBannerDungeon.SetActive(false);
		m_prefBannerMedal.SetActive(false);
		m_prefBannerSkin.SetActive(false);
	}

	public GameObject m_goCampCheck;

	public TextMeshProUGUI m_txtCheckTitle;
	public TextMeshProUGUI m_txtCheckDetail;
	public Button m_btnYes;
	public Button m_btnNo;






}
