using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
}
