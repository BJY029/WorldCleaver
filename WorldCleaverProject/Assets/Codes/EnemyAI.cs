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
        //�� �������� �� �� ��� �������� �ʴ´�.
        if (EnemyItems.Count == 5) return;

        //���� ������ ����â ����
        chooseanItems.Clear();
        //���� ������ ���� â �Ҵ�
        for(int i = 0; i < 3; i++)
        {
            Item randomItem;
            do
            {
                randomItem = ItemManager.Instance.GiveRandomItem();
            } while (chooseanItems.Contains(randomItem));
        }

        //�ش� ������ �� ���� ��Ȳ�� �°� ������ ����

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
