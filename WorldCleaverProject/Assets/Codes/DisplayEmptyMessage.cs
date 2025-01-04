using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEmptyMessage : SingleTon<DisplayEmptyMessage>
{
    public int WarningFlag;
    public float waitingTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        WarningFlag = 0;
    }

    public void closeWarningPanel()
    {
        transform.localScale = Vector3.zero ;
    }

    public void ItemIsEmpty()
    {
        if (GameManager.Instance.Turn == 44) return;
        StartCoroutine(displayEmpty());
    }

    IEnumerator displayEmpty()
    {
        WarningFlag = 1;
        transform.localScale = Vector3.one;
        yield return new WaitForSeconds(waitingTime);
        transform.localScale = Vector3.zero;
        WarningFlag = 0;
    }
}
