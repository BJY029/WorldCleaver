using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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
            //��ųʸ��� Ȱ���Ͽ� ����ȭ ����
            //TryGetValue �Լ��� itemCoff ��ųʸ����� item.Type�� Ű���Ͽ� ���� ��ųʸ��� �����´�.
            //���������� ������ ��, ���º� ���(stateCoff)�� out ������ ��ȯ�Ѵ�.
            if(itemCoff.TryGetValue(item.Type, out var stateCoff))
            {
                //coef�� ������ ��ųʸ��� ���� ���� ���� Ű�� �Ͽ���, �ش� ���� ����� ��� ���� �����´�.
                float coef = stateCoff[CurrentState];
                ManaCoef = CalcManaCoef(item.Mana);
                item.priority = coef * ManaCoef * item.Coef;

				//Charge, Heal, Village�� ��� ���� ��� ���� 1�� ���, �� ���¿� ���� ����� �߰��� ������� �Ѵ�.
				//���� ���� �������� ���, ���� ���� �������� �������� 2�� ����� ��� ���� �������� �ȴ�.
				if (item.Type == "Charge")
					item.priority *= CalcEnemyManaCoef();
				//���� ü�� ������ �������� ���, ���� ���� ü���� �������� 2�� ����� ��� ���� �������� �ȴ�.
				else if (item.Type == "Heal")
					item.priority *= CalcTreeHealthCoef();
				//���� ü�� ������ �������� ���, ���� ���� ü���� �������� 2�� ����� ��� ���� �������� �ȴ�.
				else if (item.Type == "Village")
					item.priority *= CalcVillageHealthCoef();

				Debug.Log(item.itemName + " priority is " + item.priority);
			}
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
        DisplayEnemyItems.Instance.insertItem(chooseItem);
		Debug.Log("Enemy choose " + chooseItem.itemName);
    }

    public void UseEnemyItem()
    {
        //���� ������ �������� ���� ��� �������� ������� �ʴ´�.
        if(isEnemyItemisEmpty()) return;

        //���� ū �켱������ ���� �������� ������ ����
        Item useItem = null;
        float maxPriority = -1.0f;

        //���� ���� üũ
		checkState();

		for(int i = 0; i < 5; i++)
        {
            Item item = EnemyItems[i];
            //���� ����� ������ ����Ʈ �� �ƹ��͵� ������� ���� ��� �׳� �Ѿ��.
            if(item == null) continue;

            //��ųʸ� ������ ���� �켱���� ������ �����Ѵ�.
			if (itemCoff.TryGetValue(item.Type, out var stateCoff))
			{
				//coef�� ������ ��ųʸ��� ���� ���� ���� Ű�� �Ͽ���, �ش� ���� ����� ��� ���� �����´�.
				float coef = stateCoff[CurrentState];
				ManaCoef = CalcManaCoef(item.Mana);
				item.priority = coef * ManaCoef * item.Coef;

                //Charge, Heal, Village�� ��� ���� ��� ���� 1�� ���, �� ���¿� ���� ����� �߰��� ������� �Ѵ�.
                //���� ���� �������� ���, ���� ���� �������� �������� 2�� ����� ��� ���� �������� �ȴ�.
				if (item.Type == "Charge")
					item.priority *= CalcEnemyManaCoef();
                //���� ü�� ������ �������� ���, ���� ���� ü���� �������� 2�� ����� ��� ���� �������� �ȴ�.
				else if (item.Type == "Heal")
					item.priority *= CalcTreeHealthCoef();
                //���� ü�� ������ �������� ���, ���� ���� ü���� �������� 2�� ����� ��� ���� �������� �ȴ�.
				else if (item.Type == "Village")
					item.priority *= CalcVillageHealthCoef();

				Debug.Log(i+1 + "'s item priority is " + item.priority);
			}
            //����� �������� �켱������ ���� ū ���, �ش� �������� �����Ѵ�.
            if(maxPriority < item.priority)
            {
                maxPriority = item.priority;
                useItem = item;
            }
		}

        //���� ���� �켱������ �������� ���������� ����� �� �������� ���Ѵ�.
        //���� ���� ���°� Aggressive�̸�
        if(CurrentState == AIState.Aggressive)
        {
            //����� �������� ������ ���, �ش� �������� ������� �ʴ´�.
            if (useItem.Type == "Charge" || useItem.Type == "Defense" || useItem.Type == "Heal" || useItem.Type == "Village")
                return;
        }
        //���� ���� ���°� Defenseive�̸�
        else if(CurrentState == AIState.Defensive)
        {
			string dangerFlag = ReturnCurrentDanger();
			Debug.Log("DangerFlag is " + dangerFlag);

            //������, ��� �������� ������ ��� ������� �ʴµ�
            if (useItem.Type == "Hit" || useItem.Type == "Gimmick")
            {
                //���� ��� �������� �� �� ���
                if (isEnemyItemisFull())
                {
                    //���� ������ �� �÷��� �� �������� �ִ��� Ȯ���Ѵ�.
                    Item fgItem = ReturnFlareItem();
                    //������ ����Ѵ�.
                    if (fgItem != null) useItem = fgItem;
                    //������ �׳� ������� �ʴ´�.
                    else return;
                }
                else
                {
                    return;
                }
            }
            //���� ���� ��Ȳ�� � �������� �ľ��ϴ� �Լ�
            else
            {
                int itemFlag = 0;
                //�� �����Ȳ�� ���� ����� �������� ����
                if (dangerFlag == "100")
                {
                    if (useItem.Type != "Heal" && useItem.Type != "Defense") itemFlag = 1;
                }
                else if (dangerFlag == "010")
                {
                    if (useItem.Type != "Charge") itemFlag = 1;
                }
                else if (dangerFlag == "001")
                {
                    if (useItem.Type != "Village") itemFlag = 1;
                }
                else if (dangerFlag == "110")
                {
                    if (useItem.Type == "Village") itemFlag = 1;
                }
                else if (dangerFlag == "101")
                {
                    if (useItem.Type == "Charge") itemFlag = 1;
                }
                else if (dangerFlag == "011")
                {
                    if (useItem.Type == "Heal" && useItem.Type == "Defense") itemFlag = 1;
                }
                //���� �켱������ ���� ���� �������� ���� ��Ȳ�� ���� ���� �������� ���
                if (itemFlag == 1)
                {
					//���� ������ �� �÷��� �� �������� �ִ��� Ȯ���Ѵ�.
					Item fgItem = ReturnFlareItem();
					//������ ����Ѵ�.
					if (fgItem != null) useItem = fgItem;
					//������ �׳� ������� �ʴ´�.
					else return;
				}
			}
		}

		//���� ���õ� �������� ���� ��뷮�� ���� ���� ���������� ū ���, �������� ������� �ʴ´�.
		//Hit Mana ���� ����ϱ����� 5�� ���Ѵ�.
		if (useItem.Mana + 5 >= GameManager.Instance.EnemeyController.Mana) return;
		//�ش� �������� ����Ѵ�.
		Debug.Log("Enemy use " + useItem.itemName);
        //�켱 �� ������ ����Ʈ���� �ش� �������� null ó�� �ϰ�
        EnemyItems[EnemyItems.IndexOf(useItem)] = null;
        //UI �󿡼��� ��ư ����Ʈ������ �����Ѵ�.
		DisplayEnemyItems.Instance.removeItem(useItem.icon);
        //�׸��� �ش� �������� ȿ���� ItemFunciton �Լ��� ���� �����Ѵ�.
        ItemManager.Instance.ItemFunction(useItem.id);
    }


    //�ش� �Լ��� ������ ü��, �÷��̾��� ����, ������ ü���� Ȯ���Ͽ� �����Ȳ�� ���� 1, �ƴѰ��� 0���� ��ȯ�Ѵ�.
    //�� - - - ���� ù��°�� ������ ü��, �ι�°�� ���� ������, �������� ���� ���� ü���� ���Ѵ�.
    //���� ��� 101�� ������ ü�°� ���� ü���� ���� �����Ȳ�ΰ��� ���Ѵ�.
    public string ReturnCurrentDanger()
    {
        if (CurrentTreeHealth < 300f && CurrentEnemyMana >= 40f && CurrentEnemyVillageHealth >= 300f)
            return "100";
        else if (CurrentTreeHealth >= 300f && CurrentEnemyMana < 40f && CurrentEnemyVillageHealth >= 300f)
            return "010";
        else if (CurrentTreeHealth >= 300f && CurrentEnemyMana >= 40f && CurrentEnemyVillageHealth < 300f)
            return "001";
        else if (CurrentTreeHealth < 300f && CurrentEnemyMana < 40f && CurrentEnemyVillageHealth >= 300f)
            return "110";
        else if (CurrentTreeHealth < 300f && CurrentEnemyMana >= 40f && CurrentEnemyVillageHealth < 300f)
            return "101";
        else if (CurrentTreeHealth >= 300f && CurrentEnemyMana < 40f && CurrentEnemyVillageHealth < 300f)
            return "011";
        else if (CurrentTreeHealth < 300f && CurrentEnemyMana < 40f && CurrentEnemyVillageHealth < 300f)
            return "111";
        return "000";
    }

	public bool isEnemyItemisFull()
	{
		// EnemyItems ����Ʈ �� null�� �ƴ� ������ ������ ���
		return EnemyItems.Count(item => item != null) >= 5;
	}

    public bool isEnemyItemisEmpty()
    {
        return EnemyItems.Count(item => item == null) >= 5;
    }

	public void checkState()
    {
		//�Ǵܿ� ���� ���� �޾ƿ���
		CurrentTreeHealth = GameManager.Instance.TreeController.treeHealth;
		CurrentEnemyMana = GameManager.Instance.EnemeyController.Mana;
		CurrentEnemyVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;

        if(CurrentTreeHealth < 300f || CurrentEnemyMana < 40f || CurrentEnemyVillageHealth < 300f)
        {
            CurrentState = AIState.Defensive;
        }
        else if (CurrentTreeHealth >= 600f && CurrentEnemyMana >= 60f && CurrentEnemyVillageHealth >= 500f)
        {
            CurrentState = AIState.Aggressive;
        }
        else
        {
            CurrentState= AIState.Neutral;
        }
	}
    
    public Item ReturnFlareItem()
    {
        foreach(Item item in EnemyItems)
        {
            if(item == null) continue;
            if(item.itemName == "�÷��� ��(8)")
            {
                return item;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    float CalcManaCoef(float mana)
    {
        return (30 - mana) / 15;
    }

    float CalcEnemyManaCoef()
    {
        return 1 + ((100 - CurrentEnemyMana) / 100);
    }

    float CalcTreeHealthCoef()
    {
        return 1 + ((1000 - CurrentTreeHealth) / 1000);
    }

    float CalcVillageHealthCoef()
    {
        return 1 + ((1000 - CurrentEnemyVillageHealth) / 1000); 
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
