using System.Collections;
using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;
using TMPro;

public class BaseGrid : Singleton<BaseGrid>
{
    [SerializeField] private GridManager _manager;
    [SerializeField] private TextMeshProUGUI comboText;
    public float targetScale = 1.2f;
    public float duration = 0.5f;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();

    }

    public void CheckGrid()
    {
        CheckAndDestroyRow();


        CheckAndDestroyColumn();
    }

    IEnumerator ScaleTextOverTime(float targetScale, float duration)
    {
        if(ScoreManager.Combo <= 1)
        {
            yield break;
        }
        if (!SceneManagerForMod.Mod)
        {
            yield break;
        }
        comboText.gameObject.SetActive(true);

        comboText.text = "ComboX" + ScoreManager.Combo;
        RectTransform rectTransform = comboText.GetComponent<RectTransform>();
        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScaleVector = new Vector3(targetScale, targetScale, targetScale);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScaleVector, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScaleVector;
        yield return new WaitForSeconds(1f);

        comboText.gameObject.SetActive(false);
        rectTransform.localScale = originalScale;
    }

    private void CheckAndDestroyColumn()
    {
        List<int> willDestroyColumnIndex = new List<int>();
        for (int i = 0; i < 8; i++)
        {
            if (IsFullColumn(i))
            {
                if (ScoreManager.MoveCount < 4)
                {
                    ScoreManager.Combo++;
                    StartCoroutine(ScaleTextOverTime(targetScale, duration));
                    scoreManager.ScoreIncreaseWithDestroyColumnOrRow();
                }
                else
                {
                    ScoreManager.Combo = 0;
                    scoreManager.ScoreIncreaseWithDestroyColumnOrRow();
                }
                Debug.Log($"Is Full Column {i}");
                willDestroyColumnIndex.Add(i);
            }
        }

        foreach (var column in willDestroyColumnIndex)
        {
            for (int y = 0; y < 8; y++)
            {
                var tile = (MyTile)_manager.GetTile(new Vector2Int(column, y));
                if (tile.OnMyTile)
                {
                    if (ScoreManager.Combo < 3)
                    {
                        FindObjectOfType<AudioManager>().sounds[0].volume = 0.3f;

                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().sounds[0].volume = ScoreManager.Combo * 0.12f;

                    }
                    FindObjectOfType<AudioManager>().Play("BlockBlast");

                    tile.OnMyTile.Destroy(y * .02f);
                    tile.IsOnTile = false;
                }
            }
        }
    }

    private void CheckAndDestroyRow()
    {
        List<int> willDestroyRowIndex = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            if (IsFullRow(i))
            {
                if (ScoreManager.MoveCount < 4)
                {
                    ScoreManager.Combo++;
                    StartCoroutine(ScaleTextOverTime(targetScale, duration));

                    scoreManager.ScoreIncreaseWithDestroyColumnOrRow();
                    Debug.Log("Destroy " + ScoreManager.MoveCount + " MoveCount" + ScoreManager.Score + " Score");

                }
                else
                {
                    ScoreManager.Combo = 0;
                    scoreManager.ScoreIncreaseWithDestroyColumnOrRow();
                }
                Debug.Log($"Is Full Row {i}");
                willDestroyRowIndex.Add(i);
            }
        }

        foreach (var rowIndex in willDestroyRowIndex)
        {
            for (int x = 0; x < 8; x++)
            {
                var tile = (MyTile)_manager.GetTile(new Vector2Int(x, rowIndex));
                if (tile.OnMyTile)
                {
                    if(ScoreManager.Combo < 3)
                    {
                        FindObjectOfType<AudioManager>().sounds[0].volume = 0.3f;

                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().sounds[0].volume = ScoreManager.Combo * 0.12f;

                    }
                    FindObjectOfType<AudioManager>().Play("BlockBlast");

                    tile.OnMyTile.Destroy(x * .02f);

                    tile.IsOnTile = false;

                }


            }
        }
    }


    private bool IsFullRow(int row)
    {
        for (int i = 0; i < 8; i++)
        {
            var tile = (MyTile)_manager.GetTile(new Vector2Int(i, row));
            if (!tile.OnMyTile) return false;
        }

        return true;
    }

    private bool IsFullColumn(int column)
    {
        for (int i = 0; i < 8; i++)
        {
            var tile = (MyTile)_manager.GetTile(new Vector2Int(column, i));
            if (!tile.OnMyTile) return false;
        }

        return true;
    }
}