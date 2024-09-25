using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public PlayerController PlayerController;
    public TreeController TreeController;
    public AnimationManager AnimationManager;
    public void test()
    {
        Debug.Log("GameManager");
    }
}
