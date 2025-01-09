using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//아이템 정보를 저장하는 item 클래스
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public string Type; //"Hit", "Charge", "Defense", "Heal", "Gimmick", "Village"
    public int Mana;
    public float Coef;
    public float priority; //Enemy에서 사용될 아이템 우선순위

	//생성자
	public Item(int id, string itemName, string description, Sprite icon, string type, int mana, float coef)
	{
        this.id = id;
		this.itemName = itemName;
		this.description = description;
		this.icon = icon;
        this.Type = type;
        this.Mana = mana;
        this.Coef = coef;
	}
}

public class ItemManager : SingleTon<ItemManager>
{
    //버튼 3개
    public List<Button> itemButtons;
    //모든 아이템을 저장하는 리스트
    public List<Item> allItems;
    //플레이어가 지니고 있는 아이템 리스트
    //public List<Item> playerItems;
    //적 플레이어가 지니고 있는 아이템 리스트
    public List<Item> enemyItems;

    public int Flag;
    public int smokeFlag;
    public bool FuncFlag;

    //적용할 스프라이트 아이콘들
    public Sprite potionIcon;
    public Sprite deerIcon;
    public Sprite eagleIcon;
    public Sprite fightIcon;
    public Sprite flareIcon;
    public Sprite ginsengIcon;
    public Sprite honeyIcon;
    public Sprite OilIcon;
    public Sprite sapIcon;
    public Sprite smokeIcon;
    public Sprite squidIcon;
    public Sprite treeShildIcon;
    public Sprite velvetIcon;

    //아이템 초기화
    void Start()
    {
        DisplayItems.Instance.ClosePanel();
        DisplayWarningMessage.Instance.closeWarningPanel();
        DisplayEmptyMessage.Instance.closeWarningPanel();

        smokeFlag = 0;

        allItems = new List<Item>
        {
            //해당 아이템 정보들을 생성
            new Item(0, "이상한 포션(0)", "내 기력을 일점 부분 회복시키거나 감소시킨다.", potionIcon, "Charge", 0, 1f),//완
            new Item(1, "사슴(15)", "사슴이 박치기 공격을 하여 큰 데미지를 입힌다.", deerIcon, "Hit", 15, 2f), //완
            new Item(2, "독수리(8)", "독수리가 상대방으로부터 아이템을 무작위로 빼앗아 온다.", eagleIcon, "Gimmick", 8, 1.7f), //완
            new Item(3, "결투 신청(10)", "상대방과의 미니 게임을 통해 기력을 얻거나 잃는다.", fightIcon, "Gimmick", 10, 1.7f),
            new Item(4, "플레어 건(8)", "추가 아이템을 획득한다.", flareIcon, "Gimmick", 8, 1.7f),//완
            new Item(5, "홍삼(0)", "기력을 일정 부분 회복한다.", ginsengIcon, "Charge", 0, 1f), //완
            new Item(6, "워그드라실 꿀(5)", "마을 주민의 체력을 일정 부분 회복시킨다.", honeyIcon, "Village", 5 ,1f), //완
            new Item(7, "기름(5)", "내 도끼에 기름을 발라서 나무에게 주는 데미지를 대폭 감소시킨다.", OilIcon, "Defense", 5, 2),//완
            new Item(8, "수액(10)", "나무에게 사용하여 나무의 체력을 회복시킨다.", sapIcon, "Heal", 10, 1f),//완
            new Item(9, "연막탄(9)", "나무의 체력을 두 사이클 동안 보이지 않게 한다.", smokeIcon, "Gimmick", 9, 1.7f), //완
            new Item(10, "오징어 먹물(12)", "상대방에게 던져서, 상대방이 다음 턴에 주는 모든 데미지를 대폭 하락시킨다.", squidIcon, "Hit", 12, 1.7f),//완
            new Item(11, "나무 방패(10)", "나무에게 방패를 씌워, 한 사이클동안 나무에게 주는 데미지를 감소시킨다.", treeShildIcon, "Defense", 10, 1.5f),//완
            new Item(12, "녹용(8)", "나무에게 주는 대미지를 대폭 상승시킨다.", velvetIcon, "Hit", 8, 1.5f)//완
        };


        //게임 시작시, 해당 버튼 선택 창의 버튼 리스트를 랜덤 아이템으로 초기화 할 수 있도록 함수를 호출한다.
        SetRandomItemsOnButtons();
    }

    //랜덤 아이템을 반환하는 함수
    //중복 아이템이 반환되지 않도록 처리를 해줘야 함.
    public Item GiveRandomItem()
    {
        int randomIndex = Random.Range(0, allItems.Count);
        return allItems[randomIndex];
    }

    //아이템버튼 리스트를 중복없게 할당하는 함수
    public void SetRandomItemsOnButtons()
    {
        DisplayItems.Instance.DisplayPanel();

        //버튼 아이템 리스트
        List<Item> choseanItems = new List<Item>();

        //아이템 버튼 개수 만큼 반복한다(3번)
        for (int i = 0; i < itemButtons.Count; i++)
        {
            //임시 아이템 버퍼
            Item randomItem;
            do
            {
                //랜덤 함수 인덱스를 뽑아서 저장한다
                randomItem = GiveRandomItem();
            } while (choseanItems.Contains(randomItem)); //만약 리스트 안에 이미 해당 아이템이 있으면 다시 반복문 실행
            //해당 아이템을 버튼 아이템 리스트에 추가한다.
            choseanItems.Add(randomItem);

            //해당 버튼 인덱스에 저장된 아이템을 DisplayItem의 set함수에게 넘겨서 표시한다.
            DisplayItems display = itemButtons[i].GetComponent<DisplayItems>();
            display.SetItem(randomItem);
        }
    }

