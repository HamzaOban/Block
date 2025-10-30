using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyGrid.Code;

public class Revive : Singleton<Revive>
{
    [SerializeField] private TextMeshProUGUI CountText;
    [SerializeField] private Slider CountSlider;
    [SerializeField] private GameObject SettingsManager;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject RevivePanel;

    [SerializeField] private GameObject scoreTextPanel;
    [SerializeField] private GameObject bestScoreTextPanel;

    public float elapsedTime = 10f;

    public static bool reviveBool;

    public static bool reviveClosed;

    private void FixedUpdate()
    {
        if (reviveClosed)
        {
            Debug.Log("ReviveClosedTrue");
            RevivePanel.SetActive(false);
            GameOverPanel.SetActive(true);
            bestScoreTextPanel.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("BestScore").ToString();
            scoreTextPanel.GetComponent<TextMeshProUGUI>().text = ScoreManager.Score.ToString();
            reviveClosed = false;
        }
        
    }

    void Start()
    {
        
        //SettingsManager = FindAnyObjectByType<SettingsManager>().gameObject;
        //StartCoroutine(Instance.ReviveCount());
    }
   

    public IEnumerator ReviveCount()
    {
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            CountText.text = ((int)elapsedTime).ToString();
            CountSlider.value = elapsedTime;

            if (reviveBool)
            {
                Debug.Log(reviveBool + " SA");
                RevivePanel.SetActive(false);
                elapsedTime = 10f;
                Spawner.Instance.ReviveItems();
                reviveBool = false;
                break;
            }
            
            if (elapsedTime <= 0)
            {
                var adManager = FindAnyObjectByType<AdManager>();
                adManager.LoadInterstitialAd();
                adManager.LoadRewardedAd();
                Debug.Log(reviveBool + " AS");

                reviveClosed = true;
                GameOverManager.gameOver = false;

            }
            yield return null;
        }
       
    }
}
