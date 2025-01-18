using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayPlayerItems :MonoBehaviour
{
	//��ư ����Ʈ �Ҵ�
    public List<Button> playerItems = new List<Button>();
	public List<Item> PlayerItem = new List<Item>();

	public Item checkItem;

	public bool BlockButton;

	private void Start()
	{
		checkItem = null;
		//�ش� �ڽĵ��� ��ư ����ŭ ��ư�� �Ҵ��Ѵ�.
		playerItems.AddRange(GetComponentsInChildren<Button>());

		//�׸��� ��� ��ư�� ��Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].enabled = false;
			playerItems[i].interactable = false;
		}
		BlockButton = true;
	}

	//�÷��̾� �������� �� ���� �Ǹ�, true�� ��ȯ�ϴ� �Լ�
	public bool isFull()
	{
		for (int i = 0; i < playerItems.Count; i++)
		{
			//���� ��ư�� �� �ϳ��� ��Ȱ��ȭ �� ���¶��, �� �� ���¸�
			if (!playerItems[i].enabled)
			{
				return false;				
			}
		}
		return true;
	}

	public bool isEmpty()
	{
		for (int i = 0; i < playerItems.Count; i++)
		{
			//���� ��ư�� �� �ϳ��� Ȱ��ȭ �� ���¶��, �� �� ���°� �ƴϸ�
			if (playerItems[i].enabled)
			{
				return false;
			}
		}
		return true;
	}

	//��� ��ư�� ��Ȱ��ȭ �ϴ� �Լ�
	public void disableButtons()
	{
		//�׸��� ��� ��ư�� ��Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].interactable = false;
		}
		GameManager.Instance.DisplayPlayerItems.BlockButton = true;
	}

	//�ش� ��ư�� �̹��� ������Ʈ�� �����ϴ� ��쿡�� ��ư�� Ȱ��ȭ ��Ű�� �Լ�
	public void beableButtons()
	{
		//�׸��� ��� ��ư�� Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			Sprite sprite = playerItems[i].GetComponent<Image>().sprite;
			if (sprite != null)
			{
				Debug.Log("button true");
				playerItems[i].interactable = true;
			}
		}
		GameManager.Instance.DisplayPlayerItems.BlockButton = false;
	}

	//�������� ���õǾ��� �� ȣ��Ǵ� �Լ�
	public void insertItem(Item item)
	{
		//�÷��� ����
		int flag = -1;
		//�ش� �ݺ����� �� ��ư�� ã�� �ݺ����̴�.
		for(int i = 0;i < playerItems.Count;i++)
		{
			//���� ��ư�� �� �ϳ��� ��Ȱ��ȭ �� ���¶��, �� �� ���¸�
			if(!playerItems[i].enabled)
			{
				//�÷��׿� �ش� �ε����� �����ϰ� break
				flag = i;
				break;
			}
		}

		//��� ��ư�� Ȱ��ȭ�� ���,
		if(flag == -1)
		{
			//�� ��!
			Debug.Log("FULL Error");
			return;
		}

		//�켱 �ش� ��ư�� ������ ������ ���� �̹��� ������Ʈ�� �����ϰ�
		Image imgComponent = playerItems[flag].GetComponent<Image>();
		if (imgComponent != null)
		{
			//�ش� �̹��� ������Ʈ�� ��������Ʈ��, �޾ƿ� ������ �̹����� �����Ѵ�.
			imgComponent.sprite = item.icon;
		}

		//�׸��� �ش� ��ư�� �ִ� ToolTipsManager�� ��ũ��Ʈ�� �޾ƿ´�.
		ToolTipsManager script = playerItems[flag].GetComponentInChildren < ToolTipsManager>();
		//�׸��� �� ���� ���ڿ��� �޾ƿ� ������� �ʱ�ȭ�Ѵ�.
		script.itemDesc = item.description;
		script.itemName = item.itemName;
		//�� ���� ��ư�� Ȱ��ȭ ��Ų��.
		playerItems[flag].enabled = true;
		playerItems[flag].interactable = true;

		//���� �÷���� Ȥ�� ������ �������� ���õǾ����ٸ�, ��ư�� ��Ȱ��ȭ �ϰ�, ������ �÷��׸� null�� �ʱ�ȭ
		if (checkItem != null)
		{
			//Debug.Log(checkItem.itemName);
			disableButtons();
			GameManager.Instance.DisplayPlayerItems.BlockButton = true;
			checkItem = null;
		}
		//�ƴѰ��, �׳� ��ư���� Ȱ��ȭ ��Ų��.
		else
		{
			beableButtons();
		}

		PlayerItem.Add(item);
	}

	//�������� ���õǾ��� ��, ���� �� ��� ���� �Լ�
	public void removeItem(int idx, bool isRightClick = false)
	{
		checkItem = null;
		//�ش� ��ư�� �������� ���� �����ϰ�, �����Ѵ�.
		//������ �������� �ؿ���, �ش� �������� �÷��׸� ã�� �� ���ȴ�.
		Sprite sprite = playerItems[idx].GetComponent<Image>().sprite;
		//�������� ���� ���� �����Ѵ�.
		playerItems[idx].GetComponentInChildren<Image>().sprite = null;
		ToolTipsManager script = playerItems[idx].GetComponentInChildren<ToolTipsManager>();
		script.itemDesc = null;
		script.itemName = null;
		playerItems[idx].enabled = false;
		playerItems[idx].interactable = false;

		//�÷��� �ʱ�ȭ
		int flag = -1;
		Item item = null;
		for(int i = 0; i < GameManager.Instance.ItemManager.allItems.Count;i++)
		{
			//���� �̹����� ������ �ش� �ε����� �÷��׿� ����
			if (GameManager.Instance.ItemManager.allItems[i].icon == sprite)
			{
				item = GameManager.Instance.ItemManager.allItems[i];
				flag = i;
				break;
			}
		}

		PlayerItem.Remove(item);
		//��Ŭ�� �� �ܼ��� ������ ����
		if (isRightClick)
		{
			GameManager.Instance.EffectAudioManager.PlayTrash();
			return;
		}


		//���� ��� �������� ������ �������̰ų�, �÷���� �������̸� checkItem�� ���Խ�Ų��.
		if (item.itemName == "������(8)" || item.itemName == "�÷��� ��(8)")
		{
			checkItem = item;
			//Debug.Log(checkItem.itemName);
		}
		disableButtons();

		//ItemManager�� �÷��� ���� �Ѱܼ� ��� ����
		StartCoroutine(GameManager.Instance.ItemManager.ItemFunction(flag));
		
	}

	//������ �����ۿ� �Լ�
	//�����κ��� �������� ���Ѿƿ´�.
	public void StealEnemyItem()
	{
		if (GameManager.Instance.EnemyAI.isEnemyItemisEmpty())
		{
			//��� ���
			GameManager.Instance.DisplayEmptyMessage.ItemIsEmpty();
			checkItem = null;
			return;
		}

		//�� ������ �� null�� �ƴ� �������� ������ ����Ʈ
		List<Item> idxArr = new List<Item>();

		//LINQ�� �̿��� ���͸�
		//null�� �ƴ� �����۸� ���͸��Ѵ�.
		idxArr = GameManager.Instance.EnemyAI.EnemyItems.Where(item => item != null).ToList();

		//��ȿ�� �������� ������ ��� ���
		if(idxArr.Count == 0)
		{
			//��� ���
			return;
		}		

		int maxRange = idxArr.Count;
		int randIdx = Random.Range(0, maxRange);

		//�����κ��� �������� �����Ѵ�.
		Item ChooseItem = idxArr[randIdx];

		//�� ������ ����Ʈ���� �ش� �������� �����Ѵ�.
		GameManager.Instance.EnemyAI.EnemyItems[GameManager.Instance.EnemyAI.EnemyItems.IndexOf(ChooseItem)] = null;
		GameManager.Instance.DisplayEnemyItems.removeItem(ChooseItem.icon);
		//���õ� �������� �÷��̾� ������ ����Ʈ�� ��ġ�Ѵ�.
		insertItem(ChooseItem);
	}
}
