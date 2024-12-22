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

	private void Awake()
	{
        //턴은 0, 1값을 지정해서 Hit이 눌리면 바뀐다.
        //0은 플레이어, 1은 적으로 하며, 일단 플레이어부터 턴을 시작하도록 한다.
        Turn = 0;
        WhoLose = -1;
	}

    //현재 Turn에 따라 실행하는 Hit의 함수가 다르다.
    //이를 통제하는 게임 메니져 함수이다.
    public void Hit()
    {
        //마을 체력은 매 턴마다 깎이게 된다.
        VillageManager.Instance.VilageHelath = -10f;

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

    //게임의 승자를 판별하는 함수
    public void WhoLoseGame()
    {
        //누가 막타 쳐서 게임이 종료된 경우
		if (GameManager.Instance.TreeController.treeHealth == 0)
		{
            //내가 막타 친 경우
            if (myTurn == 0)
            {
                //패자는 상대편
                WhoLose = 1;
            }
            else
            {
                //패자는 나
                WhoLose = 0;
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
        //적의 마을을 아직 구상하지 않았기 때문에 해당 작업 속히 진행해야 한다.
		if (VillageManager.Instance.VilageHelath == 0)
		{
			WhoLose = 0;
			return;
		}
	}
}
