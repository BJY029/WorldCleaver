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
	}

	IEnumerator HitTree(int Turn)
	{
		isHitingTree = 1;
		yield return new WaitForSeconds(WaitHitTime);
		if (Turn == 0) //플레이어 턴이면, 플레이어의 애니메이션을 초기화한다.
		{
			PlayerAnim.SetBool("isHit", false);
		}
		else if(Turn == 1) //적 턴이면, 적의 애니메이션을 초기화 한다.
		{
			EnemyAnim.SetBool("isHit", false);
		}
		//애니메이션이 모두 실행된 후, 카메라 교체가 이루어진다.
		CameraManager.Instance.changeCamera();
		isHitingTree = 0;
	}
}
