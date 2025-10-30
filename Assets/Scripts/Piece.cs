using System.Collections;
using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Piece : MonoBehaviour
{
    private Vector3 _offset;

    private GridManager _manager;
    private ScoreManager scoreManager;
    private Slot _slot;
    public bool scaleUp = false;
    public bool onTheGrid = false;
    private int tileCount = 0;
    public Vector3 startPosition;
    [SerializeField] private Sprite[] BlocksColor;
    private int chosenColorIndex;
    [SerializeField] private GameOverManager gameOverManager;

    private bool Mod;
    private void Start()
    {
        
        _manager = GetComponent<GridManager>();
        
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();

        gameOverManager = GameObject.FindWithTag("GameOverManager").GetComponent<GameOverManager>();
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        startPosition = transform.position;
        chosenColorIndex = Random.Range(0, BlocksColor.Length);
        SetRandomColors();

    }

    public void OnSetToGrid()
    {
        _slot.Release();
    }

    public void OnSpawn(Slot slot)
    {
        _slot = slot;
    }

    private void SetActiveOutline(bool enable)
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            myTile.SetActiveOutline(enable);
        }
    }

    private bool AllowSetToGrid()
    {
        var allowSetToGrid = true;
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;

            var myTile = (MyTile)tile;
            var hit = myTile.Movable.Hit();
            if (!hit)
            {
                allowSetToGrid = false;
                break;
            }

            //OnMyTile
            var baseTile = hit.transform.GetComponent<MyTile>();
            if (baseTile.OnMyTile)
            {
                allowSetToGrid = false;
                break;
            }
        }
        FindObjectOfType<AudioManager>().Play("DropDown");
        return allowSetToGrid;
    }

    private void SetPositionAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            tileCount++;
            myTile.Movable.SetPositionToHit();


        }
    }

    private void SetRandomColors()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;

            tile.gameObject.GetComponent<SpriteRenderer>().sprite = BlocksColor[chosenColorIndex];

        }
    }

    private void BackHomeAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            SetActiveOutline(false);
            myTile.Movable.BackHome();

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        DecreaseAllColliderSize();
        SetActiveOutline(true);
        SetOffset(eventData);
        transform.position += new Vector3(0, 3f, 0); // Bu değeri ihtiyacınıza göre ayarlayın

        //transform.position += Vector3.up * 1.0f; // Adjust this value to control how much above the piece appears

    }

    public void DecreaseAllColliderSize()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;

            var myTile = (MyTile)tile;
            myTile.Movable.DecreaseColliderSize();
        }

    }

    public void IncreaseAllColliderSize()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;

            var myTile = (MyTile)tile;
            myTile.Movable.IncreaseColliderSize();
        }

    }

    private void SetOffset(PointerEventData eventData)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, screenPoint.z);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        _offset = transform.position - worldPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Move(eventData);
    }

    private void Move(PointerEventData eventData)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, screenPoint.z);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = Vector3.Lerp(transform.position, worldPoint, 10.1f);

    }

    public void OnPointerUp()
    {
        SetActiveOutline(false);

        var allowSetToGrid = AllowSetToGrid();

        if (allowSetToGrid)
        {

            onTheGrid = true;
            SetPositionAll();
            scoreManager.ScoreIncreaseWithTileCount(tileCount);
            ScoreManager.MoveCount++;
            OnSetToGrid();
            Spawner.Instance.Check();

            BaseGrid.Instance.CheckGrid();
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Respawn").transform;
            Debug.Log("allowSetToGrid");


            StartCoroutine(gameOverManager.CheckGameOver());
        }
        else
        {
            BackHomeAll();
            transform.localPosition = new Vector3(0, 0, 0);
            IncreaseAllColliderSize();


        }
    }
    private void FixedUpdate()
    {
        if (scaleUp)
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else
        {
            if (onTheGrid)
            {
                transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

            }
            else
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            }
        }
    }
}