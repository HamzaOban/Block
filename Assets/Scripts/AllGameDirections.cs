using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllGameDirections : MonoBehaviour
{
    public void GoToClassicGame()
    {
        SceneManager.LoadScene("ClassicGameScene");
    }

    public void GoToAdventureGame()
    {
        SceneManager.LoadScene("AdventureGameMenuScene");
    }

}
