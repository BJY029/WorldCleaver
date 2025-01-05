using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingleTon<CameraManager>
{
	//플레이어와 적의 카메라를 받아온다.
    public CinemachineVirtualCamera playerCamera;
	public CinemachineVirtualCamera EnemyCamera;
	public CinemachineVirtualCamera VillageCamera;
	public CinemachineVirtualCamera OppositeVillageCamera;
	public CinemachineVirtualCamera HorseCamera;

	CinemachineBrain brain;
	CinemachineBlendDefinition originalBlend;

	//메인 맵 캔버스
	public Canvas MainCanvas;
	//마을 맵 캔버스
	public Canvas VillageCanvers;
	//적 마을 맵 캔버스
	public Canvas OppositeVillageCanvers;
	//결투신청 맵 캔버스
	public Canvas HorseCanvers;

	private void Start()
	{
		//본래 카메라를 cut으로 움직이도록 설정했으나, Cinemachine이 기본적으로
		//Blend 설정을 사용하여 카메라 간 전환을 부드럽게 처리한다.
		//따라서 적 AI가 함수를 통해 카메라를 변경할 시, cut이 아닌 ease로 부드럽게 전환이 이루어지게 된다.
		//그리하여, 카메라 전환이 발생시, Cinemachine의 Blend 설정을 cut로 변경하고, 다시 되돌리는 방식을 사용한다.

		//Cinemachine Brain 참조
		brain = Camera.main.GetComponent<CinemachineBrain>();

		//Blend 설정 저장
		originalBlend = brain.m_DefaultBlend;
	}

	//카메라 우선순위를 바꾸는 함수
	//해당 함수는 Hit 애니메이션 코루틴이 끝난 후 호출된다.
	//따라서 해당 함수가 호출 된 시기에는 이미 Turn이 교체된 상태
	public void changeCamera()
    {
        if(GameManager.Instance.Turn == 1) //현재 Turn이 적이면 적으로 카메라 전환
        {
			EnemyCamera.Priority = 10;
			playerCamera.Priority = 0;
		}
        else if(GameManager.Instance.Turn == 0)//현재 Turn이 플레이어면 플레이어로 카메라 전환
        {
			playerCamera.Priority = 10;
			EnemyCamera.Priority = 0;
		}
    }

	//마을로 카메라를 이동하는 함수
	public void goToVillage()
	{
		playerCamera.Priority = 0;
		VillageCamera.Priority = 10;
		//해당 맵에 맞는 Canvas를 활성화 시켜 준다.
		MainCanvas.enabled = false;
		VillageCanvers.enabled = true;
	}

	//다시 기존 맵으로 돌아가는 함수
	public void backToGame()
	{
		playerCamera.Priority = 10;
		VillageCamera.Priority = 0;
		//해당 맵에 맞는 Canvas를 활성화 시켜 준다.
		MainCanvas.enabled = true;
		VillageCanvers.enabled = false;
	}

	public void goToOppositeVillage()
	{
		playerCamera.Priority = 0;
		OppositeVillageCamera.Priority = 10;
		MainCanvas.enabled = false;
		OppositeVillageCanvers.enabled = true;
	}

	public void backToGameFromOppositeViilage()
	{
		playerCamera.Priority = 10;
		OppositeVillageCamera.Priority = 0;
		MainCanvas.enabled = true;
		OppositeVillageCanvers.enabled = false;
	}

	public void GoToHorse()
	{
		//전환을 cut으로 강제 전환
		brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

		if (GameManager.Instance.Turn == 0)
			playerCamera.Priority = 0;
		else if (GameManager.Instance.Turn == 1)
			EnemyCamera.Priority = 0;

		HorseCamera.Priority = 10;
		MainCanvas.enabled = false;
		HorseCanvers.enabled = true;

		//카메라 전환 후, 다시 기본 설정으로 변경
		StartCoroutine(RestoreBlend());
	}

	public void BackToGameFromHorse()
	{
		brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

		if (GameManager.Instance.Turn == 0)
			playerCamera.Priority = 10;
		else if(GameManager.Instance.Turn == 1)
			EnemyCamera.Priority = 10;
		HorseCamera.Priority = 0;
		MainCanvas.enabled = true;
		HorseCanvers.enabled = false;

		//카메라 전환 후, 다시 기본 설정으로 변경
		StartCoroutine( RestoreBlend());
	}

	private IEnumerator RestoreBlend()
	{
		yield return null;
		brain.m_DefaultBlend = originalBlend;
	}
}
