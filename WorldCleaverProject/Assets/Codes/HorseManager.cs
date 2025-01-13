using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseManager : MonoBehaviour
{
    //적 AI의 Hit 코루틴을 정지시키는 변수
    public bool EnemyAIStop;

    //현재 말들이 달리고 있는지 여부를 결정하는 변수
    public bool Horse1_Run;
    public bool Horse2_Run;

    //말들의 경주가 끝났는지 결정하는 변수
    public bool Horse1_End;
    public bool Horse2_End;
    //승자 저장 플래그
    public int WinnerFlag;

    //각 말의 요소들 저장
    public Rigidbody2D Horse1_rb;
    public Rigidbody2D Horse2_rb;
    public GameObject Horse1_rbPrefab;
    public GameObject Horse2_rbPrefab;
    public Sprite Horse1_Img;
    public Sprite Horse2_Img;

    //설명 창의 이미지 
    public Image DImg;
    //타이머 텍스트
    public Text TimerText;

    //말 경주 캔버스 및 요소들
    public Canvas HorseCanvas;
    public GameObject ItemButtons;
    public GameObject WaitingPanel;
    public GameObject DecisionPanel;
    public GameObject ResultPanel;
    public GameObject TimerPanel;

    //각 말들의 처음 위치를 저장하는 변수
    public Vector2 Horse1_InitPos;
    public Vector2 Horse2_InitPos;

    //내 말의 정보를 저장하는 변수
    public Sprite MyHorseImg;
    public string MyHorseName;
    public int MyHorseNum;

    public float timerDuration = 3f;

    // 각종 변수 초기화
    void Start()
    {
        EnemyAIStop = false;

        Horse1_Run = false;
        Horse2_Run = false;
        Horse1_End = false;
        Horse2_End = false;
        WinnerFlag = 0;

        Horse1_InitPos = Horse1_rbPrefab.transform.position;
        Horse2_InitPos = Horse2_rbPrefab.transform.position;

        //말 Canvas을 비활성화 하는 함수
        CloseAllPanel();
        HorseCanvas.enabled = false;
    }

    //게임 전 선택창 노출 및 내 말 결정 함수
    public void PrapareGame()
    {
        //아이템 사용함으로써 마나를 다 써버린 경우
        //해당 맵으로 이동하지 않는다.
        if (GameManager.Instance.Turn == 44) return;

        //적 AI의 HIT 수행 코루틴 잠시 멈춤
        EnemyAIStop = true;

        //카메라 이동과 동시에 Canvas 활성화
        CameraManager.Instance.GoToHorse();

        //만약 현재 Turn이 내 턴이면
        if(GameManager.Instance.Turn == 0)
        {
            //선택 창 활성화
            SelectHorse.Instance.DisplayPanel();
        }
        //적 Turn이면
        else if(GameManager.Instance.Turn == 1)
        {
            //잠시 대기 패널 활성화
            StartCoroutine(WaitForEnemySelect());            
        }
    }

    //게임의 끝을 처리하는 함수
    public void EndGame()
    {
        //누구 한 말이라도 경주가 끝나지 않은 경우, 수행하지 않는다.
        if (Horse1_End == false || Horse2_End == false) return;

        
		Debug.Log(WinnerFlag + "Win!");
        //만약 내 말이 이긴 경우
		if (WinnerFlag == MyHorseNum)
        {
            StartCoroutine(DisplayWinPanel());
        }
        //내 말이 진 경우
        else
        {
            StartCoroutine(DisplayLosePanel());
        }

		WinnerFlag = 0;
	}

    //적이 말을 선택하는 경우 실행되는 코루틴
    IEnumerator WaitForEnemySelect()
    {
        //플레이어에게 잠시 대기 panel을 활성화 시키고
        WaitingPanel.transform.localScale = Vector3.one;

        //Random 함수를 통해 적 AI가 말을 선택
		int enemyHorseNum = Random.Range(1, 3);
        //적 AI가 선택한 말에 따라서, 남은 말을 플레이어에게 할당
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

        //그 다음 내 말의 정보를 표시해주는 패널을 설정하는 함수 실행
        InsertInfoInDecisionPanel();

		yield return new WaitForSeconds(2f);
		//대기 패널 비활성화
        WaitingPanel.transform.localScale = Vector3.zero;
        

        //선택 과정이 끝난 후, 선정된 말을 공개
        StartCoroutine(DisplayDecisionHorse());

        
	}

    //내 말 정보를 표시해주는 패널 정보 설정 함수
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


    //결정된 말의 정보를 보요주는 코루틴 및 타이머 실행
	public IEnumerator DisplayDecisionHorse()
    {
        //결정된 말 패널 활성화 해서 보여주고
        DecisionPanel.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        DecisionPanel.transform.localScale = Vector3.zero;

        //Timer 호출 후 게임 시작
		StartCoroutine(StartTimer());
	}

    //타이머를 실행하는 코루틴
    IEnumerator StartTimer()
    {
        //3초간 초를 세고
        TimerText.text = "3";
        TimerPanel.transform.localScale = Vector3.one;

        float time = timerDuration;
        while (time > 0)
        {
            int IntTime = (int)time;
            TimerText.text = IntTime.ToString();
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        TimerPanel.transform.localScale = Vector3.zero;


        //경주 시작!
        Horse1_Run = true;
        Horse2_Run = true;
    }

    //플레이어가 승리한 경우 수행되는 코루틴
    IEnumerator DisplayWinPanel()
    {
        //결과 패널 활성화
        ResultPanel.transform.localScale = Vector3.one;
        //해당 패널에 딸린 이미지와 텍스트 받아오기
        Image panel = ResultPanel.GetComponentInChildren<Image>();
        Text text = ResultPanel.GetComponentInChildren<Text>();

        //패널 색상 설정
        if (panel != null)
        {
            if(ColorUtility.TryParseHtmlString("#bbd0ff", out Color color))
            {
                panel.color = color;
            }
        }
        else Debug.LogWarning("panelColor null warning");

        //패널 텍스트 설정
        if(text != null)
        {
            text.text = "승리!";
        }
		else Debug.LogWarning("text null warning");

		yield return new WaitForSeconds(2f);
        //다시 비활성화
		ResultPanel.transform.localScale = Vector3.zero;

        //카메라를 다시 Main으로 돌리고
        CameraManager.Instance.BackToGameFromHorse();
        BGMManager.Instance.BackToMain();

        //AI 정지 해제
        EnemyAIStop = false;
        //말의 위치 및 정보 초기화
        GameManager.Instance.Horse1Controller.initAll();
        GameManager.Instance.Horse2Controller.initAll();

        //게임 결과 적용
        GameManager.Instance.PlayerController.setMana(30);
        GameManager.Instance.EnemeyController.setMana(-10);
	}

    //플레이어가 패배한 경우 수행되는 코루틴
    IEnumerator DisplayLosePanel()
    {
		//결과 패널 활성화
		ResultPanel.transform.localScale = Vector3.one;
		//해당 패널에 딸린 이미지와 텍스트 받아오기
		Image panel = ResultPanel.GetComponentInChildren<Image>();
		Text text = ResultPanel.GetComponentInChildren<Text>();
		//패널 색상 설정
		if (panel != null)
		{
			if (ColorUtility.TryParseHtmlString("#FFD6FF", out Color color))
			{
				panel.color = color;
			}
		}
		else Debug.LogWarning("panelColor null warning");

		//패널 텍스트 설정
		if (text != null)
		{
			text.text = "패배...";
		}
		else Debug.LogWarning("text null warning");

		yield return new WaitForSeconds(2f);
		//다시 비활성화
		ResultPanel.transform.localScale = Vector3.zero;

		//카메라를 다시 Main으로 돌리고
		CameraManager.Instance.BackToGameFromHorse();
		BGMManager.Instance.BackToMain();
		//AI 정지 해제
		EnemyAIStop = false;
		//말의 위치 및 정보 초기화
		GameManager.Instance.Horse1Controller.initAll();
		GameManager.Instance.Horse2Controller.initAll();

		//게임 결과 적용
		GameManager.Instance.PlayerController.setMana(-10);
		GameManager.Instance.EnemeyController.setMana(30);
	}

    //모든 패널을 닫는 함수
    public void CloseAllPanel()
    {
        ItemButtons.transform.localScale = Vector3.zero;
        WaitingPanel.transform.localScale = Vector3.zero;
        DecisionPanel.transform.localScale = Vector3.zero;
        ResultPanel.transform.localScale = Vector3.zero;
        TimerPanel.transform.localScale = Vector3.zero;
    }
}
