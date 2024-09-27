using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyController : MonoBehaviour
{
	private float enemyMana;
	private float HitMana = 5f;

	public float Mana
	{
		get
		{
			return enemyMana;
		}
	}

	private void Awake()
	{
		enemyMana = 100.0f;
	}

	private void Start()
	{
		Debug.Log("Enemy Mana: " + enemyMana);
	}

	public void Hit()
	{
		if (enemyMana > HitMana)
		{
			enemyMana -= HitMana;
			Debug.Log("Enemy Mana: " + enemyMana);
		}
		else
		{
			//���� ����� �� ��������, Turn�� 44�� ��ȯ
			enemyMana = 0;
			Debug.Log("Enemy Lose!");
			GameManager.Instance.Turn = 44;
		}
		//Hit ��ư�� ���� ��, ���� ��ü�ȴ�.
		GameManager.Instance.ChangeTurn();
	}
}
