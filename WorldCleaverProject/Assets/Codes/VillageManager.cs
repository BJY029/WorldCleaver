using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillageManager : MonoBehaviour
{
    //ü���� ����� �����ؼ� �ѹ��� ���� �����Ѵ�.
    //����� ����
	public readonly float H = 1000.0f;
	private float VillagerHealth = 1000.0f;

    public Animator Villager_Male;
    public Animator Villager_Girl;
	public Animator Villager_Man;
	public Animator Villager_OldMan;
	public Animator Villager_OldWoman;
	public Animator Villager_Woman;

    public SpriteRenderer VillageSky;
    public SpriteRenderer VillageCloud;

    public Sprite normalSky;
    public Sprite normalCloud;
    public Sprite DangerSky1;
    public Sprite DangerSky2;
    public Sprite DangerCloud1;
    public Sprite DangerCloud2;

    //���� ü���� get,set
	public float VilageHelath
    {
        get {  return VillagerHealth; }
        set{
            if(VillagerHealth + value > H)
            {
                VillagerHealth = H;
            }
            else if(VillagerHealth + value < 0.0f)
            {
                VillagerHealth = 0.0f;
            }
            else
            {
                VillagerHealth += value;
            }
            //���� ü���� �ʱ�ȭ ��, �� �ֹε��� ü���� �ʱ�ȭ�Ѵ�.
            setVillagerHealth();
        }
    }

	private void Awake()
	{
        //ü�� �ʱ�ȭ
        VillagerHealth = H;
        Villager_Male.SetFloat("VillageHealth",VillagerHealth);
		Villager_Girl.SetFloat("VillageHealth", VillagerHealth);
		Villager_Man.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldMan.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldWoman.SetFloat("VillageHealth", VillagerHealth);
		Villager_Woman.SetFloat("VillageHealth", VillagerHealth);
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VillagerHealth <= 0.0f)
        {
            VillagerHealth = 0.0f;
            //Debug.Log("You Lose!!");
            GameManager.Instance.Turn = 44;
        }
    }

    //���� ü�¿� �°� �ֹε��� ü�µ� �ʱ�ȭ�ȴ�.
    public void setVillagerHealth()
    {
		Villager_Male.SetFloat("VillageHealth", VillagerHealth);
		Villager_Girl.SetFloat("VillageHealth", VillagerHealth);
		Villager_Man.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldMan.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldWoman.SetFloat("VillageHealth", VillagerHealth);
		Villager_Woman.SetFloat("VillageHealth", VillagerHealth);
	}

    public void ChangeBackGround()
    {
        if(VillagerHealth >= 500.0f)
        {
            VillageCloud.sprite = normalCloud;
            VillageSky.sprite = normalSky;
        }
        else if(VillagerHealth >= 300.0f)
        {
            VillageCloud.sprite = DangerCloud1;
            VillageSky.sprite = DangerSky1;
        }
        else
        {
			VillageCloud.sprite = DangerCloud2;
			VillageSky.sprite = DangerSky2;
		}
    }
}
