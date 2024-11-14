using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingleTon<CameraManager>
{
	//�÷��̾�� ���� ī�޶� �޾ƿ´�.
    public CinemachineVirtualCamera playerCamera;
	public CinemachineVirtualCamera EnemyCamera;
	public CinemachineVirtualCamera VillageCamera;

	//���� �� ĵ����
	public Canvas MainCanvas;
	//���� �� ĵ����
	public Canvas VillageCanvers;

	//ī�޶� �켱������ �ٲٴ� �Լ�
	//�ش� �Լ��� Hit �ִϸ��̼� �ڷ�ƾ�� ���� �� ȣ��ȴ�.
	//���� �ش� �Լ��� ȣ�� �� �ñ⿡�� �̹� Turn�� ��ü�� ����
	public void changeCamera()
    {
        if(GameManager.Instance.Turn == 1) //���� Turn�� ���̸� ������ ī�޶� ��ȯ
        {
			EnemyCamera.Priority = 10;
			playerCamera.Priority = 0;
		}
        else if(GameManager.Instance.Turn == 0)//���� Turn�� �÷��̾�� �÷��̾�� ī�޶� ��ȯ
        {
			playerCamera.Priority = 10;
			EnemyCamera.Priority = 0;
		}
    }

	//������ ī�޶� �̵��ϴ� �Լ�
	public void goToVillage()
	{
		playerCamera.Priority = 0;
		VillageCamera.Priority = 10;
		//�ش� �ʿ� �´� Canvas�� Ȱ��ȭ ���� �ش�.
		MainCanvas.enabled = false;
		VillageCanvers.enabled = true;
	}

	//�ٽ� ���� ������ ���ư��� �Լ�
	public void backToGame()
	{
		playerCamera.Priority = 10;
		VillageCamera.Priority = 0;
		//�ش� �ʿ� �´� Canvas�� Ȱ��ȭ ���� �ش�.
		MainCanvas.enabled = true;
		VillageCanvers.enabled = false;
	}
}
