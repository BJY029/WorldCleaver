using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�ش� ��ũ��Ʈ�� �� ��ư�� �����Ѵ�.
public class DisplayItems : SingleTon<DisplayItems>
{ 
    //�ش� ��ư�� �Ҽӵ� icon, itemname, desc�� �����Ѵ�.
    public Image itemIcon;
    public Text itemText;
    public Text itemDescription;
    private Item assignedItem;

    //ItemManager���� �ش� �Լ��� ȣ���ؼ� �ش� ��ư�� �������� �ʱ�ȭ�Ѵ�.
    public void SetItem(Item item)
    {
        assignedItem = item;
        itemIcon.sprite = item.icon;
        itemText.text = item.itemName;
        itemDescription.text = item.description;
    }

    //�� Button�� OnClick �Լ��� ���ԵǸ�, Ŭ���� ��, �÷��̾��� ������ ����Ʈ�� ���Եǰ�
    //�ش� ��ư�� �θ��� �θ�(panel)�� ũ�⸦ 0���� �����.
    public void OnItemSelected()
    {
        ItemManager.Instance.AddItemToPlayerInventory(assignedItem);

        ClosePanel();

        //�������� ���õ� �Ŀ� ��ư�� Ȱ��ȭ �Ѵ�.
		DisplayPlayerItems.Instance.beableButtons();
	}

    //������ ���� â�� ���� �Լ�
    public void DisplayPanel()
    {
        //������ ����� ���, ������ ���� â�� ����� �ʴ´�.
        if (GameManager.Instance.Turn == 44) return;

		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.localScale = Vector3.one;
		}
	}

    //������ ���� â�� ���� �Լ�
    public void ClosePanel()
    {
		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.localScale = Vector3.zero;
		}
	}
}
