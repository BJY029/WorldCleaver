using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //모든 아이템을 저장하는 리스트
    public List<Item> allItems;
    //플레이어가 지니고 있는 아이템 리스트
    public List<Item> playerItems;
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
            new Item("Potion", "Heals 100 pw", potionIcon)
        };

        playerItems = new List<Item>();
        enemyItems = new List<Item>();
    }

    //랜덤 아이템을 반환하는 함수
    //중복 아이템이 반환되지 않도록 처리를 해줘야 함.
   public Item GiveRandomItem()
    {
        int randomIndex = Random.Range(0, allItems.Count);
        return allItems[randomIndex];
    }
}
