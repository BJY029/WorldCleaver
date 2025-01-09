using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//전반적인 UI를 관리하는 스크립트
//싱글톤이기에 GameManager에 바로 접근 가능하다.
public class UIManager : SingleTon<UIManager>
{
	public Slider treeSlider; //나무 체력을 나타내주는 슬라이더 UI
	public Slider playerSlider; //플레이어 기력을 나타내주는 슬라이더 UI
	public Slider EnemySlider; //적 기력을 나타내주는 슬라이더 UI
	public Button HitButton; //HIT 버튼 UI

	public Canvas VillageCanvas;
	public Canvas OppositeVillageCanvas;
	public Button ToVillageButton;
	public Button ToOppositeVillageButton;
	public Slider VillageSlider;
	public Slider OppositeVillageSlider;
	public Slider MyVillageSliderInMain;
	public Slider OppositeVillageSliderInMain;
	public GameObject ItemListPanel;

	public GameObject YouWinPanel;
	public GameObject YouLosePanel;

	private float InitHealth; //나무 체력의 초기 값을 저장하기 위한 변수 
	private float InitPlayerPower; //플레이어 체력의 초깃값을 저장하기 위한 변수
	private float InitEnemyPower; //플레이어 체력의 초깃값을 저장하기 위한 변수
	private float InitVillageHealth;//마을 체력의 초깃값을 저장하기 위한 변수
	private float InitOppositeVillageHealth;
	private CanvasGroup SliderGroup;

	//마지막 연산을 위한 플래그 설정
	private int flag = 0;

	private void Start()
	{
		YouLosePanel.SetActive(false);
		YouWinPanel.SetActive(false);

		//초기화
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPlayerPower = GameManager.Instance.PlayerController.Mana;
		InitEnemyPower = GameManager.Instance.EnemeyController.Mana;
		InitVillageHealth = VillageManager.Instance.VilageHelath;
		InitOppositeVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;

		VillageCanvas.enabled = false;
		OppositeVillageCanvas.enabled = false;
		SliderGroup = treeSlider.GetComponent<CanvasGroup>();

		playerSlider.value = InitPlayerPower;
		EnemySlider.value = InitEnemyPower;
		treeSlider.value = InitHealth;
		VillageSlider.value = InitVillageHealth;
		OppositeVillageSlider.value = InitOppositeVillageHealth;
		MyVillageSliderInMain.value = InitVillageHealth;
		OppositeVillageSliderInMain.value = InitOppositeVillageHealth;
	}

