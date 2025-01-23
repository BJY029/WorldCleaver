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
        GameManager.Instance.ItemManager.AddItemToPlayerInventory(assignedItem);

        ClosePanel();
		GameManager.Instance.UIManager.ItemSelecting = false;
        GameManager.Instance.ItemManager.flareGunFlag = false;

  //      if(GameManager.Instance.DisplayPlayerItems.checkItem != null)
  //          GameManager.Instance.DisplayPlayerItems.BlockButton = true;
		//else GameManager.Instance.DisplayPlayerItems.BlockButton = false;

		//�������� ���õ� �Ŀ� ��ư�� Ȱ��ȭ �Ѵ�.
		//DisplayPlayerItems.Instance.beableButtons();
	}

    //������ ���� â�� ���� �Լ�
    public void DisplayPanel()
    {
        //������ ����� ���, ������ ���� â�� ����� �ʴ´�.
        if (GameManager.Instance.Turn == 44) return;

        StartCoroutine(DisplayDelay());
	}

    IEnumerator DisplayDelay()
    {
        //GameManager.Instance.UIManager.ItemSelecting = true;
        yield return new WaitForSeconds(1f);
		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.localScale = Vector3.one;
		}
        //GameManager.Instance.UIManager.ItemSelecting = false;
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
