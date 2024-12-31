using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//AI state machine
public enum AIState
{
    Neutral,
    Aggressive,
    Defensive
}

public class EnemyAI : SingleTon<EnemyAI>
{
    public List<Item> EnemyItems;
    public List<Item> chooseanItems;
	public AIState CurrentState;

    public float waitSecond;

    private float CurrentTreeHealth;
    private float CurrentEnemyMana;
    private float CurrentEnemyVillageHealth;

    //�� ���¿� ���� ���
    private float AggressiveCoef;
    private float DefensiveCoef;
    private float NeutralCoef;
    private float Coef;
    private float ManaCoef;

    void Start()
    {
        CurrentState = AIState.Neutral;
		EnemyItems = new List<Item> {null, null, null, null, null};
		chooseanItems = new List<Item> { null, null, null, null, null };
	}

	public void EnemyTurnBehavior()
	{
		if (GameManager.Instance.Turn != 1) return;
        ChooseEnemyItem();
		StartCoroutine("EnemyTurn");
	}

	IEnumerator EnemyTurn()
	{
		yield return new WaitForSeconds(waitSecond);
		GameManager.Instance.AnimationManager.Hit();
		GameManager.Instance.Hit();
	}


	public void ChooseEnemyItem()
    {
        //�� �������� �� �� ��� �������� �ʴ´�.
        if (isEnemyItemisFull()) return;

        //���� ������ ���� ����Ʈ ����
        chooseanItems.Clear();
        //���� ������ ���� ����Ʈ �Ҵ�
        for(int i = 0; i < 3; i++)
        {
            Item randomItem;
            do
            {
                randomItem = ItemManager.Instance.GiveRandomItem();
            } while (chooseanItems.Contains(randomItem));


            chooseanItems.Add(randomItem);

        }

        //�ش� ������ �� ���� ��Ȳ�� �°� ������ ����

        //�켱 ���� AI�� ���� ���¸� �����Ѵ�.
        //Aggressive , Netural, Defensive �� ���� ���°� �����Ѵ�.
        checkState();
       
        //�� �������� �켱���� ����� �ο��Ѵ�.
        foreach (Item item in chooseanItems)
        {
            if(item.Type == "Hit")
            {
                AggressiveCoef = 5.0f;
                NeutralCoef = 3.0f;
                DefensiveCoef = 1.0f;
            }
            else if(item.Type == "Charge")
            {
				DefensiveCoef = 5.0f;
				NeutralCoef = 3.0f;
				AggressiveCoef = 1.0f;
			}
			else if (item.Type == "Defense")
			{
				DefensiveCoef = 5.0f;
				NeutralCoef = 3.0f;
				AggressiveCoef = 1.0f;
			}
			else if (item.Type == "Heal")
			{
				DefensiveCoef = 5.0f;
				NeutralCoef = 3.0f;
				AggressiveCoef = 1.0f;
			}
			else if (item.Type == "Gimmick")
			{
				DefensiveCoef = 3.0f;
				NeutralCoef = 5.0f;
				AggressiveCoef = 3.0f;
			}
			else if (item.Type == "Village")
			{
				DefensiveCoef = 5.0f;
				NeutralCoef = 3.0f;
				AggressiveCoef = 1.0f;
			}

            ManaCoef = CalcManaCoef(item.Mana);

			if (CurrentState == AIState.Neutral) item.priority = NeutralCoef * ManaCoef * item.Coef;
            else if (CurrentState == AIState.Aggressive) item.priority = AggressiveCoef * ManaCoef * item.Coef;
            else if(CurrentState == AIState.Defensive) item.priority = DefensiveCoef * ManaCoef * item.Coef;
		}

        //������ �� �켱������ ���� ū �������� �����Ѵ�.
        Item chooseItem = null;
        float maxPriority = -1.0f;
        foreach (Item item in chooseanItems)
        {
            if(maxPriority < item.priority)
            {
                maxPriority = item.priority;
                chooseItem = item;
            }
        }

		//if (chooseItem.Mana > CurrentEnemyMana || chooseItem == null) return;
		//���õ� �������� ������ ����Ʈ�� �����Ѵ�.
		int emptySlotIndex = EnemyItems.IndexOf(null);
		if (emptySlotIndex != -1) // �� ������ �ִ� ���
		{
			EnemyItems[emptySlotIndex] = chooseItem;
		}

		Debug.Log("Enemy choose " + chooseItem.itemName);
    }

	public bool isEnemyItemisFull()
	{
		// EnemyItems ����Ʈ �� null�� �ƴ� ������ ������ ���
		return EnemyItems.Count(item => item != null) >= 5;
	}


	public void checkState()
    {
		//�Ǵܿ� ���� ���� �޾ƿ���
		CurrentTreeHealth = GameManager.Instance.TreeController.treeHealth;
		CurrentEnemyMana = GameManager.Instance.EnemeyController.Mana;
		CurrentEnemyVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;

        if(CurrentTreeHealth < 300f || CurrentEnemyMana < 40f || CurrentEnemyVillageHealth < 300)
        {
            CurrentState = AIState.Defensive;
        }
        else if (CurrentTreeHealth >= 600f && CurrentEnemyMana >= 60 && CurrentEnemyVillageHealth >= 500)
        {
            CurrentState = AIState.Aggressive;
        }
        else
        {
            CurrentState= AIState.Neutral;
        }
	}
    
    // Update is called once per frame
    void Update()
    {
        
    }


    float CalcManaCoef(float mana)
    {
        return 1 + (mana / 15);
    }
}
