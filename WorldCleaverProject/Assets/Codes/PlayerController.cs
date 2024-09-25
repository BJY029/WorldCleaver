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

	void Start()
    {
		Debug.Log("Player Mana: " + playerMana);
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
            //gameLose!
        }
    }
}
