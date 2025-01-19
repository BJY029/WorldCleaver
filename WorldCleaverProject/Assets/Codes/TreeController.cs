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

    //먹물과 나무방패를 위한 게수 조정 플래그
    public float MyDamageCoef;
    public float OppositeDamageCoef;

	public GameObject PlayerPopPrefab;
	public GameObject EnemyPopPrefab;

	public GameObject SpawnP;
	public GameObject SpawnE;

	private CinemachineImpulseSource impulseSource;


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
		//만약 입력받은 체력 값과의 합이 2000을 넘으면, 그냥 2000으로 초기화한다.
		if (Health + health > 2000) Health = 2000;
		//만약 0 혹은 음수가 되면, 그냥 1로 초기화한다.
		else if (Health + health <= 0)
		{
			Health = 0;
            GameManager.Instance.Turn = 44;
		}
		//그 외는 그냥 합산한 값을 저장한다.
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
            HitDamage = RanHitDamageForOil(); //데미지 감소
        }
        else if(GameManager.Instance.ItemManager.Flag == 12)
        {
            HitDamage = RanHitDamageForVelvet();//데미지 증가
        }
        else if(GameManager.Instance.ItemManager.Flag == 1)//사슴 박치기인 경우
        {
            HitDamage = RanHitDamageForDeer(); 
            GameManager.Instance.MyDamageTotalCnt = 0; //마을 데미지 스택을 0으로 초기화 시켜준다.
        }
        else
        {
			HitDamage = RandHitDamage();
		}

        //아이템 여부에 따른 데미지 계수 조정
        HitDamage = (int)(HitDamage * MyDamageCoef);
		//Debug.Log("데미지 계수 : " + MyDamageCoef);

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
            //만약 Tree의 체력이 다 떨어진 경우
			//Debug.Log(GameManager.Instance.Turn + " Lose!");
            //사슴 박치기로 인해 체력이 다 떨어진 경우
            if(GameManager.Instance.ItemManager.Flag == 1)
            {
                //나의 패배
                GameManager.Instance.DeerLastHit = 0;
            }
            Health = 0.0f;
            //Turn 44를 반환한다.
			GameManager.Instance.Turn = 44;
            Debug.Log("ImIn, Turn == " + GameManager.Instance.Turn);
		}

		//사슴아이템도 해당 함수를 사용하기 때문에, 잘못된 턴 계산을 막기위해, 사슴 아이템이 해당 함수를 사용하는 경우
		//아래 로직을 수행하지 않는다.
		if (GameManager.Instance.ItemManager.Flag != 1)
		{
			//만약 ShilFlag가 0이 아니면, 1씩 뺀다.
			if (GameManager.Instance.ItemManager.ShildFlag != 0)
			{
				GameManager.Instance.ItemManager.ShildFlag--;
			}
			//만약 ShilfFlag가 0이면, Shild 애니메이션을 끈다.
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
			HitDamageFromOp = RanHitDamageForOil(); //데미지 감소
		}
		else if (GameManager.Instance.ItemManager.Flag == 12)
		{
			HitDamageFromOp = RanHitDamageForVelvet();//데미지 증가
		}
		else if (GameManager.Instance.ItemManager.Flag == 1)//사슴 박치기인 경우
		{
			HitDamageFromOp = RanHitDamageForDeer();
			GameManager.Instance.OppositeDamageTotalCnt = 0; //마을 데미지 스택을 0으로 초기화 시켜준다.
		}
		else
		{
            Debug.Log("imin");
			HitDamageFromOp = RandHitDamage();
		}


        //아이템 여부에 따른 데미지 계수 조정
        HitDamageFromOp = (int)(HitDamageFromOp * OppositeDamageCoef);
		//Debug.Log("적 데미지 계수 : " + OppositeDamageCoef);

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
			//만약 Tree의 체력이 다 떨어진 경우
			//Debug.Log(GameManager.Instance.Turn + " Win!");
			//사슴 박치기로 인해 체력이 다 떨어진 경우
			if (GameManager.Instance.ItemManager.Flag == 1)
			{
                //적의 패배
				GameManager.Instance.DeerLastHit = 1;
			}
			Health = 0.0f;
			//Turn 44를 반환한다.
			GameManager.Instance.Turn = 44;
		}


		//사슴아이템도 해당 함수를 사용하기 때문에, 잘못된 턴 계산을 막기위해, 사슴 아이템이 해당 함수를 사용하는 경우
		//아래 로직을 수행하지 않는다.
		if (GameManager.Instance.ItemManager.Flag != 1)
		{
			//만약 ShilFlag가 0이 아니면, 1씩 뺀다.
			if (GameManager.Instance.ItemManager.ShildFlag != 0)
				GameManager.Instance.ItemManager.ShildFlag--;
			//만약 ShilfFlag가 0이면, Shild 애니메이션을 끈다.
			if (GameManager.Instance.ItemManager.ShildFlag == 0)
			{
				GameManager.Instance.AnimationManager.offShild();
			}
		}

		GameManager.Instance.ItemManager.Flag = -1;
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

	private int RanHitDamageForDeer()
	{
		int randomValue = Random.Range(300, 350);
		return randomValue;
	}
}
