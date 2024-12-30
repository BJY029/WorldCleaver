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

    public int Turn;
    public int myTurn;
    public int WhoLose;
    //사슴이 막타 친 경우 해당 플래그로 승패여부를 판별한다.
    public int DeerLastHit;

    public int MyVillageWeight;
    public int OppositeVillageWeight;
    public int MyDamageTotalCnt;
    private int OppositeDamageTotalCnt;

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
        DeerController.Instance.DeerActivated = false;
        DeerController.Instance.Deer.SetActive(false);
	}

    //현재 Turn에 따라 실행하는 Hit의 함수가 다르다.
    //이를 통제하는 게임 메니져 함수이다.
    public void Hit()
    {
        //마을 체력은 매 턴마다 깎이게 된다.
        VillageManager.Instance.VilageHelath = -20f * MyVillageWeight;
        OppositeVillageManager.Instance.OppositeVillageHealth = -20f * OppositeVillageWeight;
        

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
        if (myTurn == 0)
        {
            GameManager.Instance.TreeController.DamageHitPlayer();
            //그리고 Hit한 데미지들이 비교적 적은 경우, 마을에 가하는 데미지를 증가시킨다.
            checkingMyTotalDamage();
            GameManager.Instance.TreeController.MyDamageCoef = 1.0f;
        }
        else if (myTurn == 1)
        {
            GameManager.Instance.TreeController.DamageHitOppositePlayer();
            checkingOppositeTotalDamage();
			GameManager.Instance.TreeController.OppositeDamageCoef = 1.0f;
		}

		//플래그 초기화
		ItemManager.Instance.Flag = -1;
        Debug.Log("MyTurn : " + myTurn);
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
            DisplayPlayerItems.Instance.disableButtons();
        }
        //현재 Turn이 1이면
        else if (Turn == 1)
        {
            //플레이어로 턴을 초기화한다.
            Turn = 0;
            DisplayPlayerItems.Instance.beableButtons();
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
            //사슴이 막타를 친 경우
            //해당 플래그는 TreeController에서 부여된다.
            if(DeerLastHit == 0)
            {
                WhoLose = 0;
                return;
            }
            else if(DeerLastHit == 1)
            {
                WhoLose = 1;
                return;
            }
            //내가 막타 친 경우
            if (myTurn == 0)
            {
                //패자는 나
                WhoLose = 0;
            }
            else if(myTurn == 1)
            {
                //패자는 상대
                WhoLose = 1;
            }
            return;
		}

        //누군가의 마나가 다 떨어져서 게임이 종료된 경우
        //내 마나가 다 떨어진 경우
		if (GameManager.Instance.PlayerController.Mana == 0)
        {
            //패배는 나
            WhoLose = 0;
            return;
        }
        //상대 마나가 다 떨어진 경우
        else if(GameManager.Instance.EnemeyController.Mana == 0)
        {
            //패자는 상대편
            WhoLose = 1;
            return;
        }

        //각 마을의 체력이 다 닳아서 게임이 끝난된 경우
		if (VillageManager.Instance.VilageHelath == 0)
		{
			WhoLose = 0;
			return;
		}
        else if(OppositeVillageManager.Instance.OppositeVillageHealth == 0)
        {
            WhoLose = 1;
            return;
        }
	}
}
