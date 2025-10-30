using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject linePrefab;
    public RectTransform contentPanel; // ScrollView içindeki panel
    public int totalLevels = 100; // Toplam buton sayısı
    public float stepSize = 400; // Butonlar arası yatay mesafe
    public float stepSizeY = 100f; // Satırlar arası dikey mesafe
    public Vector2 buttonSpacing = new Vector2(200, 200);  // Butonlar arasındaki boşluk

    [SerializeField] private Sprite[] lineSprites;
    [SerializeField] private Sprite[] levelSprites;


    private List<Vector2> buttonPositions = new List<Vector2>();

    private List<GameObject> levelButtons = new List<GameObject>();


    private void Start()
    {
        PlayerPrefs.SetString("Adventure0", "Passed");
        PlayerPrefs.SetString("Adventure1", "Passed");
        PlayerPrefs.SetString("Adventure2", "Passed");


        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        float startX = -buttonSpacing.x;  // Başlangıç X pozisyonu
        float startY = 0;  // Başlangıç Y pozisyonu
        GameObject previousButton = null;

        for (int i = 0; i < totalLevels; i++)
        {
            GameObject button = Instantiate(levelButtonPrefab, contentPanel);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            var LevelText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.GetString("Adventure" + i.ToString()) == "Passed")
            {
                button.GetComponent<Image>().sprite = levelSprites[0];
                button.GetComponent<Button>().interactable = true;

            }
            else
            {
                button.GetComponent<Image>().sprite = levelSprites[1];
                button.GetComponent<Button>().interactable = false;


            }
            LevelText.text = (i + 1).ToString();
            int row = i / 3;
            int column = i % 3;

            float xOffset = startX + (column * buttonSpacing.x);
            float yOffset = startY - (row * buttonSpacing.y);

            if (row % 2 == 1)  // Satır çiftse, butonları ters sırada yerleştir
            {
                xOffset = startX + ((2 - column) * buttonSpacing.x);
            }

            rectTransform.anchoredPosition = new Vector2(xOffset, yOffset);

            levelButtons.Add(button);

            // Eğer önceki buton varsa, yeni buton ile arasında bir line oluştur
            if (previousButton != null)
            {
                CreateLineBetweenButtons(previousButton, button,i);
            }

            previousButton = button;
        }
    }

    private void CreateLineBetweenButtons(GameObject startButton, GameObject endButton,int index)
    {
        Vector3 startPos = startButton.GetComponent<RectTransform>().anchoredPosition;
        Vector3 endPos = endButton.GetComponent<RectTransform>().anchoredPosition;

        // Line'ı oluştur ve LineRenderer'ı ayarla
        GameObject line = Instantiate(linePrefab, contentPanel);
        RectTransform rectTransform = line.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = (startPos + endPos) / 2;
        if(index % 3 == 0)
        {
            line.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (PlayerPrefs.GetString("Adventure" + index.ToString()) == "Passed")
        {
            line.GetComponent<Image>().sprite = lineSprites[0];
        }
        else
        {
            line.GetComponent<Image>().sprite = lineSprites[1];

        }

    }
}