using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneUIManager : MonoBehaviour
{
    public Button GoToOpeingButton;
    public GameObject DiaryPanel;

    // Start is called before the first frame update
    void Start()
    {
        GoToOpeingButton.gameObject.SetActive(false);
        DiaryPanel.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDiary()
    {
        DiaryPanel.transform.localScale = Vector3.one;
        StartCoroutine(ActiveButtonStall());
    }

    IEnumerator ActiveButtonStall()
    {
        yield return new WaitForSeconds(5f);
        GoToOpeingButton.gameObject.SetActive(true);
    }
}
