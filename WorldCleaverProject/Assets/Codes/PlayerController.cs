using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public readonly float PlayerMana = 100.0f;
    private float playerMana;

    //플레이어의 기력 값을 반환해주는 함수
    public float Mana
    {
        get
        {
            return playerMana;
        }
    }

    public void setMana(float mana)
    {
        //만약 입력받은 마나 값과의 합이 100을 넘으면, 그냥 100으로 초기화한다.
        if (playerMana + mana > 100) playerMana = 100;
        //만약 0 혹은 음수가 되면, 그냥 0로 초기화한다.
        else if (playerMana + mana <= 0)
        {
            playerMana = 0;
            GameManager.Instance.Turn = 44;
        }
        //그 외는 그냥 합산한 값을 저장한다.
        else playerMana += mana;
    }

    private float HitMana = 5f;

	public int ReturnCurrentDanger()
	{
        float MaxTreeHealth = GameManager.Instance.TreeController.Treehealth;
		float CurrentTreeHealth = GameManager.Instance.TreeController.treeHealth;
        float CurrentPlayerMana = Mana;
        float MaxVillageHealth = GameManager.Instance.VillageManager.H;
        float CurrentPlayerVillageHealth = GameManager.Instance.VillageManager.VilageHelath;

        float TreeHealthW = CurrentTreeHealth / MaxTreeHealth;
        float PlayerManaW = CurrentPlayerMana / PlayerMana;
        float VillageHealthW = CurrentPlayerVillageHealth / MaxVillageHealth;

        float TotalWeight = (TreeHealthW * 0.7f) + PlayerManaW + VillageHealthW;

        if (TotalWeight > 1.3f) return 0;
        else if (TotalWeight > 0.9) return 1;
        else if (TotalWeight > 0.5) return 2;
        else return 3;
	}

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
            //Debug.Log("Player Mana: " +  playerMana);
        }
        else
        {
            //만약 기력이 다 떨어지면, Turn을 44로 변환
			playerMana = 0;
			//Debug.Log("Player Lose!");
			GameManager.Instance.Turn = 44;
		}
        //Hit 버튼을 누른 후, 턴이 교체된다.
		GameManager.Instance.ChangeTurn();
	}
}
