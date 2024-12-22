using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillageManager : SingleTon<VillageManager>
{
    //체력을 상수로 설정해서 한번에 값을 설정한다.
    //디버그 용이
	public readonly float H = 1000.0f;
	private float VillagerHealth = 1000.0f;

    public Animator Villager_Male;
    public Animator Villager_Girl;
	public Animator Villager_Man;
	public Animator Villager_OldMan;
	public Animator Villager_OldWoman;
	public Animator Villager_Woman;

    //마을 체력의 get,set
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
            //마을 체력을 초기화 후, 각 주민들의 체력을 초기화한다.
            setVillagerHealth();
        }
    }

	private void Awake()
	{
        //체력 초기화
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

    //마을 체력에 맞게 주민들의 체력도 초기화된다.
    public void setVillagerHealth()
    {
		Villager_Male.SetFloat("VillageHealth", VillagerHealth);
		Villager_Girl.SetFloat("VillageHealth", VillagerHealth);
		Villager_Man.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldMan.SetFloat("VillageHealth", VillagerHealth);
		Villager_OldWoman.SetFloat("VillageHealth", VillagerHealth);
		Villager_Woman.SetFloat("VillageHealth", VillagerHealth);
	}
}
