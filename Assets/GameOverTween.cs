using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTween : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectClassic;
    [SerializeField] private Vector3 scale;


    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObjectClassic, scale, 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeShake);
    }
}
