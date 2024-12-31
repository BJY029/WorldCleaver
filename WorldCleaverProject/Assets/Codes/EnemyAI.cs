using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        CurrentState = AIState.Neutral;
		chooseanItems = new List<Item>();
	}

    public void ChooseEnemyItem()
    {
        //적 아이템이 꽉 찬 경우 선택하지 않는다.
        if (EnemyItems.Count == 5) return;

        //기존 아이템 선택창 제거
        chooseanItems.Clear();
        //새로 아이템 선택 창 할당
        for(int i = 0; i < 3; i++)
        {
            Item randomItem;
            do
            {
                randomItem = ItemManager.Instance.GiveRandomItem();
            } while (chooseanItems.Contains(randomItem));
        }

        //해당 선택지 중 현재 상황에 맞게 아이템 선택

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
