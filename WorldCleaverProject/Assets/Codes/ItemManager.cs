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

    //생성자
    public Item(int id, string itemName, string description, Sprite icon)
	{
        this.id = id;
		this.itemName = itemName;
		this.description = description;
		this.icon = icon;
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
        allItems = new List<Item>
        {
            //해당 아이템 정보들을 생성
            new Item(0, "이상한 포션", "내 기력을 일점 부분 회복시키거나 감소시킨다.", potionIcon),
            new Item(1, "사슴", "사슴이 박치기 공격을 한다. 데미지는 0% - 200% 사이 랜덤 값이다.", deerIcon),
            new Item(2, "독수리", "독수리가 상대방으로부터 아이템을 빼앗아 온다.", eagleIcon),
            new Item(3, "결투 신청", "상대방과의 미니 게임을 통해 기력을 얻거나 잃는다.", fightIcon),
            new Item(4, "플레어 건", "추가 아이템을 획득한다.", flareIcon),
            new Item(5, "홍삼", "기력을 일정 부분 회복한다.", ginsengIcon),
            new Item(6, "워그드라실 꿀", "마을 주민의 체력을 일정 부분 회복시킨다.", honeyIcon),
            new Item(7, "기름", "내 도끼에 기름을 발라서 나무에게 주는 데미지를 대폭 감소시킨다.", OilIcon),
            new Item(8, "수액", "나무에게 사용하여 나무의 체력을 회복시킨다.", sapIcon),
            new Item(9, "연막탄", "나무의 체력을 일정 턴 동안 보이지 않게 한다.", smokeIcon),
            new Item(10, "오징어 먹물", "상대방에게 던져서, 상대방이 주는 데미지를 대폭 하락시킨다.", squidIcon),
            new Item(11, "나무 방패", "나무에게 방패를 씌워, 상대방의 데미지를 감소시킨다.", treeShildIcon),
            new Item(12, "녹용", "나무에게 주는 대미지를 대폭 상승시킨다.", velvetIcon)
        };

        //플레이어의 아이템들을 저장하는 리스트
        //playerItems = new List<Item>();
        //적의 아이템들을 저장하는 리스트
        enemyItems = new List<Item>();

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

    public void ItemFunction(int flag)
    {
        if (flag == 0)
        {
            int randomValue = Random.Range(-10, 30);
            Debug.Log("Random mana charge value is " + randomValue);
            GameManager.Instance.PlayerController.setMana(randomValue);
        }
        else if (flag == 1)
        {
        }
        else if (flag == 2)
        {
        }
        else if (flag == 3)
        {
        }
        else if (flag == 4)
        {
        }
        else if (flag == 5)
        {
        }
        else if (flag == 6)
        {
        }
        else if (flag == 7)
        {
        }
        else if (flag == 8)
        {
        }
        else if (flag == 9)
        {
        }
        else if (flag == 10)
        {
        }
        else if (flag == 11)
        {
        }
        else if (flag == 12)
        {
        }
    }
}
