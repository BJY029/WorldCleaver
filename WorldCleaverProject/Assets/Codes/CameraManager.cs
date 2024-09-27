using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingleTon<CameraManager>
{
	//�÷��̾�� ���� ī�޶� �޾ƿ´�.
    public CinemachineVirtualCamera playerCamer;
	public CinemachineVirtualCamera EnemyCamer;

	//ī�޶� �켱������ �ٲٴ� �Լ�
	//�ش� �Լ��� Hit �ִϸ��̼� �ڷ�ƾ�� ���� �� ȣ��ȴ�.
	//���� �ش� �Լ��� ȣ�� �� �ñ⿡�� �̹� Turn�� ��ü�� ����
	public void changeCamera()
    {
        if(GameManager.Instance.Turn == 1) //���� Turn�� ���̸� ������ ī�޶� ��ȯ
        {
			EnemyCamer.Priority = 10;
			playerCamer.Priority = 0;
		}
        else if(GameManager.Instance.Turn == 0)//���� Turn�� �÷��̾�� �÷��̾�� ī�޶� ��ȯ
        {
			playerCamer.Priority = 10;
			EnemyCamer.Priority = 0;
		}
    }
}
