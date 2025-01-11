using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    private float Health = 1000.0f;

    //�Թ��� �������и� ���� �Լ� ���� �÷���
    public float MyDamageCoef;
    public float OppositeDamageCoef;

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
			Health = 0;
            GameManager.Instance.Turn = 44;
		}
		//�� �ܴ� �׳� �ջ��� ���� �����Ѵ�.
		else Health += health;
	}

    public int HitDamage;
    public int HitDamageFromOp;

	private void Awake()
	{
        Health = 1000.0f;

        MyDamageCoef = 1.0f;
        OppositeDamageCoef = 1.0f;

		//Debug.Log("TreeHealth :" + Health);
	}

	public void DamageHitPlayer()
    {
        //Debug.Log("Damage Player");
        if(ItemManager.Instance.Flag == 7)
        {
            HitDamage = RanHitDamageForOil(); //������ ����
        }
        else if(ItemManager.Instance.Flag == 12)
        {
            HitDamage = RanHitDamageForVelvet();//������ ����
        }
        else if(ItemManager.Instance.Flag == 1)//�罿 ��ġ���� ���
        {
            HitDamage = RanHitDamageForDeer(); 
            GameManager.Instance.MyDamageTotalCnt = 0; //���� ������ ������ 0���� �ʱ�ȭ �����ش�.
        }
        else
        {
			HitDamage = RandHitDamage();
		}

        //������ ���ο� ���� ������ ��� ����
        HitDamage = (int)(HitDamage * MyDamageCoef);
        //Debug.Log("������ ��� : " + MyDamageCoef);

        if (HitDamage < Health)
        {
            Health -= HitDamage;
            //Debug.Log("TreeHealth :" + Health);
            //Debug.Log("My Hit Damage : " + HitDamage);
        }
        else
        {
            //���� Tree�� ü���� �� ������ ���
			//Debug.Log(GameManager.Instance.Turn + " Lose!");
            //�罿 ��ġ��� ���� ü���� �� ������ ���
            if(ItemManager.Instance.Flag == 1)
            {
                //���� �й�
                GameManager.Instance.DeerLastHit = 0;
            }
            Health = 0.0f;
            //Turn 44�� ��ȯ�Ѵ�.
			GameManager.Instance.Turn = 44;
            Debug.Log("ImIn, Turn == " + GameManager.Instance.Turn);
		}

		//�罿�����۵� �ش� �Լ��� ����ϱ� ������, �߸��� �� ����� ��������, �罿 �������� �ش� �Լ��� ����ϴ� ���
		//�Ʒ� ������ �������� �ʴ´�.
		if (ItemManager.Instance.Flag != 1)
		{
			//���� ShilFlag�� 0�� �ƴϸ�, 1�� ����.
			if (ItemManager.Instance.ShildFlag != 0)
			{
				ItemManager.Instance.ShildFlag--;
			}
			//���� ShilfFlag�� 0�̸�, Shild �ִϸ��̼��� ����.
			if (ItemManager.Instance.ShildFlag == 0)
			{
				GameManager.Instance.AnimationManager.offShild();
			}
		}

		ItemManager.Instance.Flag = -1;
	}

    public void DamageHitOppositePlayer()
    {
		//Debug.Log("Damage Player");
		if (ItemManager.Instance.Flag == 7)
		{
			HitDamageFromOp = RanHitDamageForOil(); //������ ����
		}
		else if (ItemManager.Instance.Flag == 12)
		{
			HitDamageFromOp = RanHitDamageForVelvet();//������ ����
		}
		else if (ItemManager.Instance.Flag == 1)//�罿 ��ġ���� ���
		{
			HitDamageFromOp = RanHitDamageForDeer();
			GameManager.Instance.OppositeDamageTotalCnt = 0; //���� ������ ������ 0���� �ʱ�ȭ �����ش�.
		}
		else
		{
            Debug.Log("imin");
			HitDamageFromOp = RandHitDamage();
		}


        //������ ���ο� ���� ������ ��� ����
        HitDamageFromOp = (int)(HitDamageFromOp * OppositeDamageCoef);
		//Debug.Log("�� ������ ��� : " + OppositeDamageCoef);

		if (HitDamageFromOp < Health)
        {
			Health -= HitDamageFromOp;
			//Debug.Log("TreeHealth :" + Health);
			Debug.Log("Opposite Hit Damage : " + HitDamageFromOp);
		}
        else
        {
			//���� Tree�� ü���� �� ������ ���
			//Debug.Log(GameManager.Instance.Turn + " Win!");
			//�罿 ��ġ��� ���� ü���� �� ������ ���
			if (ItemManager.Instance.Flag == 1)
			{
                //���� �й�
				GameManager.Instance.DeerLastHit = 1;
			}
			Health = 0.0f;
			//Turn 44�� ��ȯ�Ѵ�.
			GameManager.Instance.Turn = 44;
		}


		//�罿�����۵� �ش� �Լ��� ����ϱ� ������, �߸��� �� ����� ��������, �罿 �������� �ش� �Լ��� ����ϴ� ���
		//�Ʒ� ������ �������� �ʴ´�.
		if (ItemManager.Instance.Flag != 1)
		{
			//���� ShilFlag�� 0�� �ƴϸ�, 1�� ����.
			if (ItemManager.Instance.ShildFlag != 0)
				ItemManager.Instance.ShildFlag--;
			//���� ShilfFlag�� 0�̸�, Shild �ִϸ��̼��� ����.
			if (ItemManager.Instance.ShildFlag == 0)
			{
				GameManager.Instance.AnimationManager.offShild();
			}
		}

		ItemManager.Instance.Flag = -1;
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

	private int RanHitDamageForDeer()
	{
		int randomValue = Random.Range(300, 350);
		return randomValue;
	}
}
