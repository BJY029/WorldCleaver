using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float playerMana;

    private float HitMana = 5f;

	private void Awake()
	{
        
	}

	void Start()
    {
        playerMana = 100.0f;
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
