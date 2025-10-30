using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerAdventure : MonoBehaviour
{
    [SerializeField] Slider scoreSlider;
    private int targetScore;
    void Start()
    {
        targetScore = 200;
        scoreSlider.maxValue = targetScore;
    }

    // Update is called once per frame
    void Update()
    {
        scoreSlider.value = ScoreManager.Score;
    }
}
