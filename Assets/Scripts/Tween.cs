using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{

    [SerializeField] private Vector3 scale;
    [SerializeField] private GameObject gameObjectClassic, gameObjectAdventure, gameObjectBlockImage;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObjectClassic, scale, 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeShake);

        LeanTween.scale(gameObjectAdventure, scale, 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeShake);

        LeanTween.scale(gameObjectBlockImage, scale, 1f)
            .setDelay(0.5f)
            .setEase(LeanTweenType.easeShake);

     


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
