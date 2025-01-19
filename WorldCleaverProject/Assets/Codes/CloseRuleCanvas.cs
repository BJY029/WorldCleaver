using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRuleCanvas : MonoBehaviour
{
    public Canvas RuleCanvas;

    public void CloseCanvas()
    {
        RuleCanvas.gameObject.SetActive(false);
        BGMManager.Instance.MainBGM.Play();
    }
}
