using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


public class Movable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 _offset;
    [SerializeField] private LayerMask mask;

    public Transform _currentMovable;
    public Vector3 _homePosition;
    public Vector3 _myTileHomePosition;

    private MyTile _myTile;
    private Piece _piece;
    private BoxCollider2D collider2d;

    private void Start()
    {
        _currentMovable = transform.parent;
        _homePosition = transform.position;
        collider2d = GetComponent<BoxCollider2D>();
        _myTile = GetComponent<MyTile>();
        _myTileHomePosition = _myTile.transform.position;
        _piece = transform.parent.GetComponent<Piece>();
    }

    #region Pointer

    public void OnPointerDown(PointerEventData eventData)
    {
        _piece.OnPointerDown(eventData);
        _piece.scaleUp = true;

        SetOffset(eventData); 
    }

    private void SetOffset(PointerEventData eventData)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(_currentMovable.position);
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, screenPoint.z);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        _offset = _currentMovable.position - worldPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {

        Move(eventData);
    }

    private void Move(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y);
        var target = Camera.main.ScreenToWorldPoint(mousePosition);

        target += _offset;
        target.z = 0;
        _currentMovable.position = Vector3.Lerp(_currentMovable.position, target, 3f); // Bu değeri ihtiyacınıza göre ayarlayın (0.1f hız faktörüdür)

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _piece.OnPointerUp();
        _piece.scaleUp = false;

    }
    public void DecreaseColliderSize()
    {
        collider2d.size = new Vector2(1f, 1f);
    }
    public void IncreaseColliderSize()
    {
        collider2d.size = new Vector2(7f, 7f);
    }

    #endregion


    public void SetPositionToHit()
    {
        var hit = Hit();
        var baseTile = hit.transform.GetComponent<MyTile>();
        baseTile.IsOnTile = true;
        _myTile.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        baseTile.OnMyTile = _myTile;
        var target = hit.transform.position;
        target.z = 0.5f;
        _myTile.SetActiveCollider(false);

        //transform.position = target;
        Animation(target, .3f);
    }

    public void BackHome()
    {
        Debug.Log(_homePosition);
        _currentMovable.position = _homePosition;

        //Animation2(_homePosition, .3f);
        //transform.position = _homePosition;
        //_myTile.transform.position = _myTileHomePosition;
    }

    public RaycastHit2D Hit()
    {
        var origin = transform.position;
        return Physics2D.Raycast(origin, Vector3.forward, 10, mask);
    }

    private async void Animation(Vector3 target, float duration)
    {
        var init = transform.position;

        var passed = 0f;
        while (passed < duration)
        {
            passed += Time.deltaTime;
            var normalize = passed / duration;
            var current = Vector3.Lerp(init, target, normalize);
            transform.position = current;
            await Task.Yield();
        }
    }


}