using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseManager : MonoBehaviour
{
    //�� AI�� Hit �ڷ�ƾ�� ������Ű�� ����
    public bool EnemyAIStop;

    //���� ������ �޸��� �ִ��� ���θ� �����ϴ� ����
    public bool Horse1_Run;
    public bool Horse2_Run;

    //������ ���ְ� �������� �����ϴ� ����
    public bool Horse1_End;
    public bool Horse2_End;
    //���� ���� �÷���
    public int WinnerFlag;

    //�� ���� ��ҵ� ����
    public Rigidbody2D Horse1_rb;
    public Rigidbody2D Horse2_rb;
    public GameObject Horse1_rbPrefab;
    public GameObject Horse2_rbPrefab;
    public Sprite Horse1_Img;
    public Sprite Horse2_Img;

    //���� â�� �̹��� 
    public Image DImg;
    //Ÿ�̸� �ؽ�Ʈ
    public Text TimerText;

    //�� ���� ĵ���� �� ��ҵ�
    public Canvas HorseCanvas;
    public GameObject ItemButtons;
    public GameObject WaitingPanel;
    public GameObject DecisionPanel;
    public GameObject ResultPanel;
    public GameObject TimerPanel;

    //�� ������ ó�� ��ġ�� �����ϴ� ����
    public Vector2 Horse1_InitPos;
    public Vector2 Horse2_InitPos;

    //�� ���� ������ �����ϴ� ����
    public Sprite MyHorseImg;
    public string MyHorseName;
    public int MyHorseNum;

    public float timerDuration = 3f;

    // ���� ���� �ʱ�ȭ
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

        //�� Canvas�� ��Ȱ��ȭ �ϴ� �Լ�
        CloseAllPanel();
        HorseCanvas.enabled = false;
    }

    //���� �� ����â ���� �� �� �� ���� �Լ�
    public void PrapareGame()
    {
        //������ ��������ν� ������ �� ����� ���
        //�ش� ������ �̵����� �ʴ´�.
        if (GameManager.Instance.Turn == 44) return;

        //�� AI�� HIT ���� �ڷ�ƾ ��� ����
        EnemyAIStop = true;

        //ī�޶� �̵��� ���ÿ� Canvas Ȱ��ȭ
        CameraManager.Instance.GoToHorse();

        //���� ���� Turn�� �� ���̸�
        if(GameManager.Instance.Turn == 0)
        {
            //���� â Ȱ��ȭ
            SelectHorse.Instance.DisplayPanel();
        }
        //�� Turn�̸�
        else if(GameManager.Instance.Turn == 1)
        {
            //��� ��� �г� Ȱ��ȭ
            StartCoroutine(WaitForEnemySelect());            
        }
    }

    //������ ���� ó���ϴ� �Լ�
    public void EndGame()
    {
        //���� �� ���̶� ���ְ� ������ ���� ���, �������� �ʴ´�.
        if (Horse1_End == false || Horse2_End == false) return;

        
		Debug.Log(WinnerFlag + "Win!");
        //���� �� ���� �̱� ���
		if (WinnerFlag == MyHorseNum)
        {
            StartCoroutine(DisplayWinPanel());
        }
        //�� ���� �� ���
        else
        {
            StartCoroutine(DisplayLosePanel());
        }

		WinnerFlag = 0;
	}

    //���� ���� �����ϴ� ��� ����Ǵ� �ڷ�ƾ
    IEnumerator WaitForEnemySelect()
    {
        //�÷��̾�� ��� ��� panel�� Ȱ��ȭ ��Ű��
        WaitingPanel.transform.localScale = Vector3.one;

        //Random �Լ��� ���� �� AI�� ���� ����
		int enemyHorseNum = Random.Range(1, 3);
        //�� AI�� ������ ���� ����, ���� ���� �÷��̾�� �Ҵ�
		if (enemyHorseNum == 2)
		{
			MyHorseNum = 1;
			MyHorseImg = Horse1_Img;
			MyHorseName = "1�� ���ָ�";
		}
		else
		{
			MyHorseNum = 2;
			MyHorseImg = Horse2_Img;
			MyHorseName = "2�� ���ָ�";
		}

        //�� ���� �� ���� ������ ǥ�����ִ� �г��� �����ϴ� �Լ� ����
        InsertInfoInDecisionPanel();

		yield return new WaitForSeconds(2f);
		//��� �г� ��Ȱ��ȭ
        WaitingPanel.transform.localScale = Vector3.zero;
        

        //���� ������ ���� ��, ������ ���� ����
        StartCoroutine(DisplayDecisionHorse());

        
	}

    //�� �� ������ ǥ�����ִ� �г� ���� ���� �Լ�
    public void InsertInfoInDecisionPanel()
    {
		Text text = DecisionPanel.GetComponentInChildren<Text>();
		if (text != null)
		{
			text.text = "���� ���ָ� : " + MyHorseName;
		}
		else
		{
			Debug.LogError("Text component not found in DecisionPanel.");
		}

		DImg.sprite = MyHorseImg;
	}


    //������ ���� ������ �����ִ� �ڷ�ƾ �� Ÿ�̸� ����
	public IEnumerator DisplayDecisionHorse()
    {
        //������ �� �г� Ȱ��ȭ �ؼ� �����ְ�
        DecisionPanel.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(2f);
        DecisionPanel.transform.localScale = Vector3.zero;

        //Timer ȣ�� �� ���� ����
		StartCoroutine(StartTimer());
	}

    //Ÿ�̸Ӹ� �����ϴ� �ڷ�ƾ
    IEnumerator StartTimer()
    {
        //3�ʰ� �ʸ� ����
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


        //���� ����!
        Horse1_Run = true;
        Horse2_Run = true;
    }

    //�÷��̾ �¸��� ��� ����Ǵ� �ڷ�ƾ
    IEnumerator DisplayWinPanel()
    {
        //��� �г� Ȱ��ȭ
        ResultPanel.transform.localScale = Vector3.one;
        //�ش� �гο� ���� �̹����� �ؽ�Ʈ �޾ƿ���
        Image panel = ResultPanel.GetComponentInChildren<Image>();
        Text text = ResultPanel.GetComponentInChildren<Text>();

        //�г� ���� ����
        if (panel != null)
        {
            if(ColorUtility.TryParseHtmlString("#bbd0ff", out Color color))
            {
                panel.color = color;
            }
        }
        else Debug.LogWarning("panelColor null warning");

        //�г� �ؽ�Ʈ ����
        if(text != null)
        {
            text.text = "�¸�!";
        }
		else Debug.LogWarning("text null warning");

		yield return new WaitForSeconds(2f);
        //�ٽ� ��Ȱ��ȭ
		ResultPanel.transform.localScale = Vector3.zero;

        //ī�޶� �ٽ� Main���� ������
        CameraManager.Instance.BackToGameFromHorse();
        BGMManager.Instance.BackToMain();

        //AI ���� ����
        EnemyAIStop = false;
        //���� ��ġ �� ���� �ʱ�ȭ
        GameManager.Instance.Horse1Controller.initAll();
        GameManager.Instance.Horse2Controller.initAll();

        //���� ��� ����
        GameManager.Instance.PlayerController.setMana(30);
        GameManager.Instance.EnemeyController.setMana(-10);
	}

    //�÷��̾ �й��� ��� ����Ǵ� �ڷ�ƾ
    IEnumerator DisplayLosePanel()
    {
		//��� �г� Ȱ��ȭ
		ResultPanel.transform.localScale = Vector3.one;
		//�ش� �гο� ���� �̹����� �ؽ�Ʈ �޾ƿ���
		Image panel = ResultPanel.GetComponentInChildren<Image>();
		Text text = ResultPanel.GetComponentInChildren<Text>();
		//�г� ���� ����
		if (panel != null)
		{
			if (ColorUtility.TryParseHtmlString("#FFD6FF", out Color color))
			{
				panel.color = color;
			}
		}
		else Debug.LogWarning("panelColor null warning");

		//�г� �ؽ�Ʈ ����
		if (text != null)
		{
			text.text = "�й�...";
		}
		else Debug.LogWarning("text null warning");

		yield return new WaitForSeconds(2f);
		//�ٽ� ��Ȱ��ȭ
		ResultPanel.transform.localScale = Vector3.zero;

		//ī�޶� �ٽ� Main���� ������
		CameraManager.Instance.BackToGameFromHorse();
		BGMManager.Instance.BackToMain();
		//AI ���� ����
		EnemyAIStop = false;
		//���� ��ġ �� ���� �ʱ�ȭ
		GameManager.Instance.Horse1Controller.initAll();
		GameManager.Instance.Horse2Controller.initAll();

		//���� ��� ����
		GameManager.Instance.PlayerController.setMana(-10);
		GameManager.Instance.EnemeyController.setMana(30);
	}

    //��� �г��� �ݴ� �Լ�
    public void CloseAllPanel()
    {
        ItemButtons.transform.localScale = Vector3.zero;
        WaitingPanel.transform.localScale = Vector3.zero;
        DecisionPanel.transform.localScale = Vector3.zero;
        ResultPanel.transform.localScale = Vector3.zero;
        TimerPanel.transform.localScale = Vector3.zero;
    }
}
