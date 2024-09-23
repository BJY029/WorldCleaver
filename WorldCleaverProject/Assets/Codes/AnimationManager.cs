using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator PlayerAnim;
	public float WaitHitTime = 1.35f;

	public void Hit()
	{
		PlayerAnim.SetBool("isHit", true);
		StartCoroutine(HitTree());
	
	}

	IEnumerator HitTree()
	{
		yield return new WaitForSeconds(WaitHitTime);
		PlayerAnim.SetBool("isHit", false);
	}
}
