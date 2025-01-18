using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerClickHandler
{
    public int itemIndex;
    private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();

		if (button == null)
		{
			Debug.LogError("Button component is missing on this GameObject.");
		}
	}


	public void OnPointerClick(PointerEventData eventData)
    {

		if (GameManager.Instance.DisplayPlayerItems.BlockButton) return;
		if (GameManager.Instance.UIManager.ItemSelecting) return;


		if (GameManager.Instance.Turn == 0)
		{
			if (button == null || !button.enabled)
			{
				return;
			}
		}
		else if (GameManager.Instance.Turn == 1)
		{
			if (button == null || !button.interactable)
			{
				return;
			}
		}
		else if (GameManager.Instance.Turn == 44) return;

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.Instance.DisplayPlayerItems.removeItem(itemIndex);
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
			GameManager.Instance.DisplayPlayerItems.removeItem(itemIndex, true);
		}
    }
}
