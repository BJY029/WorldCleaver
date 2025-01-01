using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWarningMessage : SingleTon<DisplayWarningMessage>
{
    public int WarningFlag;
    public float waitingTime = 2.0f;
    void Start()
    {
		transform.localScale = Vector3.zero;
        WarningFlag = 0;
	}

    //경고 창을 닫는 함수
    public void closeWarningPanel()
    {
		transform.localScale = Vector3.zero;
	}

    //아이템이 꽉 찼을 때, 경고문을 띄우는 코루틴을 실행하는 드라이브 함수
    public void itemIsFull()
    {
        //만약 게임이 끝난 경우, 표시하지 않는다.
        if (GameManager.Instance.Turn == 44) return;
        StartCoroutine(displayWarning());
    }

    //경고문을 일정 시간동안 노출시킨 후, 닫는 함수
    IEnumerator displayWarning()
    {
        WarningFlag = 1;
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(waitingTime);
		transform.localScale = Vector3.zero;
        WarningFlag = 0;
		DisplayPlayerItems.Instance.beableButtons();
	}
}
