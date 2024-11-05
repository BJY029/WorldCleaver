using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator PlayerAnim;
	public Animator EnemyAnim;
	public float WaitHitTime = 1.35f;

	public int isHitingTree;

	private void Awake()
	{
		isHitingTree = 0;
	}

	public void Hit()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)//�÷��̾� ���̸�, �÷��̾��� �ִϸ��̼��� ���� �Ѵ�.
		{
			PlayerAnim.SetBool("isHit", true);
			StartCoroutine(HitTree(Turn));
		}
		else if(Turn == 1) //�� ���̸�, ���� �ִϸ��̼��� �����Ѵ�.
		{
			EnemyAnim.SetBool("isHit", true);
			StartCoroutine(HitTree(Turn));
		}
	}

	IEnumerator HitTree(int Turn)
	{
		isHitingTree = 1;
		yield return new WaitForSeconds(WaitHitTime);
		if (Turn == 0) //�÷��̾� ���̸�, �÷��̾��� �ִϸ��̼��� �ʱ�ȭ�Ѵ�.
		{
			PlayerAnim.SetBool("isHit", false);
		}
		else if(Turn == 1) //�� ���̸�, ���� �ִϸ��̼��� �ʱ�ȭ �Ѵ�.
		{
			EnemyAnim.SetBool("isHit", false);

			//���� �÷��̾��� �������� ���� �� �� ���� ���� ���
			if (DisplayPlayerItems.Instance.isFull() == false)
			{
				ItemManager.Instance.SetRandomItemsOnButtons(); //�� �Ͽ���, Hit�� �߻��Ǹ�, ������ ���� â�� ǥ���Ѵ�.
			}
			else //�������� �� �� ���
			{
				DisplayWarningMessage.Instance.itemIsFull(); //��� �޽��� ���
			}
		}
		//�ִϸ��̼��� ��� ����� ��, ī�޶� ��ü�� �̷������.
		CameraManager.Instance.changeCamera();
		isHitingTree = 0;
	}
}
