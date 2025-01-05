using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHorse : SingleTon<SelectHorse>
{
    public Sprite HorseImg;
    public string HorseName;

    public void OnItemSelected(int horseNum)
    {
        HorseManager.Instance.MyHorseImg = HorseImg;
        HorseManager.Instance.MyHorseName = HorseName;
		HorseManager.Instance.MyHorseNum = horseNum;
		ClosePanel();

		HorseManager.Instance.InsertInfoInDecisionPanel();
		//아이템이 선택된 후, 최종 말을 확인
		StartCoroutine(HorseManager.Instance.DisplayDecisionHorse());
    }

	public void DisplayPanel()
	{
		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.parent.localScale = Vector3.one;
		}
	}

	public void ClosePanel()
	{
		if (transform.parent != null && transform.parent.parent != null)
		{
			transform.parent.parent.parent.localScale = Vector3.zero;
		}
	}
}
