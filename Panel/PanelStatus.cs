using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PanelStatus : MonoBehaviour {

	public TextMeshProUGUI m_txtAttack;
	public TextMeshProUGUI m_txtDefence;
	public TextMeshProUGUI m_txtMagic;

	private int m_iAttack;
	private int m_iDefence;
	private int m_iMagic;

	public BtnEquip m_btnWeapon;
	public BtnEquip m_btnArmor;
	public BtnEquip m_btnAccessary1;
	public BtnEquip m_btnAccessary2;
	public BtnEquip m_btnAccessary3;

	private List<BtnEquip> btn_equip_list = new List<BtnEquip>();


	public PanelEquip m_panelEquip;
	public PanelShortcuts m_panelShortcuts;
	public PanelShortcutSet m_panelShortcutSet;
	public PanelCategory m_panelCategory_shortcutset;

	public bool Initialized = false;

	[System.Serializable]
	public class EventEquip : UnityEvent<int,int>
	{
	}


	public EventEquip OnCurrentEquip = new EventEquip();

	void Start()
	{
		m_iMagic = -1;
		m_btnWeapon.btn.onClick.AddListener(() => {
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_WEAPON);
			OnCurrentEquip.Invoke(m_btnWeapon.equip_index, MasterItem.CategoryWeapon);
		});
		m_btnArmor.btn.onClick.AddListener(() => {
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_ARMOR);
			OnCurrentEquip.Invoke(m_btnArmor.equip_index, MasterItem.CategoryArmor);
		});
		m_btnAccessary1.btn.onClick.AddListener(() => {
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_BRECELET);
			OnCurrentEquip.Invoke(m_btnAccessary1.equip_index, MasterItem.CategoryBracelet);
		});
		m_btnAccessary2.btn.onClick.AddListener(() => {
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_CLOAK);
			OnCurrentEquip.Invoke(m_btnAccessary2.equip_index, MasterItem.CategoryCloak);
		});
		m_btnAccessary3.btn.onClick.AddListener(() => {
			GameMain.Instance.Message(PanelMessage.MENU.MESSAGE_HELMET);
			OnCurrentEquip.Invoke(m_btnAccessary3.equip_index, MasterItem.CategoryHelmet);
		});


		m_btnWeapon.equip_category = MasterItem.CategoryWeapon;
		m_btnArmor.equip_category = MasterItem.CategoryArmor;
		m_btnAccessary1.equip_category = MasterItem.CategoryBracelet;
		m_btnAccessary2.equip_category = MasterItem.CategoryCloak;
		m_btnAccessary3.equip_category = MasterItem.CategoryHelmet;

		btn_equip_list.Add(m_btnWeapon);
		btn_equip_list.Add(m_btnArmor);
		btn_equip_list.Add(m_btnAccessary1);
		btn_equip_list.Add(m_btnAccessary2);
		btn_equip_list.Add(m_btnAccessary3);

		m_panelEquip.gameObject.SetActive(false);
		m_panelShortcutSet.gameObject.SetActive(false);

		Initialized = true;
	}

	public BtnEquip GetBtnEquipFromCategory( int _iCategory)
	{
		foreach( BtnEquip btn in btn_equip_list)
		{
			if( btn.equip_category == _iCategory)
			{
				return btn;
			}
		}
		return null;
	}

	public void Refresh()
	{
		m_btnWeapon.Refresh();
		m_btnArmor.Refresh();
		m_btnAccessary1.Refresh();
		m_btnAccessary2.Refresh();
		m_btnAccessary3.Refresh();

		DataManager.Instance.dataChara.RefreshEquip();
	}

	void Update()
	{
		if( m_iAttack != DataManager.Instance.dataChara.attack)
		{
			m_iAttack = DataManager.Instance.dataChara.attack;
			m_txtAttack.text = m_iAttack.ToString();
		}
		if( m_iDefence != DataManager.Instance.dataChara.defence)
		{
			m_iDefence = DataManager.Instance.dataChara.defence;
			m_txtDefence.text = m_iDefence.ToString();
		}

		if(m_iMagic != DataManager.Instance.dataChara.magic)
		{
			m_iMagic = DataManager.Instance.dataChara.magic;
			m_txtMagic.text = m_iMagic.ToString();
		}
	}








}
