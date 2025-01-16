using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeVillageManager : MonoBehaviour
{
	//ü���� ����� �����ؼ� �ѹ��� ���� �����Ѵ�.
	//����� ����
	public readonly float H = 1000.0f;
	private float OppositeVillagerHealth = 1000.0f;

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

	public void ChangeBackGround()
	{
		if (OppositeVillagerHealth >= 500.0f)
		{
			VillageCloud.sprite = normalCloud;
			VillageSky.sprite = normalSky;
		}
		else if (OppositeVillagerHealth >= 300.0f)
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
