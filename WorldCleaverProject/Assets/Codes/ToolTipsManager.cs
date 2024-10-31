using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipsManager : SingleTon<ToolTipsManager>, IPointerEnterHandler, IPointerExitHandler
{
    //�ش� ����â�� ����ִ� �ǳ� ������Ʈ
    public GameObject tooltipPanel;
    //�ǳھ��� ������ ���� �ؽ�Ʈ ������Ʈ
    public Text tooltipDescText;
    //�ǳھ��� ������ �̸� �ؽ�Ʈ ������Ʈ
    public Text tooltipNameText;
    //�� ��ư ������Ʈ
    private Button myButton;
    //������ ���� ���ڿ�
    public string itemDesc;
    //������ �̸� ���ڿ�
    public string itemName;

    //�켱 �ʱ�ȭ�� �������ְ�, �ش� â�� ��Ȱ��ȭ��Ų��.
    void Start()
    {
        myButton = GetComponent<Button>();
        tooltipPanel.SetActive(false);
    }
    
    //���� �ش� ��ư�� ���콺�� �÷�����
    public void OnPointerEnter(PointerEventData eventData)
    {
        //���� �� ��ư�� ��Ȱ��ȭ �� ���¶�� �������� �ʴ´�.
        if (!myButton.enabled) return;

        //�� ��ư�� Ȱ��ȭ �� �����̸�
        //�� �ؽ�Ʈ ������Ʈ�� ���ڿ��� �����ϰ�
        tooltipDescText.text = itemDesc;
        tooltipNameText.text = itemName;
        //�ǳ��� Ȱ��ȭ ��Ų��.
        tooltipPanel.SetActive(true);
    }

    //���콺�� ��ư���� �����
    public void OnPointerExit(PointerEventData eventData)
    {
        //��Ȱ��ȭ ��Ų��.
        tooltipPanel.SetActive(false );
    }
}
