using System;
using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;

public class test : MonoBehaviour
{
    public LocationSO level;

    private void Start()
    {
        if (level.tutorialDataSo == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("not null");
        }
    }
}