    //플레이어가 선택한 아이템을 플레이어 리스트에 추가하는 함수
    public void AddItemToPlayerInventory(Item item)
    {
        //playerItems.Add(item);
        //리스트에 저장하지 않고 해당 아이템 정보를 플레이어 아이템 관련 함수로 바로 옮긴다.
        DisplayPlayerItems.Instance.insertItem(item);
        Debug.Log($"{item.itemName} 아이템을 인벤토리에 추가했습니다!");
    }

   
    public IEnumerator ItemFunction(int flag)
    {
        FuncFlag = true;
        Flag = flag; 
        if (flag == 0)//내 기력을 일점 부분 회복시키거나 감소시킨다.
		{
            GameManager.Instance.AnimationManager.DrinkRed();
            yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitDrinkTime);
            int randomValue = Random.Range(-10, 40);
            Debug.Log("Random mana charge value is " + randomValue);
            if(GameManager.Instance.Turn == 0)
            {
				GameManager.Instance.PlayerController.setMana(randomValue);
			}
            else if(GameManager.Instance.Turn == 1)
            {
                GameManager.Instance.EnemeyController.setMana(randomValue);
            }
        }
        else if (flag == 1)
        {
            if(GameManager.Instance.Turn == 0)
            {
                GameManager.Instance.PlayerController.setMana(-15);
				DeerController.Instance.Deer.SetActive(true);
				DeerController.Instance.DeerActivated = true;
			}
            else if(GameManager.Instance.Turn == 1)
            {
                GameManager.Instance.EnemeyController.setMana(-15);
                EnemyDeerController.Instance.Deer.SetActive(true);
                EnemyDeerController.Instance.DeerActivated = true;
            }
            
        }
        else if (flag == 2)
        {
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-8);
                DisplayPlayerItems.Instance.StealEnemyItem();
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-8);
                EnemyAI.Instance.StealPlayerItem();
			}


		}
        else if (flag == 3)
        {
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-10);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-10);

			}
            HorseManager.Instance.PrapareGame();
		}
        else if (flag == 4)//추가 아이템을 획득한다
		{
            GameManager.Instance.AnimationManager.FireFlare();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitFireTime);
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-8);
				SetRandomItemsOnButtons();
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-8);
                EnemyAI.Instance.ChooseEnemyItem();
			}
        }
        else if (flag == 5)//기력을 일정 부분 회복한다.(확정적으로 회복)
		{
            GameManager.Instance.AnimationManager.EatGin();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitDrinkTime);
			int randomValue = Random.Range(10, 20);
			Debug.Log("Random mana charge value is " + randomValue);
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(randomValue);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(randomValue);
			}
		}
        else if (flag == 6) //마을 체력을 회복시키는 아이템
        {
            GameManager.Instance.AnimationManager.DrinkHoney();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitDrinkTime);
			int randomValue = Random.Range(50, 100);
			Debug.Log("Random Village charge value is " + randomValue);
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-5);
				VillageManager.Instance.VilageHelath = randomValue;
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-5);
                OppositeVillageManager.Instance.OppositeVillageHealth = randomValue;
			}
		}
        else if (flag == 7)//내 도끼에 기름을 발라서 나무에게 주는 데미지를 대폭 감소시킨다.
		{
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-5);
                GameManager.Instance.AnimationManager.PlayerAnim.SetBool("Oil", true);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-5);
                GameManager.Instance.AnimationManager.EnemyAnim.SetBool("Oil", true);
			}
			//Do job at TreeController
		}
        else if (flag == 8)//나무에게 사용하여 나무의 체력을 회복시킨다.
		{
            GameManager.Instance.AnimationManager.Sap();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitSapTime);

			int randomValue = Random.Range(200, 350);
			Debug.Log("Random tree health charge value is " + randomValue);
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-10);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-10);
			}
			
            GameManager.Instance.TreeController.setTreeHealth(randomValue);
        }
        else if (flag == 9)
        {
            GameManager.Instance.AnimationManager.ThrowSmoke();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitThrowTime);

			//두 싸이클 동안 나무의 체력 바를 숨긴다.
			smokeFlag += 4;
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-9);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-9);
			}
		}
        //아래 두 아이템은 최종 데미지 계산에서 가하는 계수를 조정한다.
        else if (flag == 10) //오징어 먹물
        {
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-12);
				GameManager.Instance.TreeController.OppositeDamageCoef = 0.3f;
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-12);
                GameManager.Instance.TreeController.MyDamageCoef = 0.3f;
			}
			
        }
        else if (flag == 11) //나무 방패
        {
            GameManager.Instance.AnimationManager.Shild();
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-10);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-10);
			}
			GameManager.Instance.TreeController.MyDamageCoef = 0.6f;
			GameManager.Instance.TreeController.OppositeDamageCoef = 0.6f;
		}
        else if (flag == 12)//나무에게 주는 대미지를 대폭 상승시킨다.
		{
            GameManager.Instance.AnimationManager.EatDeer();
			yield return new WaitForSeconds(GameManager.Instance.AnimationManager.WaitDrinkTime);
			if (GameManager.Instance.Turn == 0)
			{
				GameManager.Instance.PlayerController.setMana(-8);
			}
			else if (GameManager.Instance.Turn == 1)
			{
				GameManager.Instance.EnemeyController.setMana(-8);
			}
			//Do job at TreeController
		}
        yield return null;

        FuncFlag = false;
	}
}
