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
        //���� 0, 1���� �����ؼ� Hit�� ������ �ٲ��.
        //0�� �÷��̾�, 1�� ������ �ϸ�, �ϴ� �÷��̾���� ���� �����ϵ��� �Ѵ�.
        Turn = 0;
	}

    //���� Turn�� ���� �����ϴ� Hit�� �Լ��� �ٸ���.
    //�̸� �����ϴ� ���� �޴��� �Լ��̴�.
    public void Hit()
    {
        //���� �÷��̾� ���̸�
        if (Turn == 0)
        {
            //�켱 �� ���� ���� �� ��
            myTurn = 0;
            //�÷��̾� ��Ʈ�ѷ��� Hit �Լ��� �����Ѵ�.
            PlayerController.Hit();
        }
        else if (Turn == 1) //���� �� ���̸�
        {
            //���� ���� ������ ��
            myTurn = 1; 
            //�� ��Ʈ�ѷ��� Hit �Լ��� �����Ѵ�.
            EnemeyController.Hit();
        }
    }

    //Turn�� ��ü�ϴ� �Լ��̴�.
    //GameManager ���� �Ѱ��Ѵ�.
    public void ChangeTurn()
    {
        //���� Turn�� 0�̸�
        if (Turn == 0)
        {
            //������ ���� �ʱ�ȭ�Ѵ�.
            Turn = 1;
        }
        //���� Turn�� 1�̸�
        else if (Turn == 1)
        {
            //�÷��̾�� ���� �ʱ�ȭ�Ѵ�.
            Turn = 0;
        }
        //���� Trun�� 44�̸�
        else if (Turn == 44)
        {
            //� ������ ������ �������Ƿ� �ش� ó���� �����Ѵ�.
            Debug.Log("GameOver");
            return;
        }
	}

}
