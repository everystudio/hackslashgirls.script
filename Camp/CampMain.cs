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

	public MedalPrizeBuyCheck m_medalPrizeBuyCheck;

	void Start()
	{
		m_prefBannerDungeon.SetActive(false);
		m_prefBannerMedal.SetActive(false);
		m_prefBannerSkin.SetActive(false);
	}

	public GameObject m_goCampCheck;

	public TextMeshProUGUI m_txtCheckTitle;
	public TextMeshProUGUI m_txtCheckDetail;
	public GameObject m_goFirstClearPrize;

	public Button m_btnYes;
	public Button m_btnNo;

	public GameObject m_goSkinCheck;
	public Image m_imgSkinIcon;
	public TextMeshProUGUI m_txtSkinName;
	public Button m_btnSkinYes;
	public Button m_btnSkinNo;
	public GameObject m_goPrizeRoot;
	public BannerClearPrize prize_1;
	public BannerClearPrize prize_2;



}
