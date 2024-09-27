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

	private void Awake()
	{
        //턴은 0, 1값을 지정해서 Hit이 눌리면 바뀐다.
        //0은 플레이어, 1은 적으로 하며, 일단 플레이어부터 턴을 시작하도록 한다.
        Turn = 0;
	}

    //현재 Turn에 따라 실행하는 Hit의 함수가 다르다.
    //이를 통제하는 게임 메니져 함수이다.
    public void Hit()
    {
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
        }
        //현재 Turn이 1이면
        else if (Turn == 1)
        {
            //플레이어로 턴을 초기화한다.
            Turn = 0;
        }
        //현재 Trun이 44이면
        else if (Turn == 44)
        {
            //어떤 이유로 게임이 끝났으므로 해당 처리를 진행한다.
            Debug.Log("GameOver");
            return;
        }
	}

}
