using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HorseManager : SingleTon<HorseManager>
{
    public bool EnemyAIStop;

    public float Horse1_Speed;
    public float Horse2_Speed;

    public Rigidbody2D Horse1_rb;
    public Rigidbody2D Horse2_rb;
    public GameObject Horse1_rbPrefab;
    public GameObject Horse2_rbPrefab;
    public Sprite Horse1_Img;
    public Sprite Horse2_Img;

    public Image DImg;

    public Canvas HorseCanvas;
    public GameObject ItemButtons;
    public GameObject WaitingPanel;
    public GameObject DecisionPanel;
    public GameObject ResultPanel;

    private Vector2 Horse1_InitPos;
    private Vector2 Horse2_InitPos;

    public Sprite MyHorseImg;
    public string MyHorseName;
    public int MyHorseNum;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAIStop = false;

        Horse1_InitPos = Horse1_rbPrefab.transform.position;
        Horse2_InitPos = Horse2_rbPrefab.transform.position;

        CloseAllPanel();
        HorseCanvas.enabled = false;
    }


    public void PrapareGame()
    {
        //적 AI의 HIT 수행 코루틴 잠시 멈춤
        EnemyAIStop = true;
        //카메라 이동과 동시에 Canvas 활성화
        CameraManager.Instance.GoToHorse();

        if(GameManager.Instance.Turn == 0)
        {
            //선택 창 활성화
            SelectHorse.Instance.DisplayPanel();
        }
        else if(GameManager.Instance.Turn == 1)
        {
            StartCoroutine(WaitForEnemySelect());            
        }

    }

    IEnumerator WaitForEnemySelect()
    {
        WaitingPanel.transform.localScale = Vector3.one;
		int enemyHorseNum = Random.Range(1, 3);
		if (enemyHorseNum == 2)
		{
			MyHorseNum = 1;
			MyHorseImg = Horse1_Img;
			MyHorseName = "1번 경주마";
		}
		else
		{
			MyHorseNum = 2;
			MyHorseImg = Horse2_Img;
			MyHorseName = "2번 경주마";
		}

        InsertInfoInDecisionPanel();

		yield return new WaitForSeconds(2f);
		WaitingPanel.transform.localScale = Vector3.zero;

        //선택 과정이 끝난 후, 선정된 말을 공개
        StartCoroutine(DisplayDecisionHorse());
	}

    public void InsertInfoInDecisionPanel()
    {
		Text text = DecisionPanel.GetComponentInChildren<Text>();
		if (text != null)
		{
			text.text = "나의 경주마 : " + MyHorseName;
		}
		else
		{
			Debug.LogError("Text component not found in DecisionPanel.");
		}

		DImg.sprite = MyHorseImg;
	}

    public IEnumerator DisplayDecisionHorse()
    {
        DecisionPanel.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        DecisionPanel.transform.localScale = Vector3.zero;
    }

    public void CloseAllPanel()
    {
        ItemButtons.transform.localScale = Vector3.zero;
        WaitingPanel.transform.localScale = Vector3.zero;
        DecisionPanel.transform.localScale = Vector3.zero;
        ResultPanel.transform.localScale = Vector3.zero;

    }
}
