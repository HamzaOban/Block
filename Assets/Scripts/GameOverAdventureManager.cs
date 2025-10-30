using System.Collections;
using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;
using TMPro;
using GoogleMobileAds.Api;

public class GameOverAdventureManager : MonoBehaviour
{
    [SerializeField] private GridManager _manager;
    [SerializeField] private GameObject[] slots;
    public HashSet<int> numbers = new HashSet<int>();
    public HashSet<int> slotNumbers = new HashSet<int>();

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject revivePanel;

    [SerializeField] private GameObject scoreTextPanel;
    [SerializeField] private GameObject bestScoreTextPanel;


    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AdManager adManager;

    static int numberBool = 0;

    private List<int[,]> puzzlePieces = new List<int[,]>();

    void Start()
    {
        adManager = GameObject.FindAnyObjectByType<AdManager>();
        puzzlePieces.Add(new int[,] { { 1, 0, 0 }, { 1, 0, 0 }, { 1, 1, 1 } }); // 3x3 L parça
        puzzlePieces.Add(new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }); // 2x2 Z parça
        puzzlePieces.Add(new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }); // 3x3 kare parça
        puzzlePieces.Add(new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }); // 3x2 ters T
        puzzlePieces.Add(new int[,] { { 1, 1 }, { 1, 1 } }); // 2x2 kare
        puzzlePieces.Add(new int[,] { { 0, 1 }, { 1, 1 } }); // 2x2 ters L
        puzzlePieces.Add(new int[,] { { 1, 0 }, { 1, 1 } }); // 2x2 L
        puzzlePieces.Add(new int[,] { { 1, 1, 1 }, { 0, 1, 0 } }); // 3x2 T
        puzzlePieces.Add(new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }); // 2x2 ters Z
        puzzlePieces.Add(new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } }); // 3x3 simetrik L
        puzzlePieces.Add(new int[,] { { 1, 1, 1 } }); // 1x3 yatay düz I
        puzzlePieces.Add(new int[,] { { 1 }, { 1 }, { 1 } }); // 1x3 dikey düz I
        puzzlePieces.Add(new int[,] { { 1, 1, 1, 1 } }); // 1x4 yatay düz I
        puzzlePieces.Add(new int[,] { { 1 }, { 1 }, { 1 }, { 1 } }); // 1x4 dikey düz I
        puzzlePieces.Add(new int[,] { { 1, 1, 1, 1, 1 } }); // 1x5 yatay düz I
        puzzlePieces.Add(new int[,] { { 1 }, { 1 }, { 1 }, { 1 }, { 1 } }); // 1x5 dikey düz I
    }



    public IEnumerator CheckGameOver()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var slot in slots)
        {
            var pieceName = slot.gameObject.GetComponentInChildren<GridManager>().gameObject.name.ToString();

            pieceName = pieceName.Substring(0, pieceName.Length - 7);

            var pieceInt = int.Parse(pieceName);

            var slotNumber = int.Parse(slot.name.Substring(4, 1));


            if (IsGameOver(pieceInt))
            {
                slotNumbers.Add(slotNumber);
                foreach (var number in numbers)
                {
                    if (number == pieceInt)
                    {
                        foreach (var slotNumber2 in slotNumbers)
                        {
                            if (slotNumber2 == slotNumber)
                            {
                                numberBool++;
                            }
                        }
                    }
                }

                for (int i = 0; i <= numberBool; i++)
                {
                    if (pieceInt == 0)
                    {
                        numbers.Add(pieceInt + (numberBool * (pieceInt + 10)));
                        Debug.Log(pieceInt + (numberBool * (pieceInt + 10)));
                    }
                    else
                    {
                        numbers.Add(pieceInt + (numberBool * pieceInt));
                        Debug.Log(pieceInt + (numberBool * pieceInt));
                    }

                    Debug.Log(numberBool + " Toplam");

                }
                numbers.Add(pieceInt);


                if (numbers.Count == 3)
                {
                    yield return new WaitForSeconds(1);
                    var adManager = FindAnyObjectByType<AdManager>();

                    ScoreManager.CheckBestScore();


                    if (adManager._rewardedAd != null && adManager._rewardedAd.CanShowAd())
                    {
                        PlayerPrefs.SetInt("Rewarded", 1);
                        Debug.Log("setactive");

                    }
                    else if (adManager._interstitialAd != null && adManager._interstitialAd.CanShowAd())
                    {
                        adManager.ShowInterstitialAd();
                        Debug.Log("interstital");
                        adManager._interstitialAd.OnAdFullScreenContentClosed += () =>
                        {
                            // Ad closed event
                            adManager.LoadInterstitialAd();

                        };

                        adManager._interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
                        {
                            Debug.LogError("Interstitial ad failed to open full screen content " +
                                           "with error : " + error);

                            // Reload the ad so that we can show another as soon as possible.
                            adManager.LoadInterstitialAd();
                        };

                    }

                    Debug.Log("OutLog");



                }
            }

        }

    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Rewarded") == 1)
        {
            revivePanel.SetActive(true);
            PlayerPrefs.SetInt("Rewarded", 0);
        }
    }
    public void GameOverUI()
    {
        gameOverPanel.SetActive(true);
        bestScoreTextPanel.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("BestScore").ToString();
        scoreTextPanel.GetComponent<TextMeshProUGUI>().text = ScoreManager.Score.ToString();
    }

    private bool CanPlacePiece(int[,] piece, int startX, int startY)
    {
        int pieceRows = piece.GetLength(0);
        int pieceCols = piece.GetLength(1);
        // Parça tahtadan taşarsa false döndür
        if (startX + pieceRows > 8 || startY + pieceCols > 8)
        {
            return false;
        }

        // Parça hücrelerinin boş olup olmadığını kontrol et
        for (int i = 0; i < pieceRows; i++)
        {
            for (int j = 0; j < pieceCols; j++)
            {
                var tile = (MyTile)_manager.GetTile(new Vector2Int(startX + i, startY + j));

                if (tile == null || (piece[i, j] == 1 && tile.IsOnTile))
                {
                    return false;
                }

            }
        }
        return true;
    }

    private bool CanPlaceAnyPiece(int pieceInt)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (CanPlacePiece(puzzlePieces[pieceInt], i, j))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsGameOver(int pieceInt)
    {
        return !CanPlaceAnyPiece(pieceInt);
    }
}