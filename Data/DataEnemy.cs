using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEnemyParam : CsvDataParam
{
	public int enemy_id { get; set; }

	public bool is_battle { get; set; }
	public bool is_dead { get; set; }

	public int hp { get; set; }
	public int hp_max { get; set; }

	public int attack { get; set; }
	public int defence { get; set; }
	public int speed { get; set; }

	public string attribute { get; set; }

	public MasterEnemyParam enemy_param;

	//public const float DAMAGE_RATE = 1.0f;
	//public const float DAMAGE_SWING = 0.25f;

	public int Damage( int _iAttack , string _strAttribute)
	{
		//float fDamage = (float)(_iAttack * _iAttack) / (float)defence * (DAMAGE_RATE + (UtilRand.GetRange(DAMAGE_SWING) - DAMAGE_SWING * 0.5f));

		/*
		Debug.Log((float)(_iAttack * _iAttack));
		Debug.Log((float)defence);
		Debug.Log((float)(_iAttack * _iAttack) / (float)defence);
		Debug.Log((DAMAGE_RATE + (UtilRand.GetRange(DAMAGE_SWING) - DAMAGE_SWING * 0.5f)));
		Debug.Log(fDamage);

		float temp = 0;
		for( int i = 0; i < 100; i++)
		{
			temp += UtilRand.GetRange(DAMAGE_SWING);
		}
		Debug.Log(temp * 0.01f);
		*/
		/*
		fDamage *= Defines.GetAttributeRate(attribute, _strAttribute);

		if( fDamage < 1.0f)
		{
			fDamage = 1.0f;
		}

		// 属性補正

		int iDamageResult = (int)fDamage;
		*/

		int iDamageResult = CalcDamage.Damage(_iAttack, _strAttribute, defence, attribute);
		//Debug.Log(string.Format("enemydamage attack={0} defence={1}", _iAttack, defence));


		//Debug.Log(string.Format("hp={0} damage={1}", hp, iDamageResult));
		hp -= iDamageResult;
		return iDamageResult;
	}


}

public class DataEnemy : CsvData<DataEnemyParam> {

}
