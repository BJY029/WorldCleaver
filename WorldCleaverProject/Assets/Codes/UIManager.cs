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
	public Button HitButton; //HIT ��ư UI

	private float InitHealth; //���� ü���� �ʱ� ���� �����ϱ� ���� ���� 
	private float InitPower; //�÷��̾� ü���� �ʱ갪�� �����ϱ� ���� ����

	private void Start()
	{
		//�ʱ�ȭ
		InitHealth = GameManager.Instance.TreeController.treeHealth;
		InitPower = GameManager.Instance.PlayerController.Mana;
	}

	private void LateUpdate()
	{
		//���� ���� ü�� �޾ƿ���
		float curHealth = GameManager.Instance.TreeController.treeHealth;
		//���� ü�� �����̴� ������Ʈ
		treeSlider.value = curHealth / InitHealth;

		//���� �÷��̾� ��� �޾ƿ���
		float curPower = GameManager.Instance.PlayerController.Mana;
		//�÷��̾� ��� �����̴� ������Ʈ
		playerSlider.value = curPower / InitPower;

		//���� Hit ����� �������̸� ��ư�� ��Ȱ��ȭ ��Ų��.
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
