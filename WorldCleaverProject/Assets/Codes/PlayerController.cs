using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerMana;

    //�÷��̾��� ��� ���� ��ȯ���ִ� �Լ�
    public float Mana
    {
        get
        {
            return playerMana;
        }
    }

    public void setMana(float mana)
    {
        //���� �Է¹��� ���� ������ ���� 100�� ������, �׳� 100���� �ʱ�ȭ�Ѵ�.
        if (playerMana + mana > 100) playerMana = 100;
        //���� 0 Ȥ�� ������ �Ǹ�, �׳� 1�� �ʱ�ȭ�Ѵ�.
        else if (playerMana + mana <= 0)
        {
            playerMana = 1;
        }
        //�� �ܴ� �׳� �ջ��� ���� �����Ѵ�.
        else playerMana += mana;
    }

    private float HitMana = 5f;

	private void Awake()
	{
		playerMana = 100.0f;
	}


    void Update()
    {
        
    }

    public void Hit()
    {
        if (playerMana > HitMana)
        {
            playerMana -= HitMana;
            Debug.Log("Player Mana: " +  playerMana);
        }
        else
        {
            //���� ����� �� ��������, Turn�� 44�� ��ȯ
			playerMana = 0;
			Debug.Log("Player Lose!");
			GameManager.Instance.Turn = 44;
		}
        //Hit ��ư�� ���� ��, ���� ��ü�ȴ�.
		GameManager.Instance.ChangeTurn();
	}
}
