using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ������ �����ϴ� item Ŭ����
public class Item
{
    public string itemName;
    public string description;
    public Sprite icon;

    //������
    public Item(string itemName, string description, Sprite icon)
	{
		this.itemName = itemName;
		this.description = description;
		this.icon = icon;
	}
}

public class ItemManager : SingleTon<ItemManager>
{
    //��� �������� �����ϴ� ����Ʈ
    public List<Item> allItems;
    //�÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    public List<Item> playerItems;
    //�� �÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    public List<Item> enemyItems;

    //������ ��������Ʈ �����ܵ�
    public Sprite potionIcon;

    //������ �ʱ�ȭ
    void Start()
    {
        allItems = new List<Item>
        {
            //�ش� ������ �������� ����
            new Item("Potion", "Heals 100 pw", potionIcon)
        };

        playerItems = new List<Item>();
        enemyItems = new List<Item>();
    }

    //���� �������� ��ȯ�ϴ� �Լ�
    //�ߺ� �������� ��ȯ���� �ʵ��� ó���� ����� ��.
   public Item GiveRandomItem()
    {
        int randomIndex = Random.Range(0, allItems.Count);
        return allItems[randomIndex];
    }
}