	private void Update()
	{
		//중요! 만약 게임이 종료된 경우
		if (GameManager.Instance.Turn == 44)
		{
			//게임이 누구에 의해 종료되었는지 확인한다.
			GameManager.Instance.WhoLoseGame();
			GameManager.Instance.AnimationManager.Dead();
			//Hit 버튼을 비활성화 한다.
			HitButton.interactable = false;
			//마을 전용 Canvas 비활성화
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;

			//게임 승리자를 띄우는 UI를 활성화한다.
			if(GameManager.Instance.WhoLose == 0)
			{
				YouLosePanel.SetActive(true);
			}
			else
			{
				YouWinPanel.SetActive(true);
			}

			//그리고 현재 플래그가 1이면 이미 기력 바가 초기화된 상태이므로 그대로 return한다.
			if (flag == 1) return;
			//현재 플래그가 0이면, 기력 바의 연산이 아직 이루어지기 전이다. 
			//따라서, 기력 바의 연산을 끝낸 후, 예외 처리를 하기 위해 flag 도입
			//이제 기력 바의 연산이 끝난 후에, flag 값이 1로 업데이트 되면서 return이 된다.
			else flag = 1;
		}

		//만약 현재 연막탄 아이템이 실행중이면
		if(ItemManager.Instance.smokeFlag != 0)
		{
			//게임이 끝난 경우
			if (GameManager.Instance.Turn == 44)
			{
				//연막탄 효과를 해제한다.
				SliderGroup.alpha = 1f;
			}
			else
			{
				//아니면 유지한다.
				SliderGroup.alpha = 0f;
			}
		}
		else
		{
			SliderGroup.alpha = 1f;
		}

		//현재 나무 체력 받아오기
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//나무 체력 슬라이더 업데이트
		treeSlider.value = curHealth / InitHealth;

		//그냥 Turn 값을 불러오면, 이미 Turn이 교체된 상태에서 변경이 진행되므로 버그 발생
		//따라서, 기존 Turn을 저장한 myTurn 값을 불러와서 해당 값을 통해 현재 플레이어를 판단한다.
		int Turn = GameManager.Instance.myTurn;

		//그냥 상시에 업데이트 할 수 있도록 설정
		//현재 플레이어 기력 받아오기
		float curPower_p = GameManager.Instance.PlayerController.Mana;
		//플레이어 기력 슬리이더 업데이트
		playerSlider.value = curPower_p / InitPlayerPower;

		float curPower_e = GameManager.Instance.EnemeyController.Mana;
		EnemySlider.value = curPower_e / InitEnemyPower;

		//마을 체력 슬라이더 설정
		float VillageHealth = VillageManager.Instance.VilageHelath;
		VillageSlider.value = VillageHealth / InitVillageHealth;
		MyVillageSliderInMain.value = VillageSlider.value;

		float OppositeVillageHealth = OppositeVillageManager.Instance.OppositeVillageHealth;
		OppositeVillageSlider.value = OppositeVillageHealth / InitOppositeVillageHealth;
		OppositeVillageSliderInMain.value = OppositeVillageSlider.value;


		////플레이어 턴이면
		//if (Turn == 0)
		//{
		//	//현재 플레이어 기력 받아오기
		//	float curPower = GameManager.Instance.PlayerController.Mana;
		//	//플레이어 기력 슬리이더 업데이트
		//	playerSlider.value = curPower / InitPlayerPower;
		//}
		//else if (Turn == 1) //적 턴이면
		//{
		//	float curPower = GameManager.Instance.EnemeyController.Mana;
		//	EnemySlider.value = curPower / InitEnemyPower;
		//}

		//만약 적 턴이면 버튼을 비활성화 시킨다.
		if(GameManager.Instance.Turn == 1) ToVillageButton.interactable = false;
		else if(GameManager.Instance.Turn == 0) //만약 내 턴이면
		{
			//만약 현재 아이템 Full 경고문 혹은 Empty 경고문이 뜨고 있는 상태면
			if(DisplayWarningMessage.Instance.WarningFlag == 1 || DisplayEmptyMessage.Instance.WarningFlag == 1)
			{
				//비활성화
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			else
			{
				ToVillageButton.interactable = true;
				ToOppositeVillageButton.interactable = true;
			}

			//만약 현재 아이템 선택 창이 떠 있는 경우
			if (ItemListPanel.transform.localScale == Vector3.one)
			{
				//버튼 비활성화
				ToVillageButton.interactable = false;
				ToOppositeVillageButton.interactable = false;
			}
			else
			{
				//아니면 다시 활성화
				ToVillageButton.interactable = true;
				ToOppositeVillageButton.interactable = true;
			}
		}

		//만약 플래그가 1이면 밑의 연산은 재끼고 바로 리턴한다.
		if (flag == 1) return;
		//만약 Hit 명령을 수행중이면 버튼을 비활성화 시킨다.
		if (GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
			//마을 버튼 또한 비활성화시킨다.
			ToVillageButton.interactable = false;
			ToOppositeVillageButton.interactable = false;
		}
		//적 턴인 경우 Hit 버튼을 비활성화 시킨다.
		else if(GameManager.Instance.Turn == 1)
		{
			HitButton.interactable = false;
		}
		else if(ItemManager.Instance.FuncFlag == true)
		{
			HitButton.interactable = false;
		}
		else
		{
			HitButton.interactable= true;
		}
	}
}
