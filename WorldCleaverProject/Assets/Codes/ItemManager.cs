using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//아이템 정보를 저장하는 item 클래스
public class Item
{
    public string itemName;
    public string description;
    public Sprite icon;

    //생성자
    public Item(string itemName, string description, Sprite icon)
	{
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
        allItems = new List<Item>
        {
            //해당 아이템 정보들을 생성
            new Item("Potion", "Heals 100 pw", potionIcon),
            new Item("Deer", "Give random damage", deerIcon),
            new Item("Eagel", "Take one Item from enemy", eagleIcon),
            new Item("Fight", "Play minigame to charge mana", fightIcon),
            new Item("Flare", "Acquire additional items", flareIcon),
            new Item("Ginseng", "Recovery mana", ginsengIcon),
            new Item("Honey", "Restores the health of the villagers", honeyIcon),
            new Item("Oil", "Lower my attack damage", OilIcon),
            new Item("Sap", "Heals tree Health", sapIcon),
            new Item("Smoke", "Invisible tree's health", sapIcon),
            new Item("Squid", "Lower one's opponent's attack damage", squidIcon),
            new Item("TreeShild", "Lower one's opponent's attack damage", treeShildIcon),
            new Item("Velvet", "Significant increase in damage to trees", velvetIcon)
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
        //버튼 아이템 리스트
        List<Item> choseanItems = new List<Item>();

        //아이템 버튼 개수 만큼 반복한다(3번)
        for(int i = 0; i < itemButtons.Count; i++)
        {
            //임시 아이템 버퍼
            Item randomItem;
            do
            {   
                //랜덤 함수 인덱스를 뽑아서 저장한다
                randomItem = GiveRandomItem();
            }while(choseanItems.Contains(randomItem)); //만약 리스트 안에 이미 해당 아이템이 있으면 다시 반복문 실행
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
}
