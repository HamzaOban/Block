using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerForMod : MonoBehaviour
{
    public static bool Mod = true;
    void Awake()
    {
        /*if(SceneManager.GetActiveScene().name == "ClassicGameScene")
        {
            Mod = true;
        }
        else
        {
            Mod = false;
        }*/
        Mod = true;
    }
}
