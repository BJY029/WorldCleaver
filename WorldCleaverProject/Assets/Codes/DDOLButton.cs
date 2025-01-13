using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLButton : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManagment.Instance.ChangeScene(sceneName);
    }
}
