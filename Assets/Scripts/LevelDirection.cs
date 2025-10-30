using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelDirection : MonoBehaviour
{
    private int levelId;
    // Start is called before the first frame update
    void Start()
    {
        levelId = int.Parse(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text); 
    }

    public void goToLevelDirection()
    {
        SceneManager.LoadScene("AdventureGameScene");
    }
   
}
