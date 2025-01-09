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
    public string Type; //"Hit", "Charge", "Defense", "Heal", "Gimmick", "Village"
    public int Mana;
    public float Coef;
    public float priority; //Enemy���� ���� ������ �켱����

	//������
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
    //��ư 3��
    public List<Button> itemButtons;
    //��� �������� �����ϴ� ����Ʈ
    public List<Item> allItems;
    //�÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    //public List<Item> playerItems;
    //�� �÷��̾ ���ϰ� �ִ� ������ ����Ʈ
    public List<Item> enemyItems;

    public int Flag;
    public int smokeFlag;
    public bool FuncFlag;

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
        DisplayEmptyMessage.Instance.closeWarningPanel();

        smokeFlag = 0;

        allItems = new List<Item>
        {
            //�ش� ������ �������� ����
            new Item(0, "�̻��� ����(0)", "�� ����� ���� �κ� ȸ����Ű�ų� ���ҽ�Ų��.", potionIcon, "Charge", 0, 1f),//��
            new Item(1, "�罿(15)", "�罿�� ��ġ�� ������ �Ͽ� ū �������� ������.", deerIcon, "Hit", 15, 2f), //��
            new Item(2, "������(8)", "�������� �������κ��� �������� �������� ���Ѿ� �´�.", eagleIcon, "Gimmick", 8, 1.7f), //��
            new Item(3, "���� ��û(10)", "������� �̴� ������ ���� ����� ��ų� �Ҵ´�.", fightIcon, "Gimmick", 10, 1.7f),
            new Item(4, "�÷��� ��(8)", "�߰� �������� ȹ���Ѵ�.", flareIcon, "Gimmick", 8, 1.7f),//��
            new Item(5, "ȫ��(0)", "����� ���� �κ� ȸ���Ѵ�.", ginsengIcon, "Charge", 0, 1f), //��
            new Item(6, "���׵��� ��(5)", "���� �ֹ��� ü���� ���� �κ� ȸ����Ų��.", honeyIcon, "Village", 5 ,1f), //��
            new Item(7, "�⸧(5)", "�� ������ �⸧�� �߶� �������� �ִ� �������� ���� ���ҽ�Ų��.", OilIcon, "Defense", 5, 2),//��
            new Item(8, "����(10)", "�������� ����Ͽ� ������ ü���� ȸ����Ų��.", sapIcon, "Heal", 10, 1f),//��
            new Item(9, "����ź(9)", "������ ü���� �� ����Ŭ ���� ������ �ʰ� �Ѵ�.", smokeIcon, "Gimmick", 9, 1.7f), //��
            new Item(10, "��¡�� �Թ�(12)", "���濡�� ������, ������ ���� �Ͽ� �ִ� ��� �������� ���� �϶���Ų��.", squidIcon, "Hit", 12, 1.7f),//��
            new Item(11, "���� ����(10)", "�������� ���и� ����, �� ����Ŭ���� �������� �ִ� �������� ���ҽ�Ų��.", treeShildIcon, "Defense", 10, 1.5f),//��
            new Item(12, "���(8)", "�������� �ִ� ������� ���� ��½�Ų��.", velvetIcon, "Hit", 8, 1.5f)//��
        };


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

   
    public IEnumerator ItemFunction(int flag)
    {
        FuncFlag = true;
        Flag = flag; 
        if (flag == 0)//�� ����� ���� �κ� ȸ����Ű�ų� ���ҽ�Ų��.
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
        else if (flag == 4)//�߰� �������� ȹ���Ѵ�
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
        else if (flag == 5)//����� ���� �κ� ȸ���Ѵ�.(Ȯ�������� ȸ��)
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
        else if (flag == 6) //���� ü���� ȸ����Ű�� ������
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
        else if (flag == 7)//�� ������ �⸧�� �߶� �������� �ִ� �������� ���� ���ҽ�Ų��.
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
        else if (flag == 8)//�������� ����Ͽ� ������ ü���� ȸ����Ų��.
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

			//�� ����Ŭ ���� ������ ü�� �ٸ� �����.
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
        //�Ʒ� �� �������� ���� ������ ��꿡�� ���ϴ� ����� �����Ѵ�.
        else if (flag == 10) //��¡�� �Թ�
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
        else if (flag == 11) //���� ����
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
        else if (flag == 12)//�������� �ִ� ������� ���� ��½�Ų��.
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
