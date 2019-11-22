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
			"武器を装備すると、攻撃力(Atk)が上がります。",
			"攻撃力が高くなれば、与えるダメージがアップします"
		},
		new List<string>() {
			"防具を装備すると、防御力(Def)が上がります",
			"防御力を高くすることで受けるダメージを軽減することが可能です"
		},
		new List<string>() {
			"腕輪を装備すると、スタミナ(Stamina)の最大値を上げることができます",
			"スタミナが少ない状態では、攻撃スピードが落ちて、相手から先制されることがあります",
			"パンなどを食べて、空腹状態にしないように注意しましょう。",
		},
		new List<string>() {
			"マントを装備すると、最大HPをアップさせることができます",
			"以上！",
		},
		new List<string>() {
			"ヘルムを装備すると、魔法攻撃力(Mag)をアップさせることができます",
			"アイテムの魔法攻撃で与えられるダメージがアップします",
			"武器攻撃だけで突破が難しい場合は魔法も併用しましょう",
		},
		new List<string>() {
			"クラフトでは装備アイテムの強化が可能です",
			"強化するには各装備ごとに必要な強化素材とGemが必要になります",
			"一度にまとめて複数回強化することも可能です",
		},
		new List<string>() {
			"ショートカットにアイテムを登録するとワンタッチでアイテムを利用できます",
			"装備品をセットした場合は、素早く切り替えることが可能です",
			"<color=red>長押し</color>でセット中のショートカット内容を変更することも可能です",
		},



	};





}
