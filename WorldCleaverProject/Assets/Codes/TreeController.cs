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
            //Lose!!
        }
    }

    private int RandHitDamage() //���� ������ ����
    {
        HitDamage = Random.Range(40, 60);
        return HitDamage;
    }
}
