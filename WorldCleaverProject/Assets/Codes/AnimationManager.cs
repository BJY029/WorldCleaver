using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public Animator PlayerAnim;
	public Animator EnemyAnim;
	public Animator TreeAnim;
	public Animator PlayerSquid;
	public Animator EnemySquid;

	public GameObject ESquid;

	public float WaitHitTime = 1.1f;

	public float WaitDrinkTime = 1.35f;
	public float WaitFireTime = 1.7f;
	public float WaitThrowTime = 1f;
	public float WaitSquidTime = 2.2f;
	public float duration = 2.0f;

	public float WaitSapTime = 2.8f;

	public int isHitingTree;

	private void Awake()
	{
		isHitingTree = 0;

		//��¡�� ������Ʈ�� ���İ��� 0���� �ʱ�ȭ�ؼ�, �ش� �̹����� ������ �ʵ��� �������ش�.
		SpriteRenderer image1 = ESquid.GetComponent<SpriteRenderer>();
		Color color1 = image1.color;

		image1.color = new Color(color1.r, color1.g, color1.b, 0f);
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
		if(Turn == 0)
		{
			BGMManager.Instance.PlayerDead();
		}
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

	public void Squid()
	{
		int Turn = GameManager.Instance.Turn;
		SpriteRenderer SImage = null;
		Color color = new Color(1f, 1f, 1f);
		if (Turn == 1)
		{
			GameManager.Instance.EffectAudioManager.playPop();
			//�ش� ��¡�� �̹����� ���̰� ������ְ�
			SImage = ESquid.GetComponent<SpriteRenderer>();
			color = SImage.color;
			SImage.color = new Color(color.r, color.g, color.b, 1f);
			//�ִϸ��̼� ���
			EnemySquid.SetBool("Squid", true);
		}
		//�ڷ�ƾ�� �����Ѵ�.
		StartCoroutine(Squid(Turn, SImage, color));
	}

	IEnumerator Squid(int Turn, SpriteRenderer SImage, Color color)
	{
		yield return new WaitForSeconds(0.3f);
		GameManager.Instance.EffectAudioManager.playSqueeze();
		//���� �ð� ��� ��, �ش� �̹����� ������ �����ϰ� �����Ų��.
		yield return new WaitForSeconds(WaitSquidTime - 0.3f);
		float elapsedTime = 0.0f;
		while(elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			float newAlpa = Mathf.Lerp(1f, 0f, elapsedTime / duration);
			SImage.color = new Color(color.r, color.g, color.b, newAlpa);

			yield return null;
		}
		//���� ������ ������ �����ϰ� �ǵ��� �������ְ�
		SImage.color = new Color(color.r, color.g, color.b, 0f);

		//�ִϸ��̼� ���� �ʱ�ȭ �����ش�.
		if(Turn == 1)
		{
			EnemySquid.SetBool("Squid", false);
		}
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
		yield return new WaitForSeconds(WaitSapTime/2);
		GameManager.Instance.EffectAudioManager.PlayHeal();
		yield return new WaitForSeconds(WaitSapTime / 2);
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
		GameManager.Instance.EffectAudioManager.PlaySmoke();
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
		yield return new WaitForSeconds(WaitFireTime - 0.4f);
		GameManager.Instance.EffectAudioManager.PlayFlareGun();
		yield return new WaitForSeconds(0.4f);
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
		yield return new WaitForSeconds(WaitDrinkTime/2);
		GameManager.Instance.EffectAudioManager.PlayCrunch();
		yield return new WaitForSeconds(WaitDrinkTime / 2);
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
		yield return new WaitForSeconds(WaitDrinkTime / 2);
		GameManager.Instance.EffectAudioManager.PlayEat();
		yield return new WaitForSeconds(WaitDrinkTime / 2);
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
		yield return new WaitForSeconds(WaitDrinkTime / 2);
		GameManager.Instance.EffectAudioManager.PlayDrink();
		yield return new WaitForSeconds(WaitDrinkTime / 2);
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
		yield return new WaitForSeconds(WaitDrinkTime / 2);
		GameManager.Instance.EffectAudioManager.PlayDrink();
		yield return new WaitForSeconds(WaitDrinkTime / 2);
		if (Turn == 0)
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
		GameManager.Instance.EffectAudioManager.PlayHit();
		//yield return new WaitForSeconds(0.5f);
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
