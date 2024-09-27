using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingleTon<CameraManager>
{
	//플레이어와 적의 카메라를 받아온다.
    public CinemachineVirtualCamera playerCamer;
	public CinemachineVirtualCamera EnemyCamer;

	//카메라 우선순위를 바꾸는 함수
	//해당 함수는 Hit 애니메이션 코루틴이 끝난 후 호출된다.
	//따라서 해당 함수가 호출 된 시기에는 이미 Turn이 교체된 상태
	public void changeCamera()
    {
        if(GameManager.Instance.Turn == 1) //현재 Turn이 적이면 적으로 카메라 전환
        {
			EnemyCamer.Priority = 10;
			playerCamer.Priority = 0;
		}
        else if(GameManager.Instance.Turn == 0)//현재 Turn이 플레이어면 플레이어로 카메라 전환
        {
			playerCamer.Priority = 10;
			EnemyCamer.Priority = 0;
		}
    }
}
