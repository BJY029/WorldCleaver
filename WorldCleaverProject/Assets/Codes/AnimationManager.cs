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

		//오징어 오브젝트의 알파값을 0으로 초기화해서, 해당 이미지가 보이지 않도록 설정해준다.
		SpriteRenderer image1 = ESquid.GetComponent<SpriteRenderer>();
		Color color1 = image1.color;

		image1.color = new Color(color1.r, color1.g, color1.b, 0f);
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
			//해당 오징어 이미지를 보이게 만들어주고
			SImage = ESquid.GetComponent<SpriteRenderer>();
			color = SImage.color;
			SImage.color = new Color(color.r, color.g, color.b, 1f);
			//애니메이션 재생
			EnemySquid.SetBool("Squid", true);
		}
		//코루틴을 시작한다.
		StartCoroutine(Squid(Turn, SImage, color));
	}

	IEnumerator Squid(int Turn, SpriteRenderer SImage, Color color)
	{
		yield return new WaitForSeconds(0.3f);
		GameManager.Instance.EffectAudioManager.playSqueeze();
		//일정 시간 대기 후, 해당 이미지를 서서히 투명하게 변경시킨다.
		yield return new WaitForSeconds(WaitSquidTime - 0.3f);
		float elapsedTime = 0.0f;
		while(elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			float newAlpa = Mathf.Lerp(1f, 0f, elapsedTime / duration);
			SImage.color = new Color(color.r, color.g, color.b, newAlpa);

			yield return null;
		}
		//최종 값으로 완전히 투명하게 되도록 설정해주고
		SImage.color = new Color(color.r, color.g, color.b, 0f);

		//애니메이션 값을 초기화 시켜준다.
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
