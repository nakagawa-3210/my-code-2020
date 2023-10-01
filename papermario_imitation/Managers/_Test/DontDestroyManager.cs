using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    void Start ()
    {
        DontDestroyOnLoad (gameManager);
    }
}