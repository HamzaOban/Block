using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject RevivePanel;
    [SerializeField] private GameObject gameOverManager;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject itemSpawner;
    [SerializeField] private GameObject bestScoreImage;
    [SerializeField] private GameObject bestScoreText;
    [SerializeField] private Sprite[] soundAssets;
    [SerializeField] private TextMeshProUGUI soundText;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private Image soundImage;
    [SerializeField] private Image bgmImage;
    [SerializeField] private GameObject moreSettingsPanel;
    private Revive revive;
    private string LoadScene = "";
    [SerializeField] private string ActiveSceneName;




    private void Start()
    {
        var adManager = FindAnyObjectByType<AdManager>();
        adManager.LoadInterstitialAd();
        adManager.LoadRewardedAd();
        adManager.LoadAd();

        revive = FindAnyObjectByType<Revive>();
    }

    private bool settingsBool = false;

    public void OnContactUsButtonClick()
    {
        string email = "hamzaoban3@gmail.com"; // Buraya kendi e-posta adresinizi yazın
        string subject = MyEscapeURL("Contact Us");
        string body = MyEscapeURL("Merhaba, sizinle iletişime geçmek istiyorum.");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void GoToHomeMenu()
    {
        ShowInterstitialAd("MainMenu");
    }

    public void ShowInterstitialAd(string loadScene)
    {
        var adManager = FindAnyObjectByType<AdManager>();
        if (adManager._interstitialAd != null && adManager._interstitialAd.CanShowAd())
        {
            adManager.ShowInterstitialAd();
            adManager._interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                adManager.LoadInterstitialAd();

                Debug.Log("Ad: Closed");
                LoadScene = loadScene;
            };
        }
        else
        {
            LoadScene = loadScene;
        }
    }

    private void Update()
    {
        if(LoadScene == "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");

        }else if(LoadScene == "ClassicGameScene")
        {
            SceneManager.LoadScene("ClassicGameScene");

        }
        else if (LoadScene == "AdventureGameScene")
        {
            SceneManager.LoadScene("AdventureGameScene");

        }
    }

    public void ReplayGame()
    {
        ShowInterstitialAd(ActiveSceneName);
        Revive.reviveClosed = false;
    }

    public void NotReviveAfterAd()
    {
        Revive.reviveClosed = false;

        ShowInterstitialAd(ActiveSceneName);
    }

    public void ReviveTheGame()
    {
        var adManager = FindAnyObjectByType<AdManager>();
        if (adManager._rewardedAd == null || !adManager._rewardedAd.CanShowAd())
        {
            adManager.LoadRewardedAd();
        }


        if (adManager._rewardedAd != null && adManager._rewardedAd.CanShowAd())
        {
            adManager.ShowRewardedAd();

            adManager._rewardedAd.OnAdFullScreenContentOpened += () =>
            {
                Revive.reviveBool = true;
                Debug.Log("reviveBoolopened");
            };

            adManager._rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("reviveBoolclosed");
                Revive.reviveBool = true;


            };
        }
        
    }

    public void OnOffSound(TextMeshProUGUI text)
    {
        var audioManager = FindObjectOfType<AudioManager>();
        if (text.text == "On")
        {
            if(text.name == "SoundOnOffText")
            {
                soundText.text = "Off";
                soundImage.sprite = soundAssets[1];
                for (int i = 0; i < audioManager.sounds.Length; i++)
                {
                    if (audioManager.sounds[i].name == "BGM")
                        continue;
                    audioManager.Mute(audioManager.sounds[i].name,true);
                }
                

            }
            else
            {
                bgmText.text = "Off";
                bgmImage.sprite = soundAssets[1];
                audioManager.Mute(audioManager.sounds[2].name, true);

            }

        }
        else
        {
            if (text.name == "SoundOnOffText")
            {
                soundText.text = "On";
                soundImage.sprite = soundAssets[0];
                for (int i = 0; i < audioManager.sounds.Length; i++)
                {
                    audioManager.Mute(audioManager.sounds[i].name,false);
                }
            }
            else
            {
                bgmText.text = "On";
                bgmImage.sprite = soundAssets[0];
                audioManager.Mute(audioManager.sounds[2].name, false);

            }
        }
    }
    public void openTheSettings()
    {
        if (!settingsBool)
        {
            panel.SetActive(true);
            scoreText.SetActive(false);
            grid.SetActive(false);
            itemSpawner.SetActive(false);
            bestScoreImage.SetActive(false);
            bestScoreText.SetActive(false);
            settingsBool = true;
        }
        else
        {
            if (moreSettingsPanel.activeSelf)
            {
                moreSettingsPanel.SetActive(false);

            }
            panel.SetActive(false);

            
            scoreText.SetActive(true);
            grid.SetActive(true);
            itemSpawner.SetActive(true);
            bestScoreImage.SetActive(true);
            bestScoreText.SetActive(true);
            settingsBool = false;
        }
    }

    public void OpenPrivacyPolicyPage()
    {
        Application.OpenURL("https://doc-hosting.flycricket.io/block-puzzle-yapboz-bulmaca-privacy-policy/ed1ac346-0d3b-4bc2-a970-febb60f9e415/privacy");
    }

    public void OpenTermsOfService()
    {
        Application.OpenURL("https://www.termsfeed.com/live/97c3ee08-8069-47ec-b4df-64fd20363163");
    }

    public void OpenMoreSettingsPanel()
    {
        moreSettingsPanel.SetActive(true);
    }

}
