using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�������� UI�� �����ϴ� ��ũ��Ʈ
//�̱����̱⿡ GameManager�� �ٷ� ���� �����ϴ�.
public class UIManager : SingleTon<UIManager>
{
	public Slider treeSlider; //���� ü���� ��Ÿ���ִ� �����̴� UI
	public Slider playerSlider; //�÷��̾� ����� ��Ÿ���ִ� �����̴� UI
	public Slider EnemySlider; //�� ����� ��Ÿ���ִ� �����̴� UI
	public Button HitButton; //HIT ��ư UI

	public Canvas VillageCanvas;
	public Button ToVillageButton;
	public GameObject ItemListPanel;

	private float InitHealth; //���� ü���� �ʱ� ���� �����ϱ� ���� ���� 
	private float InitPlayerPower; //�÷��̾� ü���� �ʱ갪�� �����ϱ� ���� ����
	private float InitEnemyPower; //�÷��̾� ü���� �ʱ갪�� �����ϱ� ���� ����

	//������ ������ ���� �÷��� ����
	private int flag = 0;

	private void Start()
	{
		//�ʱ�ȭ
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPlayerPower = GameManager.Instance.PlayerController.Mana;
		InitEnemyPower = GameManager.Instance.EnemeyController.Mana;

		VillageCanvas.enabled = false;

		playerSlider.value = InitPlayerPower;
		EnemySlider.value = InitEnemyPower;
		treeSlider.value = InitHealth;
	}

	private void Update()
	{
		//�߿�! ���� ������ ����� ���
		if (GameManager.Instance.Turn == 44)
		{
			//Hit ��ư�� ��Ȱ��ȭ �Ѵ�.
			HitButton.interactable = false;
			//���� ���� Canvas ��Ȱ��ȭ
			ToVillageButton.interactable = false;
			//�׸��� ���� �÷��װ� 1�̸� �̹� ��� �ٰ� �ʱ�ȭ�� �����̹Ƿ� �״�� return�Ѵ�.
			if (flag == 1) return;
			//���� �÷��װ� 0�̸�, ��� ���� ������ ���� �̷������ ���̴�. 
			//����, ��� ���� ������ ���� ��, ���� ó���� �ϱ� ���� flag ����
			//���� ��� ���� ������ ���� �Ŀ�, flag ���� 1�� ������Ʈ �Ǹ鼭 return�� �ȴ�.
			else flag = 1;
		}

		//���� ���� ü�� �޾ƿ���
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//���� ü�� �����̴� ������Ʈ
		treeSlider.value = curHealth / InitHealth;

		//�׳� Turn ���� �ҷ�����, �̹� Turn�� ��ü�� ���¿��� ������ ����ǹǷ� ���� �߻�
		//����, ���� Turn�� ������ myTurn ���� �ҷ��ͼ� �ش� ���� ���� ���� �÷��̾ �Ǵ��Ѵ�.
		int Turn = GameManager.Instance.myTurn;

		//�׳� ��ÿ� ������Ʈ �� �� �ֵ��� ����
		//���� �÷��̾� ��� �޾ƿ���
		float curPower_p = GameManager.Instance.PlayerController.Mana;
		//�÷��̾� ��� �����̴� ������Ʈ
		playerSlider.value = curPower_p / InitPlayerPower;

		float curPower_e = GameManager.Instance.EnemeyController.Mana;
		EnemySlider.value = curPower_e / InitEnemyPower;

		////�÷��̾� ���̸�
		//if (Turn == 0)
		//{
		//	//���� �÷��̾� ��� �޾ƿ���
		//	float curPower = GameManager.Instance.PlayerController.Mana;
		//	//�÷��̾� ��� �����̴� ������Ʈ
		//	playerSlider.value = curPower / InitPlayerPower;
		//}
		//else if (Turn == 1) //�� ���̸�
		//{
		//	float curPower = GameManager.Instance.EnemeyController.Mana;
		//	EnemySlider.value = curPower / InitEnemyPower;
		//}

		//���� �� ���̸� ��ư�� ��Ȱ��ȭ ��Ų��.
		if(GameManager.Instance.Turn == 1) ToVillageButton.interactable = false;
		else if(GameManager.Instance.Turn == 0) //���� �� ���̸�
		{
			//���� ���� ������ Full ����� �߰� �ִ� ���¸�
			if(DisplayWarningMessage.Instance.WarningFlag == 1)
			{
				//��Ȱ��ȭ
				ToVillageButton.interactable = false;
			}
			else
			{
				ToVillageButton.interactable = true;
			}

			//���� ���� ������ ���� â�� �� �ִ� ���
			if (ItemListPanel.transform.localScale == Vector3.one)
			{
				//��ư ��Ȱ��ȭ
				ToVillageButton.interactable = false;
			}
			else
			{
				//�ƴϸ� �ٽ� Ȱ��ȭ
				ToVillageButton.interactable = true;
			}
		}

		//���� �÷��װ� 1�̸� ���� ������ �糢�� �ٷ� �����Ѵ�.
		if (flag == 1) return;
		//���� Hit ����� �������̸� ��ư�� ��Ȱ��ȭ ��Ų��.
		if (GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
			//���� ��ư ���� ��Ȱ��ȭ��Ų��.
			ToVillageButton.interactable = false;
		}
		else
		{
			HitButton.interactable= true;
		}
	}
}
