using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayPlayerItems :MonoBehaviour
{
	//버튼 리스트 할당
    public List<Button> playerItems = new List<Button>();
	public List<Item> PlayerItem = new List<Item>();

	public Item checkItem;

	public bool BlockButton;

	private void Start()
	{
		checkItem = null;
		//해당 자식들의 버튼 수만큼 버튼을 할당한다.
		playerItems.AddRange(GetComponentsInChildren<Button>());

		//그리고 모든 버튼을 비활성화 시킨다.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].enabled = false;
			playerItems[i].interactable = false;
		}
		BlockButton = true;
	}

	//플레이어 아이템이 꽉 차게 되면, true를 반환하는 함수
	public bool isFull()
	{
		for (int i = 0; i < playerItems.Count; i++)
		{
			//만약 버튼들 중 하나가 비활성화 된 상태라면, 즉 빈 상태면
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
			//만약 버튼들 중 하나가 활성화 된 상태라면, 즉 빈 상태가 아니면
			if (playerItems[i].enabled)
			{
				return false;
			}
		}
		return true;
	}

	//모든 버튼을 비활성화 하는 함수
	public void disableButtons()
	{
		//그리고 모든 버튼을 비활성화 시킨다.
		for (int i = 0; i < playerItems.Count; i++)
		{
			playerItems[i].interactable = false;
		}
		GameManager.Instance.DisplayPlayerItems.BlockButton = true;
	}

	//해당 버튼의 이미지 오브젝트가 존재하는 경우에만 버튼을 활성화 시키는 함수
	public void beableButtons()
	{
		//그리고 모든 버튼을 활성화 시킨다.
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

	//아이템이 선택되었을 때 호출되는 함수
	public void insertItem(Item item)
	{
		//플래그 설정
		int flag = -1;
		//해당 반복문은 빈 버튼을 찾는 반복문이다.
		for(int i = 0;i < playerItems.Count;i++)
		{
			//만약 버튼들 중 하나가 비활성화 된 상태라면, 즉 빈 상태면
			if(!playerItems[i].enabled)
			{
				//플래그에 해당 인덱스를 저장하고 break
				flag = i;
				break;
			}
		}

		//모든 버튼이 활성화된 경우,
		if(flag == -1)
		{
			//꽉 참!
			Debug.Log("FULL Error");
			return;
		}

		//우선 해당 버튼의 아이콘 설정을 위해 이미지 오브젝트를 저장하고
		Image imgComponent = playerItems[flag].GetComponent<Image>();
		if (imgComponent != null)
		{
			//해당 이미지 오브젝트의 스프라이트에, 받아온 아이템 이미지를 저장한다.
			imgComponent.sprite = item.icon;
		}

		//그리고 해당 버튼에 있는 ToolTipsManager의 스크립트를 받아온다.
		ToolTipsManager script = playerItems[flag].GetComponentInChildren < ToolTipsManager>();
		//그리고 각 설명 문자열을 받아온 정보들로 초기화한다.
		script.itemDesc = item.description;
		script.itemName = item.itemName;
		//그 다음 버튼을 활성화 시킨다.
		playerItems[flag].enabled = true;
		playerItems[flag].interactable = true;

		//만약 플레어건 혹은 독수리 아이템이 선택되었었다면, 버튼을 비활성화 하고, 아이템 플래그를 null로 초기화
		if (checkItem != null)
		{
			//Debug.Log(checkItem.itemName);
			disableButtons();
			GameManager.Instance.DisplayPlayerItems.BlockButton = true;
			checkItem = null;
		}
		//아닌경우, 그냥 버튼들을 활성화 시킨다.
		else
		{
			beableButtons();
		}

		PlayerItem.Add(item);
	}

	//아이템이 선택되었을 때, 삭제 및 기능 수행 함수
	public void removeItem(int idx, bool isRightClick = false)
	{
		checkItem = null;
		//해당 버튼의 아이콘을 따로 저장하고, 삭제한다.
		//저장한 아이콘은 밑에서, 해당 아이템의 플레그를 찾을 때 사용된다.
		Sprite sprite = playerItems[idx].GetComponent<Image>().sprite;
		//아이템의 설명 또한 삭제한다.
		playerItems[idx].GetComponentInChildren<Image>().sprite = null;
		ToolTipsManager script = playerItems[idx].GetComponentInChildren<ToolTipsManager>();
		script.itemDesc = null;
		script.itemName = null;
		playerItems[idx].enabled = false;
		playerItems[idx].interactable = false;

		//플래그 초기화
		int flag = -1;
		Item item = null;
		for(int i = 0; i < GameManager.Instance.ItemManager.allItems.Count;i++)
		{
			//같은 이미지가 나오면 해당 인덱스를 플래그에 저장
			if (GameManager.Instance.ItemManager.allItems[i].icon == sprite)
			{
				item = GameManager.Instance.ItemManager.allItems[i];
				flag = i;
				break;
			}
		}

		PlayerItem.Remove(item);
		//우클릭 시 단순히 아이템 삭제
		if (isRightClick)
		{
			GameManager.Instance.EffectAudioManager.PlayTrash();
			return;
		}


		//만약 사용 아이템이 독수리 아이템이거나, 플레어건 아이템이면 checkItem에 포함시킨다.
		if (item.itemName == "독수리(8)" || item.itemName == "플레어 건(8)")
		{
			checkItem = item;
			//Debug.Log(checkItem.itemName);
		}
		disableButtons();

		//ItemManager에 플래그 값을 넘겨서 기능 수행
		StartCoroutine(GameManager.Instance.ItemManager.ItemFunction(flag));
		
	}

	//독수리 아이템용 함수
	//적으로부터 아이템을 빼앗아온다.
	public void StealEnemyItem()
	{
		if (GameManager.Instance.EnemyAI.isEnemyItemisEmpty())
		{
			//경고문 출력
			GameManager.Instance.DisplayEmptyMessage.ItemIsEmpty();
			checkItem = null;
			return;
		}

		//적 아이템 중 null이 아닌 아이템을 저장할 리스트
		List<Item> idxArr = new List<Item>();

		//LINQ를 이용한 필터링
		//null이 아닌 아이템만 필터링한다.
		idxArr = GameManager.Instance.EnemyAI.EnemyItems.Where(item => item != null).ToList();

		//유효한 아이템이 없으면 경고 출력
		if(idxArr.Count == 0)
		{
			//경고문 출력
			return;
		}		

		int maxRange = idxArr.Count;
		int randIdx = Random.Range(0, maxRange);

		//적으로부터 아이템을 선택한다.
		Item ChooseItem = idxArr[randIdx];

		//적 아이템 리스트에서 해당 아이템을 제거한다.
		GameManager.Instance.EnemyAI.EnemyItems[GameManager.Instance.EnemyAI.EnemyItems.IndexOf(ChooseItem)] = null;
		GameManager.Instance.DisplayEnemyItems.removeItem(ChooseItem.icon);
		//선택된 아이템을 플레이어 아이템 리스트에 배치한다.
		insertItem(ChooseItem);
	}
}
