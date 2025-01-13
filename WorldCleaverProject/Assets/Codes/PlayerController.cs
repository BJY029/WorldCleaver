using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public readonly float PlayerMana = 100.0f;
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
        //���� 0 Ȥ�� ������ �Ǹ�, �׳� 0�� �ʱ�ȭ�Ѵ�.
        else if (playerMana + mana <= 0)
        {
            playerMana = 0;
            GameManager.Instance.Turn = 44;
        }
        //�� �ܴ� �׳� �ջ��� ���� �����Ѵ�.
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
            //���� ����� �� ��������, Turn�� 44�� ��ȯ
			playerMana = 0;
			//Debug.Log("Player Lose!");
			GameManager.Instance.Turn = 44;
		}
        //Hit ��ư�� ���� ��, ���� ��ü�ȴ�.
		GameManager.Instance.ChangeTurn();
	}
}
