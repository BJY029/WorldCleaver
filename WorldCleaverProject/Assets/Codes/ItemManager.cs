using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //��ư 3��
	public List<Button> itemButtons;
	//��� �������� �����ϴ� ����Ʈ
	public List<Item> allItems;
    //�÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    //public List<Item> playerItems;
    //�� �÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    public List<Item> enemyItems;

    //������ ��������Ʈ �����ܵ�
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

    //������ �ʱ�ȭ
    void Start()
    {
        DisplayItems.Instance.ClosePanel();
		allItems = new List<Item>
        {
            //�ش� ������ �������� ����
            new Item("�̻��� ����", "�� ����� ���� �κ� ȸ����Ű�ų� ���ҽ�Ų��.", potionIcon),
            new Item("�罿", "�罿�� ��ġ�� ������ �Ѵ�. �������� 0% - 200% ���� ���� ���̴�.", deerIcon),
            new Item("������", "�������� �������κ��� �������� ���Ѿ� �´�.", eagleIcon),
            new Item("���� ��û", "������� �̴� ������ ���� ����� ��ų� �Ҵ´�.", fightIcon),
            new Item("�÷��� ��", "�߰� �������� ȹ���Ѵ�.", flareIcon),
            new Item("ȫ��", "����� ���� �κ� ȸ���Ѵ�.", ginsengIcon),
            new Item("���׵��� ��", "���� �ֹ��� ü���� ���� �κ� ȸ����Ų��.", honeyIcon),
            new Item("�⸧", "�� ������ �⸧�� �߶� �������� �ִ� �������� ���� ���ҽ�Ų��.", OilIcon),
            new Item("����", "�������� ����Ͽ� ������ ü���� ȸ����Ų��.", sapIcon),
            new Item("����ź", "������ ü���� ���� �� ���� ������ �ʰ� �Ѵ�.", sapIcon),
            new Item("��¡�� �Թ�", "���濡�� ������, ������ �ִ� �������� ���� �϶���Ų��.", squidIcon),
            new Item("���� ����", "�������� ���и� ����, ������ �������� ���ҽ�Ų��.", treeShildIcon),
            new Item("���", "�������� �ִ� ������� ���� ��½�Ų��.", velvetIcon)
        };

        //�÷��̾��� �����۵��� �����ϴ� ����Ʈ
        //playerItems = new List<Item>();
        //���� �����۵��� �����ϴ� ����Ʈ
        enemyItems = new List<Item>();

        //���� ���۽�, �ش� ��ư ���� â�� ��ư ����Ʈ�� ���� ���������� �ʱ�ȭ �� �� �ֵ��� �Լ��� ȣ���Ѵ�.
        SetRandomItemsOnButtons();
    }

    //���� �������� ��ȯ�ϴ� �Լ�
    //�ߺ� �������� ��ȯ���� �ʵ��� ó���� ����� ��.
   public Item GiveRandomItem()
    {
        int randomIndex = Random.Range(0, allItems.Count);
        return allItems[randomIndex];
    }

    //�����۹�ư ����Ʈ�� �ߺ����� �Ҵ��ϴ� �Լ�
    public void SetRandomItemsOnButtons()
    {
		DisplayItems.Instance.DisplayPanel();

		//��ư ������ ����Ʈ
		List<Item> choseanItems = new List<Item>();

        //������ ��ư ���� ��ŭ �ݺ��Ѵ�(3��)
        for(int i = 0; i < itemButtons.Count; i++)
        {
            //�ӽ� ������ ����
            Item randomItem;
            do
            {   
                //���� �Լ� �ε����� �̾Ƽ� �����Ѵ�
                randomItem = GiveRandomItem();
            }while(choseanItems.Contains(randomItem)); //���� ����Ʈ �ȿ� �̹� �ش� �������� ������ �ٽ� �ݺ��� ����
            //�ش� �������� ��ư ������ ����Ʈ�� �߰��Ѵ�.
            choseanItems.Add(randomItem);

            //�ش� ��ư �ε����� ����� �������� DisplayItem�� set�Լ����� �Ѱܼ� ǥ���Ѵ�.
            DisplayItems display = itemButtons[i].GetComponent<DisplayItems>();
            display.SetItem(randomItem);
        }
    }

    //�÷��̾ ������ �������� �÷��̾� ����Ʈ�� �߰��ϴ� �Լ�
    public void AddItemToPlayerInventory(Item item)
    {
        //playerItems.Add(item);
        //����Ʈ�� �������� �ʰ� �ش� ������ ������ �÷��̾� ������ ���� �Լ��� �ٷ� �ű��.
        DisplayPlayerItems.Instance.insertItem(item);
        Debug.Log($"{item.itemName} �������� �κ��丮�� �߰��߽��ϴ�!");
    }
}
