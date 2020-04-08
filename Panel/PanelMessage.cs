using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMessage : MonoBehaviour {

	public GameObject m_goRoot;
	public GameObject m_goPrev;
	public GameObject m_goNext;
	public GameObject m_goClose;

	public enum MENU
	{
		MESSAGE_WEAPON = 0,
		MESSAGE_ARMOR,
		MESSAGE_BRECELET,
		MESSAGE_CLOAK,
		MESSAGE_HELMET,
		MESSAGE_CRAFT,
		MESSAGE_SHORTCUT
	};

	public void Show( MENU _menu , bool _bOnly = false )
	{
		//Debug.Log(_menu);
		if( _bOnly == true)
		{
			if(DataManager.Instance.user_data.HasKey(_menu.ToString()))
			{
				return;
			}
		}

		play_message = messages[(int)_menu];

		page_index = 0;
		page_max = play_message.Count;

		disp_page();

		m_goRoot.SetActive(true);

		DataManager.Instance.user_data.WriteInt(_menu.ToString(), 1);

	}

	public void Prev()
	{
		page_index -= 1;
		disp_page();
	}

	public void Next()
	{
		page_index += 1;
		disp_page();
	}

	public void Close()
	{
		m_goRoot.SetActive(false);
	}

	private void disp_page()
	{
		m_txtMessage.text = play_message[page_index];

		m_goPrev.SetActive(page_index != 0);
		m_goNext.SetActive((page_index + 1) < page_max);
		m_goClose.SetActive((page_index + 1) == page_max);


	}

	private List<string> play_message;
	private int page_index;
	private int page_max;

	public TMPro.TextMeshProUGUI m_txtMessage;

	public const int CategoryBracelet = 5;
	public const int CategoryCloak = 6;
	public const int CategoryHelmet = 7;

	public List<List<string>> messages = new List<List<string>>
	{
		new List<string>() {
			"Equip weapons to increase attack power",
			"The higher the attack power, the more damage you deal"
		},
		new List<string>() {
			"Equip armor to increase defense",
			"It is possible to reduce the damage received by increasing the defense"
		},
		new List<string>() {
			"Equip your bangles to increase your maximum stamina",
			"If you have low stamina, your attack speed may drop and your opponent may preempt you",
			"Eat bread and be careful not to be hungry.",
		},
		new List<string>() {
			"Equip your cloak to increase your maximum HP.",
			"that's all!",
		},
		new List<string>() {
			"ヘルムを装備すると、魔法攻撃力(Mag)をアップさせることができます",
			"アイテムの魔法攻撃で与えられるダメージがアップします",
			"武器攻撃だけで突破が難しい場合は魔法も併用しましょう",
		},
		new List<string>() {
			"Craft items can enhance equipment items.",
			"To strengthen, you need the necessary reinforcement materials and gems for each equipment.",
			"It is also possible to strengthen several times at once",
		},
		new List<string>() {
			"If you register an item as a shortcut, you can use the item with one touch",
			"If you set equipment, you can switch quickly",
			"It is also possible to change the contents of the shortcut being set by <color=red>pressing and holding</color>",
		},



	};





}
