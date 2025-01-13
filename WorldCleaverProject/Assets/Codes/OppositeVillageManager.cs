using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeVillageManager : MonoBehaviour
{
	//체력을 상수로 설정해서 한번에 값을 설정한다.
	//디버그 용이
	public readonly float H = 1000.0f;
	private float OppositeVillagerHealth = 1000.0f;

	public Animator Villager_Male;
	public Animator Villager_Girl;
	public Animator Villager_Man;
	public Animator Villager_OldMan;
	public Animator Villager_OldWoman;
	public Animator Villager_Woman;


	public float OppositeVillageHealth
	{
		get { return OppositeVillagerHealth; }
		set
		{
			if (OppositeVillageHealth + value > H)
			{
				OppositeVillagerHealth = H;
			}
			else if (OppositeVillagerHealth + value < 0.0f)
			{
				OppositeVillagerHealth = 0.0f;
			}
			else
			{
				OppositeVillagerHealth += value;
			}
			setVillagerHealth();
		}
	}


	void Start()
    {
		OppositeVillagerHealth = H;
		Villager_Male.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Girl.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Man.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_OldMan.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_OldWoman.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Woman.SetFloat("VillageHealth", OppositeVillagerHealth);
	}

    // Update is called once per frame
    void Update()
    {
		if (OppositeVillagerHealth <= 0.0f)
		{
			OppositeVillagerHealth = 0.0f;
			GameManager.Instance.Turn = 44;
		}
    }

	public void setVillagerHealth()
	{
		Villager_Male.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Girl.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Man.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_OldMan.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_OldWoman.SetFloat("VillageHealth", OppositeVillagerHealth);
		Villager_Woman.SetFloat("VillageHealth", OppositeVillagerHealth);
	}
}
