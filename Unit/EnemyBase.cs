using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

	public SpriteRenderer m_sprMain;
	public Animator m_animator;
	public GameObject m_goSpriteRoot;

	[SerializeField]
	private GameObject m_goRootDamageText;
	[SerializeField]
	private GameObject m_prefDamageText;

	[SerializeField]
	private GameObject m_goRootAttack;
	[SerializeField]
	private GameObject m_prefAttack;

	private GameMain m_gameMain;

	public bool Initialized;
	public bool Touched;

	public DataEnemyParam enemy_param;
	public MasterEnemyParam master_param;
	/*
	[SerializeField]
	private int show_hp;
	[SerializeField]
	private int show_hp_max;
	[SerializeField]
	private int show_attack;
	[SerializeField]
	private int show_defence;
	[SerializeField]
	private int show_speed;
	*/

	public void Initialize(MasterEnemyParam _master, float _fPos , GameMain _game , bool _isBoss , Vector3 _scale)
	{
		enemy_param = _master.Create(_isBoss);
		master_param = _master;
		m_gameMain = _game;
		SetSprite(_master.sprite_name);
		if( _isBoss)
		{
			gameObject.name += "boss";
		}
		m_goSpriteRoot.transform.localScale = _scale;
		Touched = false;
		/*
		show_hp = enemy_param.hp;
		show_hp_max = enemy_param.hp_max;
		show_attack = enemy_param.attack;
		show_defence = enemy_param.defence;
		show_speed = enemy_param.speed;
		*/

		transform.localPosition = new Vector3(_fPos, 0.0f, 0.0f);
	}

	public void Damage( int _iAttack , string _strAttribute , int _iCount )
	{
		int iDamage = enemy_param.Damage(_iAttack, _strAttribute);

		Defines.ATTRIBUTE_CONDITION condition = Defines.GetAttributeCondition( enemy_param.attribute , _strAttribute);
		Color damage_color = Defines.GetAttributeDamageColor(condition);

		StartCoroutine(create_damage_num(iDamage, 0.1f * _iCount , damage_color));
	}

	private IEnumerator create_damage_num( int _iDamage , float _fDelay , Color _damage_color )
	{
		yield return new WaitForSeconds(_fDelay);

		const float DAMAGE_VECTOR_BASE = 0.2f;
		const float DAMAGE_VECTOR_SWING = 0.1f;

		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject obj = Instantiate(m_prefDamageText, Vector3.zero, rot, m_goRootDamageText.transform) as GameObject;

		obj.SetActive(true);
		obj.transform.localPosition = Vector3.zero;
		//obj.GetComponent<Animator>().SetInteger("damage_level", 1);

		//Debug.Log(iDamage);
		obj.GetComponent<TMPro.TextMeshPro>().text = _iDamage.ToString();
		obj.GetComponent<TMPro.TextMeshPro>().color = _damage_color;

		Vector2 dir = new Vector2(
			DAMAGE_VECTOR_BASE + (UtilRand.GetRange(DAMAGE_VECTOR_SWING) - DAMAGE_VECTOR_SWING * 0.5f),
			1.0f + (UtilRand.GetRange(DAMAGE_VECTOR_SWING) + DAMAGE_VECTOR_SWING * 0.5f));

		obj.GetComponent<Rigidbody2D>().AddForce(200f * dir);

		Destroy(obj, 2);


	}


	public void Attack()
	{
		Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
		GameObject obj = Instantiate(m_prefAttack, Vector3.zero, rot, m_goRootAttack.transform) as GameObject;

		obj.SetActive(true);
		obj.transform.localPosition = Vector3.zero;

		EnemyAttackArea script = obj.GetComponent<EnemyAttackArea>();
		//Debug.Log(enemy_param);
		script.Initialize(enemy_param);

		Destroy(obj, 2);
	}

	protected void SetSprite( string _strSpriteName)
	{
		m_sprMain.sprite = SpriteManager.Instance.Get("enemy", _strSpriteName);
	}

	public Vector3 Move(float _fDelta)
	{
		gameObject.transform.localPosition += new Vector3(_fDelta, 0.0f, 0.0f);
		return gameObject.transform.localPosition;
	}

	public void CreateDropObject()
	{
		m_gameMain.CreateDropObject(this, master_param);
	}

}
