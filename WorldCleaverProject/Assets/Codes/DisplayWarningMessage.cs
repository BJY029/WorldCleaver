using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWarningMessage : SingleTon<DisplayWarningMessage>
{
    public float waitingTime = 2.0f;
    void Start()
    {
		transform.localScale = Vector3.zero;
	}

    //��� â�� �ݴ� �Լ�
    public void closeWarningPanel()
    {
		transform.localScale = Vector3.zero;
	}

    //�������� �� á�� ��, ����� ���� �ڷ�ƾ�� �����ϴ� ����̺� �Լ�
    public void itemIsFull()
    {
        StartCoroutine(displayWarning());
    }

    //����� ���� �ð����� �����Ų ��, �ݴ� �Լ�
    IEnumerator displayWarning()
    {
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(waitingTime);
		transform.localScale = Vector3.zero;
	}
}
