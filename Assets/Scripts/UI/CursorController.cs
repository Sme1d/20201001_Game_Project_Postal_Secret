using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    // Variables
    public List<Sprite> cursorSprites;

    private readonly PointerEventData    _pointerData   = new PointerEventData(EventSystem.current);
    private readonly List<RaycastResult> _results       = new List<RaycastResult>();
    private          string              _currentCursor = Constants.CursorTags[0];

    private void Awake()
    {
        Cursor.visible = false;
        UpdateCursor(_currentCursor);
    }

    private void FixedUpdate()
    {
        transform.position = Input.mousePosition;

        var tagLabel = Constants.CursorTags[0];

        if (EventSystem.current.IsPointerOverGameObject())
        {
            _pointerData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_pointerData, _results);

            if (_results.Count > 0) tagLabel = _results[0].gameObject.tag;
        }
        else
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit)) tagLabel = hit.collider.gameObject.tag;
        }

        UpdateCursor(tagLabel);
    }

    private void UpdateCursor(string newCursor)
    {
        if (_currentCursor == newCursor) return;

        _currentCursor               = newCursor;
        GetComponent<Image>().sprite = cursorSprites[Constants.CursorTags.IndexOf(_currentCursor)];
        var pivot = cursorSprites[Constants.CursorTags.IndexOf(_currentCursor)].pivot;
        pivot.x                                       /= cursorSprites[Constants.CursorTags.IndexOf(_currentCursor)].rect.width;
        pivot.y                                       /= cursorSprites[Constants.CursorTags.IndexOf(_currentCursor)].rect.height;
        transform.GetComponent<RectTransform>().pivot =  pivot;
    }
}