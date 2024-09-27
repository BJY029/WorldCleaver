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

		//�÷��̾� ���̸�
		if (Turn == 0)
		{
			//���� �÷��̾� ��� �޾ƿ���
			float curPower = GameManager.Instance.PlayerController.Mana;
			//�÷��̾� ��� �����̴� ������Ʈ
			playerSlider.value = curPower / InitPlayerPower;
		}
		else if (Turn == 1) //�� ���̸�
		{
			float curPower = GameManager.Instance.EnemeyController.Mana;
			EnemySlider.value = curPower / InitEnemyPower;
		}

		//���� �÷��װ� 1�̸� ���� ������ �糢�� �ٷ� �����Ѵ�.
		if (flag == 1) return;
		//���� Hit ����� �������̸� ��ư�� ��Ȱ��ȭ ��Ų��.
		if (GameManager.Instance.AnimationManager.isHitingTree == 1)
		{
			HitButton.interactable = false;
		}
		else
		{
			HitButton.interactable= true;
		}
	}
}
