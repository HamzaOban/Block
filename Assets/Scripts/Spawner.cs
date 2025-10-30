using System.Collections.Generic;
using MyGrid.Code;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private GameObject GameOverPanel;

    [SerializeField] private List<Piece> items;
    [SerializeField] private List<Piece> reviveItems;

    [SerializeField] private List<Slot> slots;
    [SerializeField] private GameOverManager gameOverManager;
    static int gameOverSlotCount = 0;
    

    private void Start()
    {
        Check();
        ReviveItems();

    }
    private void Update()
    {
        if (GameOverManager.gameOver)
        {
            foreach (var slot in slots)
            {
                foreach (var collider in slot.GetComponentsInChildren<BoxCollider2D>())
                {
                    collider.enabled = false;
                }
            }
        }
        else
        {
            foreach (var slot in slots)
            {
                foreach (var collider in slot.GetComponentsInChildren<BoxCollider2D>())
                {
                    collider.enabled = true;
                }
            }
        }
    }



    public void Check()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty()) continue;

            var index = Random.Range(0, items.Count);
            var item = items[index];
            var piece = Instantiate(item, slot.transform);
            piece.transform.localPosition = Vector3.zero;
            piece.OnSpawn(slot);
            slot.SetPiece(piece);
            gameOverManager.numbers.Clear();
        }
    }

    public void ReviveItems()
    {
        foreach (var slot in slots)
        {
            if (Revive.reviveBool)
            {
                Destroy(slot.transform.GetChild(0).gameObject);

                var index = Random.Range(0, reviveItems.Count);
                var item = reviveItems[index];
                var piece = Instantiate(item, slot.transform);
                piece.transform.localPosition = Vector3.zero;
                piece.OnSpawn(slot);
                slot.SetPiece(piece);
                gameOverManager.numbers.Clear();
                GameOverManager.gameOver = false;
            }
        }

    }

}