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
		//만약 입력받은 마나 값과의 합이 100을 넘으면, 그냥 100으로 초기화한다.
		if (enemyMana + mana > 100) enemyMana = 100;
		//만약 0 혹은 음수가 되면, 그냥 0로 초기화한다.
		else if (enemyMana + mana <= 0)
		{
			enemyMana = 0;
			GameManager.Instance.Turn = 44;
		}
		//그 외는 그냥 합산한 값을 저장한다.
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
			//만약 기력이 다 떨어지면, Turn을 44로 변환
			enemyMana = 0;
			Debug.Log("Enemy Lose!");
			GameManager.Instance.Turn = 44;
		}
		//Hit 버튼을 누른 후, 턴이 교체된다.
		GameManager.Instance.ChangeTurn();
	}
}
