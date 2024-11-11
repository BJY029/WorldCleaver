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

    public void setTreeHealth(float health)
    {
		//만약 입력받은 마나 값과의 합이 100을 넘으면, 그냥 100으로 초기화한다.
		if (Health + health > 1000) Health = 1000;
		//만약 0 혹은 음수가 되면, 그냥 1로 초기화한다.
		else if (Health + health <= 0)
		{
			Health = 1;
		}
		//그 외는 그냥 합산한 값을 저장한다.
		else Health += health;
	}

    private int HitDamage;

	private void Awake()
	{
        Health = 1000.0f;
		Debug.Log("TreeHealth :" + Health);
	}

	public void DamageHit()
    {
        if(ItemManager.Instance.Flag == 7)
        {
            HitDamage = RanHitDamageForOil(); //데미지 감소
        }
        else if(ItemManager.Instance.Flag == 12)
        {
            HitDamage = RanHitDamageForVelvet();//데미지 증가
        }
        else
        {
			HitDamage = RandHitDamage();
		}
        
        if (HitDamage < Health)
        {
            Health -= HitDamage;
            Debug.Log("TreeHealth :" + Health);
            Debug.Log("My Hit Damage : " + HitDamage);
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
        int randomValue = Random.Range(40, 60);
        return randomValue;
    }

    private int RanHitDamageForOil()
    {
		int randomValue = Random.Range(5, 15);
        return randomValue;
    }

	private int RanHitDamageForVelvet()
	{
		int randomValue = Random.Range(150, 250);
		return randomValue;
	}
}
