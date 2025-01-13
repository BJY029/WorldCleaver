using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHorse : SingleTon<SelectHorse>
{
    public Sprite HorseImg;
    public string HorseName;

    public void OnItemSelected(int horseNum)
    {
        GameManager.Instance.HorseManager.MyHorseImg = HorseImg;
        GameManager.Instance.HorseManager.MyHorseName = HorseName;
		GameManager.Instance.HorseManager.MyHorseNum = horseNum;
		ClosePanel();

		GameManager.Instance.HorseManager.InsertInfoInDecisionPanel();
		//아이템이 선택된 후, 최종 말을 확인
		StartCoroutine(GameManager.Instance.HorseManager.DisplayDecisionHorse());
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
