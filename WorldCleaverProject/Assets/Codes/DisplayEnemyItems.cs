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
        //만약 넘어온게 null이면 제거하지 않는다.(오류방지)
        if (icon == null) return;

        //버튼 리스트를 돌면서
		for(int i = 0; i < enemyItems.Count; i++)
        {
            //만약 해당 리스트가 비활성화 된 경우, 즉 비어있는 경우, 그냥 넘어간다.
            if(!enemyItems[i].enabled) continue;
            //해당 버튼 리스트로부터 이미지 스프라이트를 불러온다.
            Sprite sprite = enemyItems[i].GetComponentInChildren<Image>().sprite;
            //만약 인자로 넘겨진 스프라이트와 같은 경우
            if(sprite == icon)
            {
                //해당 이미지를 삭제하고
                enemyItems[i].GetComponentInChildren<Image>().sprite = null;
                //설명 스크립트 또한 초기화한다.
                ToolTipsManager script = enemyItems[i].GetComponentInChildren<ToolTipsManager>();
                script.itemDesc = null;
                script.itemName = null;

                //그리고 해당 아이템 버튼을 비활성화 시킨다.
                enemyItems[i].enabled = false;
				enemyItems[i].interactable = false;
				break;
            }
        }
	}
}
