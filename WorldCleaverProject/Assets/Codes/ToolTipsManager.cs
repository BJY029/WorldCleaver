using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipsManager : SingleTon<ToolTipsManager>, IPointerEnterHandler, IPointerExitHandler
{
    //해당 설명창을 담고있는 판넬 오브젝트
    public GameObject tooltipPanel;
    //판넬안의 아이템 설명 텍스트 오브젝트
    public Text tooltipDescText;
    //판넬안의 아이템 이름 텍스트 오브젝트
    public Text tooltipNameText;
    //내 버튼 오브젝트
    private Button myButton;
    //아이템 설명 문자열
    public string itemDesc;
    //아이템 이름 문자열
    public string itemName;

    //우선 초기화를 진행해주고, 해당 창을 비활성화시킨다.
    void Start()
    {
        myButton = GetComponent<Button>();
        tooltipPanel.SetActive(false);
    }
    
    //만약 해당 버튼에 마우스가 올려지면
    public void OnPointerEnter(PointerEventData eventData)
    {
        //만약 내 버튼이 비활성화 된 상태라면 실행하지 않는다.
        if (!myButton.enabled) return;

        //내 버튼이 활성화 된 상태이면
        //각 텍스트 오브젝트에 문자열을 삽입하고
        tooltipDescText.text = itemDesc;
        tooltipNameText.text = itemName;
        //판넬을 활성화 시킨다.
        tooltipPanel.SetActive(true);
    }

    //마우스가 버튼에서 벗어나면
    public void OnPointerExit(PointerEventData eventData)
    {
        //비활성화 시킨다.
        tooltipPanel.SetActive(false );
    }
}
