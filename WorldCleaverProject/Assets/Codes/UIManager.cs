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
	public Button HitButton; //HIT 버튼 UI

	private float InitHealth; //나무 체력의 초기 값을 저장하기 위한 변수 
	private float InitPower; //플레이어 체력의 초깃값을 저장하기 위한 변수

	private void Start()
	{
		//초기화
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPower = GameManager.Instance.PlayerController.Mana;
	}

	private void LateUpdate()
	{
		//현재 나무 체력 받아오기
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//나무 체력 슬라이더 업데이트
		treeSlider.value = curHealth / InitHealth;

		//현재 플레이어 기력 받아오기
		float curPower = GameManager.Instance.PlayerController.Mana;
		//플레이어 기력 슬리이더 업데이트
		playerSlider.value = curPower / InitPower;

		//만약 Hit 명령을 수행중이면 버튼을 비활성화 시킨다.
		if(GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
		}
		else
		{
			HitButton.interactable= true;
		}
	}
}
