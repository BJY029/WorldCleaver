using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator PlayerAnim;
	public float WaitHitTime = 1.35f;

	public int isHitingTree;

	private void Awake()
	{
		isHitingTree = 0;
	}

	public void Hit()
	{
		PlayerAnim.SetBool("isHit", true);
		StartCoroutine(HitTree());
	}

	IEnumerator HitTree()
	{
		isHitingTree = 1;
		yield return new WaitForSeconds(WaitHitTime);
		PlayerAnim.SetBool("isHit", false);
		isHitingTree = 0;
	}
}
