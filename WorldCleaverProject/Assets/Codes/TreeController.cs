using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private float Health = 1000.0f;

    //������ ü���� ��ȯ���ִ� �Լ�
    public float treeHealth
    {
        get
        {
            return Health;
        }
    }

    public void setTreeHealth(float health)
    {
		//���� �Է¹��� ���� ������ ���� 100�� ������, �׳� 100���� �ʱ�ȭ�Ѵ�.
		if (Health + health > 1000) Health = 1000;
		//���� 0 Ȥ�� ������ �Ǹ�, �׳� 1�� �ʱ�ȭ�Ѵ�.
		else if (Health + health <= 0)
		{
			Health = 1;
		}
		//�� �ܴ� �׳� �ջ��� ���� �����Ѵ�.
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
            HitDamage = RanHitDamageForOil(); //������ ����
        }
        else if(ItemManager.Instance.Flag == 12)
        {
            HitDamage = RanHitDamageForVelvet();//������ ����
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
            //���� Tree�� ü���� �� ������ ���
			Debug.Log(GameManager.Instance.Turn + " Lose!");
            Health = 0.0f;
            //Turn 44�� ��ȯ�Ѵ�.
			GameManager.Instance.Turn = 44;
		}
    }

    private int RandHitDamage() //���� ������ ����
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
