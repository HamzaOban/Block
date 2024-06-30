using MyGrid.Code;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private GridManager _manager;
    private Slot _slot;

    private void Start()
    {
        _manager = GetComponent<GridManager>();
        transform.localScale = new Vector3(1, 1, 1);
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

        return allowSetToGrid;
    }

    private void SetPositionAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;

            myTile.Movable.SetPositionToHit();


        }
    }

    private void BackHomeAll()
    {
        foreach (var tile in _manager.Tiles)
        {
            if (!tile.gameObject.activeSelf) continue;
            var myTile = (MyTile)tile;
            myTile.Movable.BackHome();
            SetActiveOutline(false);
        }
    }

    public void OnPointerDown()
    {
        SetActiveOutline(true);
    }

    public void OnPointerUp()
    {
        SetActiveOutline(false);
        transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);


        var allowSetToGrid = AllowSetToGrid();

        if (allowSetToGrid)
        {
            SetPositionAll();

            OnSetToGrid();
            Spawner.Instance.Check();

            BaseGrid.Instance.CheckGrid();
        }
        else
        {
            BackHomeAll();

        }
    }
}