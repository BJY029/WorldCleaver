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
	public CinemachineVirtualCamera OppositeVillageCamera;
	public CinemachineVirtualCamera HorseCamera;

	CinemachineBrain brain;
	CinemachineBlendDefinition originalBlend;

	//���� �� ĵ����
	public Canvas MainCanvas;
	//���� �� ĵ����
	public Canvas VillageCanvers;
	//�� ���� �� ĵ����
	public Canvas OppositeVillageCanvers;
	//������û �� ĵ����
	public Canvas HorseCanvers;

	private void Start()
	{
		//���� ī�޶� cut���� �����̵��� ����������, Cinemachine�� �⺻������
		//Blend ������ ����Ͽ� ī�޶� �� ��ȯ�� �ε巴�� ó���Ѵ�.
		//���� �� AI�� �Լ��� ���� ī�޶� ������ ��, cut�� �ƴ� ease�� �ε巴�� ��ȯ�� �̷������ �ȴ�.
		//�׸��Ͽ�, ī�޶� ��ȯ�� �߻���, Cinemachine�� Blend ������ cut�� �����ϰ�, �ٽ� �ǵ����� ����� ����Ѵ�.

		//Cinemachine Brain ����
		brain = Camera.main.GetComponent<CinemachineBrain>();

		//Blend ���� ����
		originalBlend = brain.m_DefaultBlend;
	}

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
		//��ȯ�� cut���� ���� ��ȯ
		brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

		if (GameManager.Instance.Turn == 0)
			playerCamera.Priority = 0;
		else if (GameManager.Instance.Turn == 1)
			EnemyCamera.Priority = 0;

		HorseCamera.Priority = 10;
		MainCanvas.enabled = false;
		HorseCanvers.enabled = true;

		//ī�޶� ��ȯ ��, �ٽ� �⺻ �������� ����
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

		//ī�޶� ��ȯ ��, �ٽ� �⺻ �������� ����
		StartCoroutine( RestoreBlend());
	}

	private IEnumerator RestoreBlend()
	{
		yield return null;
		brain.m_DefaultBlend = originalBlend;
	}
}
