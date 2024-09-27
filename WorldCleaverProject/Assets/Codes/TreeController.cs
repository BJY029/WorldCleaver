using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private float Health = 1000.0f;

    //나무의 체력을 반환해주는 함수
    public float treeHealth
    {
        get
        {
            return Health;
        }
    }

    private int HitDamage;

	private void Awake()
	{
        Health = 1000.0f;
		Debug.Log("TreeHealth :" + Health);
	}

	public void DamageHit()
    {
        HitDamage = RandHitDamage();
        if (HitDamage < Health)
        {
            Health -= HitDamage;
            Debug.Log("TreeHealth :" + Health);
        }
        else
        {
            //만약 Tree의 체력이 다 떨어진 경우
			Debug.Log(GameManager.Instance.Turn + " Lose!");
            Health = 0.0f;
            //Turn 44를 반환한다.
			GameManager.Instance.Turn = 44;
		}
    }

    private int RandHitDamage() //랜덤 데미지 생성
    {
        HitDamage = Random.Range(40, 60);
        return HitDamage;
    }
}
