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

	public void setMana(float mana)
	{
		//���� �Է¹��� ���� ������ ���� 100�� ������, �׳� 100���� �ʱ�ȭ�Ѵ�.
		if (enemyMana + mana > 100) enemyMana = 100;
		//���� 0 Ȥ�� ������ �Ǹ�, �׳� 0�� �ʱ�ȭ�Ѵ�.
		else if (enemyMana + mana <= 0)
		{
			enemyMana = 0;
			GameManager.Instance.Turn = 44;
		}
		//�� �ܴ� �׳� �ջ��� ���� �����Ѵ�.
		else enemyMana += mana;
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
