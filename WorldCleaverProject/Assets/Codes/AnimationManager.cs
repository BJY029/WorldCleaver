using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator PlayerAnim;
	public Animator EnemyAnim;
	public Animator TreeAnim;
	public float WaitHitTime = 1.35f;

	public float WaitDrinkTime = 1.35f;
	public float WaitFireTime = 1.7f;
	public float WaitThrowTime = 1f;

	public float WaitSapTime = 2.8f;

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
		//else
		//{	
		//	if (GameManager.Instance.WhoLose == 0)
		//	{
		//		PlayerAnim.SetBool("isHit", true);
		//		StartCoroutine(HitTree(Turn));
		//	}
		//	else if(GameManager.Instance.WhoLose == 1)
		//	{
		//		EnemyAnim.SetBool("isHit", true);
		//		StartCoroutine(HitTree(Turn));
		//	}
		//}
	}

	public void Dead()
	{
		int Turn = GameManager.Instance.WhoLose;
		StartCoroutine(Dead(Turn));
	}

	IEnumerator Dead(int Turn)
	{
		if(GameManager.Instance.ReasonFlag == 0)
			yield return new WaitForEndOfFrame();
		else
			yield return new WaitForSeconds(WaitHitTime);

		if (Turn == 0)
		{
			PlayerAnim.SetBool("Dead", true);
			TreeAnim.SetBool("PlayerLose", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("Dead", true);
			TreeAnim.SetBool("EnemyLose", true);
		}
	}

	public void Shild()
	{
		TreeAnim.SetBool("Shild", true);
	}

	public void offShild()
	{
		TreeAnim.SetBool("Shild", false);
	}

	public void Sap()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			TreeAnim.SetBool("PlayerSap", true);
		}
		else if (Turn == 1)
		{
			TreeAnim.SetBool("EnemySap", true);
		}
		StartCoroutine(Sap(Turn));
	}

	IEnumerator Sap(int Turn)
	{
		yield return new WaitForSeconds(WaitSapTime);
		if (Turn == 0)
		{
			TreeAnim.SetBool("PlayerSap", false);
		}
		else if (Turn == 1)
		{
			TreeAnim.SetBool("EnemySap", false);
		}
	}

	public void DrinkRed()
	{
		int Turn = GameManager.Instance.Turn;
		if(Turn == 0)
		{
			PlayerAnim.SetBool("DrinkRed", true);
		}
		else if(Turn == 1)
		{
			EnemyAnim.SetBool("DrinkRed", true);
		}
		StartCoroutine(DrinkRed(Turn));
	}

	public void DrinkHoney()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			PlayerAnim.SetBool("DrinkHoney", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("DrinkHoney", true);
		}
		StartCoroutine(DrinkHoney(Turn));
	}

	public void EatDeer()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			PlayerAnim.SetBool("EatDeer", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("EatDeer", true);
		}
		StartCoroutine(EatDeer(Turn));
	}

	public void EatGin()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			PlayerAnim.SetBool("EatGin", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("EatGin", true);
		}
		StartCoroutine(EatGin(Turn));
	}

	public void FireFlare()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			PlayerAnim.SetBool("FireFlare", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("FireFlare", true);
		}
		StartCoroutine(FireFlare(Turn));
	}

	public void ThrowSmoke()
	{
		int Turn = GameManager.Instance.Turn;
		if (Turn == 0)
		{
			PlayerAnim.SetBool("ThrowSmoke", true);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("ThrowSmoke", true);
		}
		StartCoroutine(ThrowSmoke(Turn));
	}

	IEnumerator ThrowSmoke(int Turn)
	{
		yield return new WaitForSeconds(WaitThrowTime);
		if (Turn == 0)
		{
			PlayerAnim.SetBool("ThrowSmoke", false);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("ThrowSmoke", false);
		}
	}

	IEnumerator FireFlare(int Turn)
	{
		yield return new WaitForSeconds(WaitFireTime);
		if (Turn == 0)
		{
			PlayerAnim.SetBool("FireFlare", false);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("FireFlare", false);
		}
	}

	IEnumerator EatDeer(int Turn)
	{
		yield return new WaitForSeconds(WaitDrinkTime);
		if (Turn == 0)
		{
			PlayerAnim.SetBool("EatDeer", false);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("EatDeer", false);
		}
	}

	IEnumerator EatGin(int Turn)
	{
		yield return new WaitForSeconds(WaitDrinkTime);
		if (Turn == 0)
		{
			PlayerAnim.SetBool("EatGin", false);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("EatGin", false);
		}
	}

	IEnumerator DrinkHoney(int Turn)
	{
		yield return new WaitForSeconds(WaitDrinkTime);
		if (Turn == 0)
		{
			PlayerAnim.SetBool("DrinkHoney", false);
		}
		else if (Turn == 1)
		{
			EnemyAnim.SetBool("DrinkHoney", false);
		}
	}

	IEnumerator DrinkRed(int Turn)
	{
		yield return new WaitForSeconds(WaitDrinkTime);
		if(Turn == 0)
		{
			PlayerAnim.SetBool("DrinkRed", false);
		}
		else if(Turn == 1)
		{
			EnemyAnim.SetBool("DrinkRed", false);
		}
	}

	IEnumerator HitTree(int Turn)
	{
		Debug.Log(GameManager.Instance.Turn);
		isHitingTree = 1;
		yield return new WaitForSeconds(WaitHitTime);
		if (Turn == 0) //�÷��̾� ���̸�, �÷��̾��� �ִϸ��̼��� �ʱ�ȭ�Ѵ�.
		{
			PlayerAnim.SetBool("isHit", false);
			PlayerAnim.SetBool("Oil", false);
		}
		else if(Turn == 1) //�� ���̸�, ���� �ִϸ��̼��� �ʱ�ȭ �Ѵ�.
		{
			EnemyAnim.SetBool("isHit", false);
			EnemyAnim.SetBool("Oil", false);
			//DisplayPlayerItems.Instance.beableButtons();
			//���� �÷��̾��� �������� ���� �� �� ���� ���� ���
			//if (DisplayPlayerItems.Instance.isFull() == false)
			//{
			//	ItemManager.Instance.SetRandomItemsOnButtons(); //�� �Ͽ���, Hit�� �߻��Ǹ�, ������ ���� â�� ǥ���Ѵ�.
			//}
			//else //�������� �� �� ���
			//{
			//	DisplayWarningMessage.Instance.itemIsFull(); //��� �޽��� ���
			//}
		}
		//else
		//{
		//	PlayerAnim.SetBool("isHit", false);
		//	Dead();
		//}
		//�ִϸ��̼��� ��� ����� ��, ī�޶� ��ü�� �̷������.
		//if (GameManager.Instance.Turn != 44)
		//{
		//	CameraManager.Instance.changeCamera();
		//}
		isHitingTree = 0;
	}
}
