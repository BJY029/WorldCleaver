using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayEnemyItems : SingleTon<DisplayEnemyItems>
{
    public List<Button> enemyItems = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        enemyItems.AddRange(GetComponentsInChildren<Button>());

        for(int i = 0; i < enemyItems.Count; i++)
        {
            enemyItems[i].enabled = false;
            enemyItems[i].interactable = false;
        }
    }

    public void insertItem(Item item)
    {
        int flag = -1;
        for (int i = 0; i < enemyItems.Count; i++)
        {
            if (!enemyItems[i].enabled)
            {
                flag = i;
                break;
            }
        }

        //Full error
        if (flag == -1)
        {
            return;
        }

        Image imgComponent = enemyItems[flag].GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = item.icon;
        }

        ToolTipsManager script = enemyItems[flag].GetComponentInChildren<ToolTipsManager>();
        script.itemDesc = item.description;
        script.itemName = item.itemName;

        enemyItems[flag].enabled = true;
    }

	public void removeItem(Sprite icon)
	{
        //���� �Ѿ�°� null�̸� �������� �ʴ´�.(��������)
        if (icon == null) return;

        //��ư ����Ʈ�� ���鼭
		for(int i = 0; i < enemyItems.Count; i++)
        {
            //���� �ش� ����Ʈ�� ��Ȱ��ȭ �� ���, �� ����ִ� ���, �׳� �Ѿ��.
            if(!enemyItems[i].enabled) continue;
            //�ش� ��ư ����Ʈ�κ��� �̹��� ��������Ʈ�� �ҷ��´�.
            Sprite sprite = enemyItems[i].GetComponentInChildren<Image>().sprite;
            //���� ���ڷ� �Ѱ��� ��������Ʈ�� ���� ���
            if(sprite == icon)
            {
                //�ش� �̹����� �����ϰ�
                enemyItems[i].GetComponentInChildren<Image>().sprite = null;
                //���� ��ũ��Ʈ ���� �ʱ�ȭ�Ѵ�.
                ToolTipsManager script = enemyItems[i].GetComponentInChildren<ToolTipsManager>();
                script.itemDesc = null;
                script.itemName = null;

                //�׸��� �ش� ������ ��ư�� ��Ȱ��ȭ ��Ų��.
                enemyItems[i].enabled = false;
				enemyItems[i].interactable = false;
				break;
            }
        }
	}
}
