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
		if (Turn == 0)//플레이어 턴이면, 플레이어의 애니메이션을 실행 한다.
		{
			PlayerAnim.SetBool("isHit", true);
			StartCoroutine(HitTree(Turn));
		}
		else if(Turn == 1) //적 턴이면, 적의 애니메이션을 실행한다.
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
		if (Turn == 0) //플레이어 턴이면, 플레이어의 애니메이션을 초기화한다.
		{
			PlayerAnim.SetBool("isHit", false);
			PlayerAnim.SetBool("Oil", false);
		}
		else if(Turn == 1) //적 턴이면, 적의 애니메이션을 초기화 한다.
		{
			EnemyAnim.SetBool("isHit", false);
			EnemyAnim.SetBool("Oil", false);
			//DisplayPlayerItems.Instance.beableButtons();
			//만약 플레이어의 아이템이 아직 다 꽉 차지 않은 경우
			//if (DisplayPlayerItems.Instance.isFull() == false)
			//{
			//	ItemManager.Instance.SetRandomItemsOnButtons(); //적 턴에서, Hit가 발생되면, 아이템 선택 창을 표시한다.
			//}
			//else //아이템이 꽉 찬 경우
			//{
			//	DisplayWarningMessage.Instance.itemIsFull(); //경고 메시지 출력
			//}
		}
		//else
		//{
		//	PlayerAnim.SetBool("isHit", false);
		//	Dead();
		//}
		//애니메이션이 모두 실행된 후, 카메라 교체가 이루어진다.
		//if (GameManager.Instance.Turn != 44)
		//{
		//	CameraManager.Instance.changeCamera();
		//}
		isHitingTree = 0;
	}
}
