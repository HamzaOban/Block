using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int Score;
    public static int Combo;
    public static int MoveCount;
    public int BestScore;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;


    private void Start()
    {
        Score = 0;

        if (SceneManagerForMod.Mod)
        {
            Debug.Log("ScoreModGirdi");
            BestScore = PlayerPrefs.GetInt("BestScore");
            bestScoreText.text = BestScore.ToString();
            Combo = 0;
            MoveCount = 0;
        }
     
    }

    public static void CheckBestScore()
    {
        if (PlayerPrefs.GetInt("BestScore") < Score)
            PlayerPrefs.SetInt("BestScore", Score);
    }

    public void ScoreIncreaseWithDestroyColumnOrRow()
    {
        var ComboScore = Combo * 10;
        Score += ComboScore;
        MoveCount = 0;
        Debug.Log(Combo.ToString());
    }

    public void ScoreIncreaseWithTileCount(int tileCount)
    {
        Score += tileCount;
    }
    private void FixedUpdate()
    {
        scoreText.text = Score.ToString();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
    }
}
