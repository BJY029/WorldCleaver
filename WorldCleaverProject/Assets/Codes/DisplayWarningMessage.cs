using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWarningMessage : MonoBehaviour
{
    public int WarningFlag;
    public float waitingTime = 2.0f;
    void Start()
    {
		transform.localScale = Vector3.zero;
        WarningFlag = 0;
	}

    //��� â�� �ݴ� �Լ�
    public void closeWarningPanel()
    {
		transform.localScale = Vector3.zero;
	}

    //�������� �� á�� ��, ����� ���� �ڷ�ƾ�� �����ϴ� ����̺� �Լ�
    public void itemIsFull()
    {
        //���� ������ ���� ���, ǥ������ �ʴ´�.
        if (GameManager.Instance.Turn == 44) return;
        StartCoroutine(displayWarning());
    }

    //����� ���� �ð����� �����Ų ��, �ݴ� �Լ�
    IEnumerator displayWarning()
    {
        WarningFlag = 1;
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(waitingTime);
		transform.localScale = Vector3.zero;
        WarningFlag = 0;
		GameManager.Instance.UIManager.ItemSelecting = false;
		GameManager.Instance.DisplayPlayerItems.beableButtons();
	}
}
