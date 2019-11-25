using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameMain : Singleton<GameMain> {

	public CharaControl charaControl;
	public PanelStatus panelStatus;
	public PanelBossStatus panelBossStatus;
	public PanelCaution panelCaution;
	public PanelMessage panelMessage;

	public GameObject m_goStage;
	public GameObject m_goFloor;
	public GameObject m_prefEnemy;
	public GameObject m_prefDropObject;

	public FadeScreen m_fadeScreen;
	public Animator m_animFadeScreen;

	public PanelUnityAdsBonus m_panleUnityAdsBonus;

	public GameObject m_goSleepChara;
	//public GameObject m_goSleepBack;

	[SerializeField]
	private PanelBattleLog battle_log;

	void Start()
	{
		m_prefEnemy.SetActive(false);

	}
#if UNITY_EDITOR
	public int chara_hp;
	void Update()
	{
		chara_hp = DataManager.Instance.dataChara.hp;

	}
#endif

	public void FadeIn()
	{
		m_animFadeScreen.SetBool("open", true);
	}
	public void FadeOut()
	{
		m_animFadeScreen.SetBool("open", false);
	}

	[HideInInspector]
	public List<EnemyBase> enemy_list = new List<EnemyBase>();

	public void ClearEnemy()
	{
		if (m_prefEnemy.activeSelf)
		{
			m_prefEnemy.SetActive(false);
		}
		EnemyBase[] enemy_arr = m_goStage.GetComponentsInChildren<EnemyBase>();

		foreach( EnemyBase enemy in enemy_arr)
		{
			Destroy(enemy.gameObject);
		}
		enemy_list.Clear();
	}

	public DataEnemyParam CreateEnemy( MasterEnemyParam _master , float _fPosX , bool _bIsBoss )
	{
		Enemy enemy = PrefabManager.Instance.MakeScript<Enemy>(m_prefEnemy, m_goStage);

		Vector3 scale = Vector3.one * ((float)_master.size / 100.0f);

		if( _bIsBoss)
		{
			scale *= 2.0f;
		}
		//enemy.gameObject.transform.localScale = scale;
		enemy.Initialize(_master, _fPosX, this,_bIsBoss,scale);
		enemy_list.Add(enemy);

		return enemy.enemy_param;
	}

	public bool IsBattleEnemy()
	{
		foreach( EnemyBase enemy in enemy_list)
		{
			if(enemy.enemy_param.is_battle && enemy.enemy_param.is_dead == false )
			{
				//Debug.Log(enemy.gameObject.name);
				return true;
			}
		}
		return false;
	}

	public void CreateDropObject(EnemyBase enemyBase, MasterEnemyParam master_param)
	{
		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject obj = Instantiate(m_prefDropObject, enemyBase.gameObject.transform.position , rot, m_goFloor.transform) as GameObject;
		obj.SetActive(true);
		DropObject script = obj.GetComponent<DropObject>();

		int iDropItemId = master_param.GetDropItemId();

		MasterItemParam drop_item = DataManager.Instance.masterItem.list.Find(p => p.item_id == iDropItemId);

		script.Initialize(drop_item);
	}

	public void ClearDropObjects()
	{
		DropObject[] obj_arr = m_goFloor.GetComponentsInChildren<DropObject>();

		foreach( DropObject drop in obj_arr)
		{
			Destroy(drop.gameObject);
		}
	}

	public void BattleLog(string _strMessage)
	{
		battle_log.AddMessage(_strMessage);
	}

	public void Caution(string _strTitle , string _strMessage)
	{
		panelCaution.Show(_strTitle, _strMessage);
	}

	public void Message( PanelMessage.MENU _menu , bool _bOnly = true)
	{
		panelMessage.Show(_menu,_bOnly);
	}

	public bool ShortcutRefresh( int _iSerial)
	{
		//UpdateSerial
		return panelStatus.m_panelShortcuts.RefreshSerial(_iSerial);
	}

	public bool ShowAd(int _iAdd )
	{
		if (Advertisement.IsReady())
		{
			DataManager.Instance.user_data.AddInt(Defines.KeyGem, _iAdd);
			Advertisement.Show();

			m_panleUnityAdsBonus.gameObject.SetActive(true);
			return true;
		}
		return false;
	}

	public void Sleep(bool _bFlag)
	{
		m_goSleepChara.SetActive(_bFlag);
		//m_goSleepBack.SetActive(_bFlag);
		m_goSleepChara.GetComponent<Animator>().Play("sleep");
	}
}
