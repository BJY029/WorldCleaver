using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerMana;

    //플레이어의 기력 값을 반환해주는 함수
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
            //만약 기력이 다 떨어지면, Turn을 44로 변환
			playerMana = 0;
			Debug.Log("Player Lose!");
			GameManager.Instance.Turn = 44;
		}
        //Hit 버튼을 누른 후, 턴이 교체된다.
		GameManager.Instance.ChangeTurn();
	}
}
