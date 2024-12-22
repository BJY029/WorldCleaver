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
        //���� 0, 1���� �����ؼ� Hit�� ������ �ٲ��.
        //0�� �÷��̾�, 1�� ������ �ϸ�, �ϴ� �÷��̾���� ���� �����ϵ��� �Ѵ�.
        Turn = 0;
        WhoLose = -1;
	}

    //���� Turn�� ���� �����ϴ� Hit�� �Լ��� �ٸ���.
    //�̸� �����ϴ� ���� �޴��� �Լ��̴�.
    public void Hit()
    {
        //���� ü���� �� �ϸ��� ���̰� �ȴ�.
        VillageManager.Instance.VilageHelath = -10f;

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
            DisplayPlayerItems.Instance.disableButtons();
        }
        //���� Turn�� 1�̸�
        else if (Turn == 1)
        {
            //�÷��̾�� ���� �ʱ�ȭ�Ѵ�.
            Turn = 0;
            DisplayPlayerItems.Instance.beableButtons();
        }
        //���� Trun�� 44�̸�
        else if (Turn == 44)
        {
            //� ������ ������ �������Ƿ� �ش� ó���� �����Ѵ�.
            Debug.Log("GameOver");
            return;
        }
	}

    //������ ���ڸ� �Ǻ��ϴ� �Լ�
    public void WhoLoseGame()
    {
        //���� ��Ÿ �ļ� ������ ����� ���
		if (GameManager.Instance.TreeController.treeHealth == 0)
		{
            //���� ��Ÿ ģ ���
            if (myTurn == 0)
            {
                //���ڴ� �����
                WhoLose = 1;
            }
            else
            {
                //���ڴ� ��
                WhoLose = 0;
            }
            return;
		}

        //�������� ������ �� �������� ������ ����� ���
        //�� ������ �� ������ ���
		if (GameManager.Instance.PlayerController.Mana == 0)
        {
            //�й�� ��
            WhoLose = 0;
            return;
        }
        //��� ������ �� ������ ���
        else if(GameManager.Instance.EnemeyController.Mana == 0)
        {
            //���ڴ� �����
            WhoLose = 1;
            return;
        }

        //�� ������ ü���� �� ��Ƽ� ������ ������ ���
        //���� ������ ���� �������� �ʾұ� ������ �ش� �۾� ���� �����ؾ� �Ѵ�.
		if (VillageManager.Instance.VilageHelath == 0)
		{
			WhoLose = 0;
			return;
		}
	}
}
