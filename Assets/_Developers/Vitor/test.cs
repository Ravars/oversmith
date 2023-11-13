using System;
using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public LocationSO level;
    public int potato;

    private void Start()
    {
        switch (potato)
        {
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(2);
                break;
            case 3:
                SceneManager.LoadScene(3);
                break;
            case 4:
                SceneManager.LoadScene(4);
                break;
            case 5:
                SceneManager.LoadScene(5);
                break;
            case 6:
                SceneManager.LoadScene(6);
                break;
            case 7:
                SceneManager.LoadScene(7);
                break;
            case 8:
                SceneManager.LoadScene(8);
                break;
            case 9:
                SceneManager.LoadScene(9);
                break;
            case 10:
                SceneManager.LoadScene(10);
                break;
        }
    }
}
