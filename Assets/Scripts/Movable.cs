using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    [SerializeField] private LayerMask mask;
    private Vector3 firstPosition;

    private void Start()
    {
        firstPosition = transform.position;      
    }
    public void OnDrag(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        target += offset;
        target.z = 0;
        transform.position = target;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var target = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - target;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var hit = Hit();
        if (hit)
        {
            Vector3 newPosition = new Vector3(hit.transform.position.x, hit.transform.position.y, -2f);
            transform.position = newPosition;
        }
        else
        {
            BackToPosition();
        }
    }

    private void BackToPosition()
    {
        transform.position = firstPosition;
    }

    private RaycastHit2D Hit()
    {
        var origin = transform.position;
        origin.z += .5f;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector3.forward, 10, mask);
        return hit2D;

        //if (hit2D)
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.blue);
        //    Debug.Log("Did Hit");
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not hit");

        //}
    }
}
