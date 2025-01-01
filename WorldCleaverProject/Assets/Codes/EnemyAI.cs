using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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

    private Dictionary<string, Dictionary<AIState, float>> itemCoff;

    void Start()
    {
        CurrentState = AIState.Neutral;
		EnemyItems = new List<Item> {null, null, null, null, null};
		chooseanItems = new List<Item> { null, null, null, null, null };

        itemCoff = new Dictionary<string, Dictionary<AIState, float>>()
        {
            {"Hit", new Dictionary<AIState, float>{{AIState.Aggressive, 5.0f}, {AIState.Neutral, 3.0f}, {AIState.Defensive, 1.0f } } },
            {"Charge", new Dictionary<AIState, float>{ {AIState.Aggressive, 1.0f }, { AIState.Neutral, 3.0f}, {AIState.Defensive, 5.0f } } },
			{"Defense", new Dictionary<AIState, float>{ {AIState.Aggressive, 1.0f }, { AIState.Neutral, 3.0f}, {AIState.Defensive, 5.0f } } },
			{"Heal", new Dictionary<AIState, float>{ {AIState.Aggressive, 1.0f }, { AIState.Neutral, 3.0f}, {AIState.Defensive, 5.0f } } },
			{"Village", new Dictionary<AIState, float>{ {AIState.Aggressive, 1.0f }, { AIState.Neutral, 3.0f}, {AIState.Defensive, 5.0f } } },
			{"Gimmick", new Dictionary<AIState, float>{ {AIState.Aggressive, 1.0f }, { AIState.Neutral, 5.0f}, {AIState.Defensive, 3.0f } } },
		};
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
        /*
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
        */

        foreach(Item item in chooseanItems)
        {
            //딕셔너리를 활용하여 최적화 진행
            //TryGetValue 함수는 itemCoff 딕셔너리에서 item.Type을 키로하여 내부 딕셔너리를 가져온다.
            //성공적으로 가져올 시, 상태별 계수(stateCoff)을 out 변수로 반환한다.
            if(itemCoff.TryGetValue(item.Type, out var stateCoff))
            {
                //coef에 가져온 딕셔너리의 현재 상태 값을 키로 하여서, 해당 값에 연결된 계수 값을 가져온다.
                float coef = stateCoff[CurrentState];
                ManaCoef = CalcManaCoef(item.Mana);
                item.priority = coef * ManaCoef * item.Coef;
            }
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
        DisplayEnemyItems.Instance.insertItem(chooseItem);
		Debug.Log("Enemy choose " + chooseItem.itemName);
    }

    public void UseEnemyItem()
    {
        //현재 보유한 아이템이 없는 경우 아이템을 사용하지 않는다.
        if(isEnemyItemisEmpty()) return;

        //가장 큰 우선순위를 지닌 아이템을 저장할 변수
        Item useItem = null;
        float maxPriority = -1.0f;

        //현재 상태 체크
		checkState();

		for(int i = 0; i < 5; i++)
        {
            Item item = EnemyItems[i];
            //만약 저장된 아이템 리스트 중 아무것도 저장되지 않은 경우 그냥 넘어간다.
            if(item == null) continue;

            //딕셔너리 연산을 통해 우선순위 연산을 진행한다.
			if (itemCoff.TryGetValue(item.Type, out var stateCoff))
			{
				//coef에 가져온 딕셔너리의 현재 상태 값을 키로 하여서, 해당 값에 연결된 계수 값을 가져온다.
				float coef = stateCoff[CurrentState];
				ManaCoef = CalcManaCoef(item.Mana);
				item.priority = coef * ManaCoef * item.Coef;
			}
            //계산한 아이템의 우선순위가 가장 큰 경우, 해당 아이템을 저장한다.
            if(maxPriority < item.priority)
            {
                maxPriority = item.priority;
                useItem = item;
            }
		}

        //만약 선택된 아이템의 마나 사용량이 나의 마나 보유량보다 큰 경우, 아이템을 사용하지 않는다.
        //Hit Mana 까지 고려하기위해 5를 더한다.
        if (useItem.Mana + 5 >= GameManager.Instance.EnemeyController.Mana) return;

        //해당 아이템을 사용한다.
		Debug.Log("Enemy use " + useItem.itemName);
        //우선 적 아이템 리스트에서 해당 아이템을 null 처리 하고
        EnemyItems[EnemyItems.IndexOf(useItem)] = null;
        //UI 상에서의 버튼 리스트에서도 제거한다.
		DisplayEnemyItems.Instance.removeItem(useItem.icon);
        //그리고 해당 아이템의 효과를 ItemFunciton 함수를 통해 수행한다.
        ItemManager.Instance.ItemFunction(useItem.id);
    }



	public bool isEnemyItemisFull()
	{
		// EnemyItems 리스트 내 null이 아닌 아이템 개수를 계산
		return EnemyItems.Count(item => item != null) >= 5;
	}

    public bool isEnemyItemisEmpty()
    {
        return EnemyItems.Count(item => item == null) >= 5;
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


	public void EnemyTurnBehavior()
	{
		if (GameManager.Instance.Turn != 1) return;
		ChooseEnemyItem();
		StartCoroutine("EnemyTurn");
	}

	IEnumerator EnemyTurn()
	{
		yield return new WaitForSeconds(waitSecond);
        UseEnemyItem();
		yield return new WaitForSeconds(waitSecond);
		GameManager.Instance.AnimationManager.Hit();
		GameManager.Instance.Hit();
	}
}
