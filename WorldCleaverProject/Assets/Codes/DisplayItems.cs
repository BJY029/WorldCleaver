using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//해당 스크립트를 각 버튼에 삽입한다.
public class DisplayItems : SingleTon<DisplayItems>
{ 
    //해당 버튼에 소속된 icon, itemname, desc를 삽입한다.
    public Image itemIcon;
    public Text itemText;
    public Text itemDescription;
    private Item assignedItem;

    //ItemManager에서 해당 함수를 호출해서 해당 버튼의 정보들을 초기화한다.
    public void SetItem(Item item)
    {
        assignedItem = item;
        itemIcon.sprite = item.icon;
        itemText.text = item.itemName;
        itemDescription.text = item.description;
    }

    //각 Button의 OnClick 함수에 삽입되며, 클릭될 시, 플레이어의 아이템 리스트에 삽입되고
    //해당 버튼의 부모의 부모(panel)의 크기를 0으로 만든다.
    public void OnItemSelected()
    {
        ItemManager.Instance.AddItemToPlayerInventory(assignedItem);

        ClosePanel();

        //아이템이 선택된 후에 버튼을 활성화 한다.
		DisplayPlayerItems.Instance.beableButtons();
	}

    //아이템 선택 창을 띄우는 함수
    public void DisplayPanel()
    {
        //게임이 종료된 경우, 아이템 선택 창을 띄우지 않는다.
        if (GameManager.Instance.Turn == 44) return;

		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.localScale = Vector3.one;
		}
	}

    //아이템 선택 창을 끄는 함수
    public void ClosePanel()
    {
		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.localScale = Vector3.zero;
		}
	}
}
