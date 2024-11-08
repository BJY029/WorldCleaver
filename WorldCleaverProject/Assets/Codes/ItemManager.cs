using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ ������ �����ϴ� item Ŭ����
public class Item
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;

    //������
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
        DisplayWarningMessage.Instance.closeWarningPanel();
        allItems = new List<Item>
        {
            //�ش� ������ �������� ����
            new Item(0, "�̻��� ����", "�� ����� ���� �κ� ȸ����Ű�ų� ���ҽ�Ų��.", potionIcon),
            new Item(1, "�罿", "�罿�� ��ġ�� ������ �Ѵ�. �������� 0% - 200% ���� ���� ���̴�.", deerIcon),
            new Item(2, "������", "�������� �������κ��� �������� ���Ѿ� �´�.", eagleIcon),
            new Item(3, "���� ��û", "������� �̴� ������ ���� ����� ��ų� �Ҵ´�.", fightIcon),
            new Item(4, "�÷��� ��", "�߰� �������� ȹ���Ѵ�.", flareIcon),
            new Item(5, "ȫ��", "����� ���� �κ� ȸ���Ѵ�.", ginsengIcon),
            new Item(6, "���׵��� ��", "���� �ֹ��� ü���� ���� �κ� ȸ����Ų��.", honeyIcon),
            new Item(7, "�⸧", "�� ������ �⸧�� �߶� �������� �ִ� �������� ���� ���ҽ�Ų��.", OilIcon),
            new Item(8, "����", "�������� ����Ͽ� ������ ü���� ȸ����Ų��.", sapIcon),
            new Item(9, "����ź", "������ ü���� ���� �� ���� ������ �ʰ� �Ѵ�.", smokeIcon),
            new Item(10, "��¡�� �Թ�", "���濡�� ������, ������ �ִ� �������� ���� �϶���Ų��.", squidIcon),
            new Item(11, "���� ����", "�������� ���и� ����, ������ �������� ���ҽ�Ų��.", treeShildIcon),
            new Item(12, "���", "�������� �ִ� ������� ���� ��½�Ų��.", velvetIcon)
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
        for (int i = 0; i < itemButtons.Count; i++)
        {
            //�ӽ� ������ ����
            Item randomItem;
            do
            {
                //���� �Լ� �ε����� �̾Ƽ� �����Ѵ�
                randomItem = GiveRandomItem();
            } while (choseanItems.Contains(randomItem)); //���� ����Ʈ �ȿ� �̹� �ش� �������� ������ �ٽ� �ݺ��� ����
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
