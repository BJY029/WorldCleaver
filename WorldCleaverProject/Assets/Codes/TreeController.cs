using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class TreeController : MonoBehaviour
{
	public readonly float Treehealth = 2000.0f;
    private float Health = 2000.0f;

    //�Թ��� �������и� ���� �Լ� ���� �÷���
    public float MyDamageCoef;
    public float OppositeDamageCoef;

	public GameObject PlayerPopPrefab;
	public GameObject EnemyPopPrefab;

	public GameObject SpawnP;
	public GameObject SpawnE;

	private CinemachineImpulseSource impulseSource;


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
		//���� �Է¹��� ü�� ������ ���� 2000�� ������, �׳� 2000���� �ʱ�ȭ�Ѵ�.
		if (Health + health > 2000) Health = 2000;
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
        Health = 2000.0f;

        MyDamageCoef = 1.0f;
        OppositeDamageCoef = 1.0f;

		impulseSource = GetComponent<CinemachineImpulseSource>();
		//Debug.Log("TreeHealth :" + Health);
	}

	public void DamageHitPlayer()
    {
        //Debug.Log("Damage Player");
        if(GameManager.Instance.ItemManager.Flag == 7)
        {
            HitDamage = RanHitDamageForOil(); //������ ����
        }
        else if(GameManager.Instance.ItemManager.Flag == 12)
        {
            HitDamage = RanHitDamageForVelvet();//������ ����
        }
        else if(GameManager.Instance.ItemManager.Flag == 1)//�罿 ��ġ���� ���
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

		Color color = Color.white;
		if (HitDamage <= 50)
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.2f);
			color = Color.white;
		}
        else if(HitDamage <= 150)
        {
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.4f);
			color = Color.yellow;
        }
		else if(HitDamage <= 300)
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.6f);
			color = Color.red;
		}
		else
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 1f);
			color = Color.blue;
		}

        GameObject popUp = Instantiate(PlayerPopPrefab, SpawnP.transform.position, Quaternion.identity);
		popUp.GetComponentInChildren<TMP_Text>().color = color;
		popUp.GetComponentInChildren<TMP_Text>().text = HitDamage.ToString();


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
            if(GameManager.Instance.ItemManager.Flag == 1)
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
		if (GameManager.Instance.ItemManager.Flag != 1)
		{
			//���� ShilFlag�� 0�� �ƴϸ�, 1�� ����.
			if (GameManager.Instance.ItemManager.ShildFlag != 0)
			{
				GameManager.Instance.ItemManager.ShildFlag--;
			}
			//���� ShilfFlag�� 0�̸�, Shild �ִϸ��̼��� ����.
			if (GameManager.Instance.ItemManager.ShildFlag == 0)
			{
				GameManager.Instance.AnimationManager.offShild();
			}
		}

		GameManager.Instance.ItemManager.Flag = -1;
	}

    public void DamageHitOppositePlayer()
    {
		//Debug.Log("Damage Player");
		if (GameManager.Instance.ItemManager.Flag == 7)
		{
			HitDamageFromOp = RanHitDamageForOil(); //������ ����
		}
		else if (GameManager.Instance.ItemManager.Flag == 12)
		{
			HitDamageFromOp = RanHitDamageForVelvet();//������ ����
		}
		else if (GameManager.Instance.ItemManager.Flag == 1)//�罿 ��ġ���� ���
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

		Color color = Color.white;
		if (HitDamageFromOp <= 50)
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.2f);
			color = Color.white;
		}
		else if (HitDamageFromOp <= 150)
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.4f);
			color = Color.yellow;
		}
		else if (HitDamageFromOp <= 300)
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 0.6f);
			color = Color.red;
		}
		else
		{
			GameManager.Instance.CameraManager.CameraShake(impulseSource, 1f);
			color = Color.blue;
		}

		GameObject popUp = Instantiate(EnemyPopPrefab, SpawnE.transform.position, Quaternion.identity);
		popUp.GetComponentInChildren<TMP_Text>().color = color;
		popUp.GetComponentInChildren<TMP_Text>().text = HitDamageFromOp.ToString();

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
			if (GameManager.Instance.ItemManager.Flag == 1)
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
		if (GameManager.Instance.ItemManager.Flag != 1)
		{
			//���� ShilFlag�� 0�� �ƴϸ�, 1�� ����.
			if (GameManager.Instance.ItemManager.ShildFlag != 0)
				GameManager.Instance.ItemManager.ShildFlag--;
			//���� ShilfFlag�� 0�̸�, Shild �ִϸ��̼��� ����.
			if (GameManager.Instance.ItemManager.ShildFlag == 0)
			{
				GameManager.Instance.AnimationManager.offShild();
			}
		}

		GameManager.Instance.ItemManager.Flag = -1;
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
