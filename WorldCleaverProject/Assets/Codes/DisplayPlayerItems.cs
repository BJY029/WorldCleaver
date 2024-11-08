using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class DisplayPlayerItems : SingleTon<DisplayPlayerItems>
{
	//��ư ����Ʈ �Ҵ�
    public List<Button> playerItems = new List<Button>();


	private void Start()
	{
		//�ش� �ڽĵ��� ��ư ����ŭ ��ư�� �Ҵ��Ѵ�.
		playerItems.AddRange(GetComponentsInChildren<Button>());

		//�׸��� ��� ��ư�� ��Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].enabled = false;
		}
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

	//��� ��ư�� ��Ȱ��ȭ �ϴ� �Լ�
	public void disableButtons()
	{
		//�׸��� ��� ��ư�� ��Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].enabled = false;
		}
	}

	//�ش� ��ư�� �̹��� ������Ʈ�� �����ϴ� ��쿡�� ��ư�� Ȱ��ȭ ��Ű�� �Լ�
	public void beableButtons()
	{
		//�׸��� ��� ��ư�� Ȱ��ȭ ��Ų��.
		for (int i = 0; i < playerItems.Count; i++)
		{
			Sprite sprite = playerItems[i].GetComponent<Image>().sprite;
			if (sprite != null) playerItems[i].enabled = true;
		}
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
	}

	//�������� ���õǾ��� ��, ���� �� ��� ���� �Լ�
	public void removeItem(int idx)
	{
		//�ش� ��ư�� �������� ���� �����ϰ�, �����Ѵ�.
		//������ �������� �ؿ���, �ش� �������� �÷��׸� ã�� �� ���ȴ�.
		Sprite sprite = playerItems[idx].GetComponent<Image>().sprite;
		//�������� ���� ���� �����Ѵ�.
		playerItems[idx].GetComponentInChildren<Image>().sprite = null;
		ToolTipsManager script = playerItems[idx].GetComponentInChildren<ToolTipsManager>();
		script.itemDesc = null;
		script.itemName = null;
		playerItems[idx].enabled = false;

		//�÷��� �ʱ�ȭ
		int flag = -1;
		for(int i = 0; i < ItemManager.Instance.allItems.Count;i++)
		{
			//���� �̹����� ������ �ش� �ε����� �÷��׿� ����
			if (ItemManager.Instance.allItems[i].icon == sprite)
			{
				flag = i;
				break;
			}
		}

		//ItemManager�� �÷��� ���� �Ѱܼ� ��� ����
		ItemManager.Instance.ItemFunction(flag);
	}
}
