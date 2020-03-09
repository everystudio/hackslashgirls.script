using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerClearPrize : MonoBehaviour {

	public Image m_imgIcon;
	public TMPro.TextMeshProUGUI m_txtName;
	public TMPro.TextMeshProUGUI m_txtBikou;

	public void Initialize( MasterItemParam _master , int _iNum)
	{
		if(_master == null)
		{
			gameObject.SetActive(false);
			return;
		}

		m_imgIcon.sprite = SpriteManager.Instance.Get(_master.sprite_name);
		m_imgIcon.color = _master.GetSpriteColor();
		m_txtName.text = _master.name;

		if( 1 < _iNum)
		{
			m_txtName.text = string.Format("{0}x{1}", _master.name, _iNum);
		}

		m_txtBikou.text = "";

		if( _master.item_id / MasterItem.LargeCategory == MasterItem.CategoryWeapon)
		{
			m_txtBikou.text = string.Format("ATK:{0}", _master.param);
		}
		else if(_master.item_id / MasterItem.LargeCategory == MasterItem.CategoryArmor)
		{
			m_txtBikou.text = string.Format("DEF:{0}", _master.param);
		}
		else
		{
			m_txtBikou.text = "";
		}

		gameObject.SetActive(true);
	}

}
