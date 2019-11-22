using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcDamage : MonoBehaviour {

	public const float DAMAGE_RATE = 0.5f;
	public const float DAMAGE_SWING = 0.025f;

	static public int Damage( int _iAttack , string _sttAttributeAttack , int _iDefence , string _strAttributeDefence)
	{
		int iResult = 0;

		float fDamage = (float)(_iAttack * _iAttack) / (float)_iDefence * (DAMAGE_RATE + (UtilRand.GetRange(DAMAGE_SWING) - DAMAGE_SWING * 0.5f));
		fDamage *= Defines.GetAttributeRate(_strAttributeDefence, _sttAttributeAttack);
		if (fDamage < 1.0f)
		{
			fDamage = 1.0f;
		}
		iResult = (int)fDamage;

		return iResult;
	}



}
