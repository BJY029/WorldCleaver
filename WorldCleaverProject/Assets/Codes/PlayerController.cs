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
