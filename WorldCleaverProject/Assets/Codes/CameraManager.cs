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

	//메인 맵 캔버스
	public Canvas MainCanvas;
	//마을 맵 캔버스
	public Canvas VillageCanvers;

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
}
