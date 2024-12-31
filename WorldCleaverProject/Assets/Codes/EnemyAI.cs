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

    //각 상태에 따른 계수
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
        //적 아이템이 꽉 찬 경우 선택하지 않는다.
        if (isEnemyItemisFull()) return;

        //기존 아이템 선택 리스트 제거
        chooseanItems.Clear();
        //새로 아이템 선택 리스트 할당
        for(int i = 0; i < 3; i++)
        {
            Item randomItem;
            do
            {
                randomItem = ItemManager.Instance.GiveRandomItem();
            } while (chooseanItems.Contains(randomItem));


            chooseanItems.Add(randomItem);

        }

        //해당 선택지 중 현재 상황에 맞게 아이템 선택

        //우선 현재 AI가 취할 상태를 결정한다.
        //Aggressive , Netural, Defensive 세 가지 상태가 존재한다.
        checkState();
       
        //각 아이템의 우선순위 계수를 부여한다.
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

        //아이템 중 우선순위가 가장 큰 아이템을 선정한다.
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
		//선택된 아이템을 아이템 리스트에 삽입한다.
		int emptySlotIndex = EnemyItems.IndexOf(null);
		if (emptySlotIndex != -1) // 빈 슬롯이 있는 경우
		{
			EnemyItems[emptySlotIndex] = chooseItem;
		}

		Debug.Log("Enemy choose " + chooseItem.itemName);
    }

	public bool isEnemyItemisFull()
	{
		// EnemyItems 리스트 내 null이 아닌 아이템 개수를 계산
		return EnemyItems.Count(item => item != null) >= 5;
	}


	public void checkState()
    {
		//판단에 쓰일 값들 받아오기
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
