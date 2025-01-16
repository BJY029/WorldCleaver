using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public PlayerController PlayerController;
    public EnemeyController EnemeyController;
    public TreeController TreeController;
    public AnimationManager AnimationManager;
    public CameraManager CameraManager;
    public DeerController DeerController;
    public DisplayEmptyMessage DisplayEmptyMessage;
    public DisplayEnemyItems DisplayEnemyItems;
    //public DisplayItems DisplayItems;
    public DisplayPlayerItems DisplayPlayerItems;
    public DisplayWarningMessage DisplayWarningMessage;
    public EffectAudioManager EffectAudioManager;
    public EnemyAI EnemyAI;
    public EnemyDeerController EnemyDeerController;
    public EnemyEagleController EnemyEagleController;
    public Horse1Controller Horse1Controller;
    public Horse2Controller Horse2Controller;
    public HorseManager HorseManager;
    public ItemManager ItemManager;
    public VillageManager VillageManager;
    public OppositeVillageManager OppositeVillageManager;
    public PlayerEagleController PlayerEagleController;
    //public SelectHorse SelectHorse;
    public SmokeEffect SmokeEffect;
    //public ToolTipsManager ToolTipsManager;
    public UIManager UIManager;

    public int Turn;
    public int myTurn;
    public int WhoLose;
    //사슴이 막타 친 경우 해당 플래그로 승패여부를 판별한다.
    public int DeerLastHit;
    public int ReasonFlag; //무슨 이유로 게임을 졌는지 저장하는 플래그
    //0 : 막타, 1 : 마나가 다 떨어짐, 2 : 마을 체력이 다 떨어짐

    public int MyVillageWeight;
    public int OppositeVillageWeight;
    public int MyDamageTotalCnt;
    public int OppositeDamageTotalCnt;

	private void Awake()
	{
        //턴은 0, 1값을 지정해서 Hit이 눌리면 바뀐다.
        //0은 플레이어, 1은 적으로 하며, 일단 플레이어부터 턴을 시작하도록 한다.
        Turn = 0;
        WhoLose = -1;
        DeerLastHit = -1;

        MyVillageWeight = 1;
        OppositeVillageWeight = 1;

        MyDamageTotalCnt = 0;
        OppositeDamageTotalCnt = 0;

        //사슴 아이템 비활성화
        GameManager.Instance.DeerController.DeerActivated = false;
        GameManager.Instance.DeerController.Deer.SetActive(false);
        GameManager.Instance.EnemyDeerController.DeerActivated = false;
        GameManager.Instance.EnemyDeerController.Deer.SetActive(false) ;
	}

    //현재 Turn에 따라 실행하는 Hit의
    //함수가 다르다.
    //이를 통제하는 게임 메니져 함수이다.
    public void Hit()
    {
        //마을 체력은 매 턴마다 깎이게 된다.
        GameManager.Instance.VillageManager.VilageHelath = -20f * MyVillageWeight;
        GameManager.Instance.OppositeVillageManager.OppositeVillageHealth = -20f * OppositeVillageWeight;

        GameManager.Instance.VillageManager.ChangeBackGround();
        GameManager.Instance.OppositeVillageManager.ChangeBackGround();

        //0이 아니면, 0이 될때까지 1씩 감소시킨다.
        //0이 아닌경우, 체력 바를 숨긴다. 즉, 연막탄 아이템의 시전 시간이다.
        if (GameManager.Instance.ItemManager.smokeFlag != 0) GameManager.Instance.ItemManager.smokeFlag -= 1;
        if (GameManager.Instance.ItemManager.smokeFlag == 0)
        {
            GameManager.Instance.EffectAudioManager.StopSmoke();
            GameManager.Instance.SmokeEffect.StopSmoke();
        }

        //현재 플레이어 턴이면
        if (Turn == 0)
        {
            //우선 내 턴을 저장 한 후
            myTurn = 0;
            //플레이어 컨트롤러의 Hit 함수를 실행한다.
            PlayerController.Hit();
        }
        else if (Turn == 1) //현재 적 턴이면
        {
            //적의 턴을 저장한 후
            myTurn = 1; 
            //적 컨트롤러의 Hit 함수를 실행한다.
            EnemeyController.Hit();
        }

        //각 Hit한 사람에 맞게 데미지를 부여한다.
        StartCoroutine(DamageTree(myTurn));
        //      if (myTurn == 0)
        //      {
        //          GameManager.Instance.TreeController.DamageHitPlayer();
        //          //그리고 Hit한 데미지들이 비교적 적은 경우, 마을에 가하는 데미지를 증가시킨다.
        //          checkingMyTotalDamage();
        //          GameManager.Instance.TreeController.MyDamageCoef = 1.0f;
        //      }
        //      else if (myTurn == 1)
        //      {
        //          GameManager.Instance.TreeController.DamageHitOppositePlayer();
        //          checkingOppositeTotalDamage();
        //	GameManager.Instance.TreeController.OppositeDamageCoef = 1.0f;
        //}

        
        Debug.Log("Turn : " + Turn);
	}

    IEnumerator DamageTree(int turn)
    {
        yield return new WaitForSeconds(Instance.AnimationManager.WaitHitTime);
        if(turn == 0)
        {
			GameManager.Instance.TreeController.DamageHitPlayer();
			//그리고 Hit한 데미지들이 비교적 적은 경우, 마을에 가하는 데미지를 증가시킨다.
			checkingMyTotalDamage();
			GameManager.Instance.TreeController.MyDamageCoef = 1.0f;
		}
        else if(turn == 1)
        {
			GameManager.Instance.TreeController.DamageHitOppositePlayer();
			checkingOppositeTotalDamage();
			GameManager.Instance.TreeController.OppositeDamageCoef = 1.0f;
		}
        Debug.Log("Turn : " + Turn);

        //만약 막타로 게임이 끝나버린 경우
        if (Turn != 44)
        {
            GameManager.Instance.CameraManager.changeCamera();

            //이전 턴이 내 차례였다면, 적으로 턴을 넘긴다.
            if (turn == 0)
            {
                GameManager.Instance.EnemyAI.EnemyTurnBehavior();
            }
            else if (turn == 1)
            {
                if (GameManager.Instance.DisplayPlayerItems.isFull() == false)
                {
                    GameManager.Instance.ItemManager.SetRandomItemsOnButtons(); //적 턴에서, Hit가 발생되면, 아이템 선택 창을 표시한다.
                }
                else //아이템이 꽉 찬 경우
                {
                    GameManager.Instance.DisplayWarningMessage.itemIsFull(); //경고 메시지 출력
                }
            }
        }
		//Instance.AnimationManager.Hit();
		//플래그 초기화
		GameManager.Instance.ItemManager.Flag = -1;
        BGMManager.Instance.CheckState();
	}

    //Turn을 교체하는 함수이다.
    //GameManager 에서 총괄한다.
    public void ChangeTurn()
    {
        //현재 Turn이 0이면
        if (Turn == 0)
        {
            //적으로 턴을 초기화한다.
            Turn = 1;
            GameManager.Instance.DisplayPlayerItems.disableButtons();
            //적으로 턴을 변경 후, 함수를 호출한다.
            //EnemyAI.Instance.EnemyTurnBehavior();
        }
        //현재 Turn이 1이면
        else if (Turn == 1)
        {
            //플레이어로 턴을 초기화한다.
            Turn = 0;
            //DisplayPlayerItems.Instance.beableButtons();
        }
        //현재 Trun이 44이면
        else if (Turn == 44)
        {
            //어떤 이유로 게임이 끝났으므로 해당 처리를 진행한다.
            Debug.Log("GameOver");
            return;
        }
	}
    
    //마을에 가해지는 데미지의 지수를 조정하는 함수
    //내가 가하는 데미지가 3턴 이내로 100이상을 넘지 않을 경우
    //마을에 가해지는 데미지가 증가하게 된다.(x2, x3, x4, ...)
    public void checkingMyTotalDamage()
    {
        int Damage = GameManager.Instance.TreeController.HitDamage;
        if(Damage > 100)
        {
            MyDamageTotalCnt = 0;
        }
        else
        {
            MyDamageTotalCnt++;
        }

        MyVillageWeight = MyDamageTotalCnt / 3 + 1;
    }


	//적의 마을에 가해지는 데미지의 지수를 조정하는 함수
	//적이 가하는 데미지가 3턴 이내로 100이상을 넘지 않을 경우
	//적 마을에 가해지는 데미지가 증가하게 된다.(x2, x3, x4, ...)
	public void checkingOppositeTotalDamage()
    {
        int Damage = GameManager.Instance.TreeController.HitDamageFromOp;
        if(Damage > 100)
        {
            OppositeDamageTotalCnt = 0;
        }
        else
        {
            OppositeDamageTotalCnt++;
        }
        OppositeVillageWeight = OppositeDamageTotalCnt / 3 + 1;
    }

    //게임의 승자를 판별하는 함수
    public void WhoLoseGame()
    {
        //누가 막타 쳐서 게임이 종료된 경우
		if (GameManager.Instance.TreeController.treeHealth == 0)
		{
            ReasonFlag = 0;
            //Debug.Log("나무의 체력이 다 되었으며,");
            //사슴이 막타를 친 경우
            //해당 플래그는 TreeController에서 부여된다.
            if(DeerLastHit == 0)
            {
                //Debug.Log("플레이어가 막타를 쳤습니다.");
                WhoLose = 0;
                return;
            }
            else if(DeerLastHit == 1)
            {
				//Debug.Log("적이 막타를 쳤습니다.");
				WhoLose = 1;
                return;
            }
            //내가 막타 친 경우
            if (myTurn == 0)
            {
				//Debug.Log("플레이어가 막타를 쳤습니다.");
				//패자는 나
				WhoLose = 0;
            }
            else if(myTurn == 1)
            {
				//Debug.Log("적이 막타를 쳤습니다.");
				//패자는 상대
				WhoLose = 1;
            }
            return;
		}

        //누군가의 마나가 다 떨어져서 게임이 종료된 경우
        //내 마나가 다 떨어진 경우
		if (GameManager.Instance.PlayerController.Mana == 0)
        {
			//Debug.Log("나의 마나가 다 떨어져서 게임을 졌습니다.");
			ReasonFlag = 1;
            //패배는 나
            WhoLose = 0;
            return;
        }
        //상대 마나가 다 떨어진 경우
        else if(GameManager.Instance.EnemeyController.Mana == 0)
        {
			//Debug.Log("적의 마나가 다 떨어져서 게임을 졌습니다.");
			ReasonFlag = 1;
            //패자는 상대편
            WhoLose = 1;
            return;
        }

        //각 마을의 체력이 다 닳아서 게임이 끝난된 경우
		if (GameManager.Instance.VillageManager.VilageHelath == 0)
		{
			//Debug.Log("나의 마을 체력이 모두 소진되어 게임을 졌습니다..");
			ReasonFlag = 2;
			WhoLose = 0;
			return;
		}
        else if(GameManager.Instance.OppositeVillageManager.OppositeVillageHealth == 0)
        {
			//Debug.Log("적의 마을 체력이 모두 소진되어 게임을 졌습니다..");
			ReasonFlag = 2;
            WhoLose = 1;
            return;
        }
	}
}
