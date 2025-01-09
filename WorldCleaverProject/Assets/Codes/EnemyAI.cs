using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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

    public void insertItem(Item item)
    {
        StartCoroutine(InsertItem(item));
	}

    IEnumerator InsertItem(Item item)
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.Turn != 44)
        {
            //if (chooseItem.Mana > CurrentEnemyMana || chooseItem == null) return;
            //선택된 아이템을 아이템 리스트에 삽입한다.
            int emptySlotIndex = EnemyItems.IndexOf(null);
            if (emptySlotIndex != -1) // 빈 슬롯이 있는 경우
            {
                EnemyItems[emptySlotIndex] = item;
            }
            DisplayEnemyItems.Instance.insertItem(item);
            Debug.Log("Enemy choose " + item.itemName);
        }
	}

	public void ChooseEnemyItem()
    {
		if (GameManager.Instance.Turn == 44) return;
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

				//Charge, Heal, Village의 경우 고유 계수 값이 1인 대신, 각 상태에 따른 계수를 추가로 곱해줘야 한다.
				//마나 충전 아이템의 경우, 현재 마나 보유량이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				if (item.Type == "Charge")
					item.priority *= CalcEnemyManaCoef();
				//나무 체력 충전의 아이템인 경우, 현재 나무 체력이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				else if (item.Type == "Heal")
					item.priority *= CalcTreeHealthCoef();
				//마을 체력 충전의 아이템인 경우, 현재 마을 체력이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				else if (item.Type == "Village")
					item.priority *= CalcVillageHealthCoef();

				Debug.Log(item.itemName + " priority is " + item.priority);
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
		insertItem(chooseItem);
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

                //Charge, Heal, Village의 경우 고유 계수 값이 1인 대신, 각 상태에 따른 계수를 추가로 곱해줘야 한다.
                //마나 충전 아이템의 경우, 현재 마나 보유량이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				if (item.Type == "Charge")
					item.priority *= CalcEnemyManaCoef();
                //나무 체력 충전의 아이템인 경우, 현재 나무 체력이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				else if (item.Type == "Heal")
					item.priority *= CalcTreeHealthCoef();
                //마을 체력 충전의 아이템인 경우, 현재 마을 체력이 낮을수록 2에 가까운 계수 값이 곱해지게 된다.
				else if (item.Type == "Village")
					item.priority *= CalcVillageHealthCoef();

				Debug.Log(i+1 + "'s item priority is " + item.priority);
			}
            //계산한 아이템의 우선순위가 가장 큰 경우, 해당 아이템을 저장한다.
            if(maxPriority < item.priority)
            {
                maxPriority = item.priority;
                useItem = item;
            }
		}

        //가장 높은 우선순위의 아이템을 최종적으로 사용할 지 안할지를 정한다.
        //만약 현재 상태가 Aggressive이면
        if(CurrentState == AIState.Aggressive)
        {
            //방어적 아이템이 선정된 경우, 해당 아이템을 사용하지 않는다.
            if (useItem.Type == "Charge" || useItem.Type == "Defense" || useItem.Type == "Heal" || useItem.Type == "Village")
                return;
        }
        //만약 현재 상태가 Defenseive이면
        else if(CurrentState == AIState.Defensive)
        {
			string dangerFlag = ReturnCurrentDanger();
			Debug.Log("DangerFlag is " + dangerFlag);

            //공격적, 기믹 아이템이 선정된 경우 사용하지 않는데
            if (useItem.Type == "Hit" || useItem.Type == "Gimmick")
            {
                //만약 모든 아이템이 꽉 찬 경우
                if (isEnemyItemisFull())
                {
                    //보유 아이템 중 플레어 건 아이템이 있는지 확인한다.
                    Item fgItem = ReturnFlareItem();
                    //있으면 사용한다.
                    if (fgItem != null) useItem = fgItem;
                    //없으면 그냥 사용하지 않는다.
                    else return;
                }
                else
                {
                    return;
                }
            }
            //현재 위기 상황이 어떤 것인지를 파악하는 함수
            else
            {
                int itemFlag = 0;
                //각 위기상황에 따라서 사용할 아이템을 선정
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
                //만약 우선순위가 가장 높은 아이템이 현재 상황에 맞지 않은 아이템일 경우
                if (itemFlag == 1)
                {
					//보유 아이템 중 플레어 건 아이템, 혹은 독수리 아이템이 있는지 확인한다.
					Item fgItem = ReturnFlareItem();
                    Item egItem = ReturnEagleItem();
                    //있으면 사용한다.
                    if (fgItem != null || egItem != null)
                    {
                        if (fgItem != null && egItem != null)
                        {
							int RanIdx = Random.Range(0, 1);
							if (RanIdx == 0) useItem = fgItem;
							else if (RanIdx == 1) useItem = egItem;
                        }
                        else if(egItem != null)
                        {
                            useItem = egItem;
                        }
                        else
                        {
							useItem = fgItem;
						}
                    }
                    //없으면 그냥 사용하지 않는다.
                    else return;
				}
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
        StartCoroutine(ItemManager.Instance.ItemFunction(useItem.id));
    }


    //해당 함수는 나무의 체력, 플레이어의 마나, 마을의 체력을 확인하여 위기상황인 것을 1, 아닌것을 0으로 반환한다.
    //즉 - - - 에서 첫번째는 나무의 체력, 두번째는 적의 마나량, 마지막은 적의 마을 체력을 뜻한다.
    //예를 들어 101은 나무의 체력과 마을 체력이 현재 위기상황인것을 뜻한다.
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
        
        //연막탄이 펼처진 경우, 실제 값 기준 -300 ~ 300 사이의 값을 랜덤으로 받도록 설정한다.
        if(ItemManager.Instance.smokeFlag != 0)
        {
            float minValue = CurrentTreeHealth - 300f;
            if (minValue <= 0) minValue = 10f;
            float maxValue = CurrentTreeHealth + 300f;
            if (maxValue >= 1000) maxValue = 1000f;
            CurrentTreeHealth = Random.Range(minValue, maxValue);
        }


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
            if(item.itemName == "플레어 건(8)")
            {
                return item;
            }
        }
        return null;
    }

	public Item ReturnEagleItem()
	{
		foreach (Item item in EnemyItems)
		{
			if (item == null) continue;
			if (item.itemName == "독수리(8)")
			{
				return item;
			}
		}
		return null;
	}

	public void StealPlayerItem()
    {
        //만약 플레이어가 가진 아이템이 없으면 아무것도 실행하지 않는다.
        if (DisplayPlayerItems.Instance.isEmpty())
        {
            //경고문 출력
            return;
        }

        //플레이어 아이템 리스트는 버튼 리스트로 되어있다.ㅜㅜ
        //플레이어가 가진 아이템들을 버튼으로 받아온다.
        List<Button> items = new List<Button>();
        for (int i = 0; i < DisplayPlayerItems.Instance.playerItems.Count; i++)
        {
            if (DisplayPlayerItems.Instance.playerItems[i].enabled == false || DisplayPlayerItems.Instance.playerItems[i] == null)
            {
                continue;
            }
            items.Add(DisplayPlayerItems.Instance.playerItems[i]);
        }

        //만약 가진 아이템이 없으면 아무것도 하지 않는다.(오류 방지 2차검증)
        if (items.Count == 0) return;

        //가진 아이템 중 무작위 선출을 위한 설정 
        int maxRange = items.Count;
        Debug.Log("내 아이템 수 : " + maxRange);
        int randIdx = Random.Range(0, maxRange);

        //선정된 아이템을 버튼으로 저장
        Button ChooseItem = items[randIdx];
        //해당 인덱스를 저장한다.
        int idx = DisplayPlayerItems.Instance.playerItems.IndexOf(ChooseItem);

        //item 형식으로 변환하기 위한 작업, 이미지 스프라이트를 통해서 item을 가져온다.
        Sprite sprite = DisplayPlayerItems.Instance.playerItems[idx].GetComponent<Image>().sprite;
		//Linq를 사용한다.
		var result = ItemManager.Instance.allItems
	 .Where(item => item != null && item.icon != null) // null 값을 걸러냄
	 .Select((item, index) => new { Item = item, Index = index })
	 .FirstOrDefault(x => x.Item.icon == sprite);
		//만약 아이템이 모두 null이면 오류를 반환한다.
		if (result == null)
        {
            Debug.Log("Error");
            return;
        }

        //아이템 삭제, 버튼 리스트에서 삭제를 하고
        DisplayPlayerItems.Instance.playerItems[idx].GetComponentInChildren<Image>().sprite = null;
        //따로 관리하는 아이템 리스트에서도 삭제한다.
		if (!DisplayPlayerItems.Instance.PlayerItem.Contains(result.Item))
		{
			Debug.LogError("Item not found in PlayerItem list!");
			return;
		}
		DisplayPlayerItems.Instance.PlayerItem.Remove(result.Item);
        //또한 해당 버튼에 달린 설명 스크립트도 삭제한다.
		ToolTipsManager script = DisplayPlayerItems.Instance.playerItems[idx].GetComponentInChildren<ToolTipsManager>();
        script.itemDesc = null;
        script.itemName = null;
        //해당 버튼을 비활성화 시킨다.
		DisplayPlayerItems.Instance.playerItems[idx].interactable = false;
		DisplayPlayerItems.Instance.playerItems[idx].enabled = false;

		//insert
		insertItem(result.Item);
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
		if (ItemManager.Instance.smokeFlag != 0)
		{
			float minValue = CurrentTreeHealth - 300f;
			if (minValue <= 0) minValue = 10f;
			float maxValue = CurrentTreeHealth + 300f;
			if (maxValue >= 1000) maxValue = 1000f;
			CurrentTreeHealth = Random.Range(minValue, maxValue);
		}

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

        //결투 신청 아이템의 경우, 해당 작업 수행 중에, HIT을 수행하지 못하도록
        //코루틴의 시간 흐름을 잠시 멈춘다.
        float elapsed = 0f;
        while (elapsed < waitSecond)
        {
            if (!HorseManager.Instance.EnemyAIStop)
            {
                elapsed += Time.deltaTime;
            }
            yield return null;
        }
		GameManager.Instance.AnimationManager.Hit();
		GameManager.Instance.Hit();
	}
}
