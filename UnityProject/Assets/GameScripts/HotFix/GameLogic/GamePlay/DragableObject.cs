using System;
using System.Diagnostics;
using TEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace GameLogic.GamePlay
{
    public class DragableObject : MonoBehaviour
    {
        private Vector3   startPosition;
        private Transform originalParent;
        DragHandler dragHandler;

        protected bool isOnDrag => dragHandler?dragHandler.isOnDrag : false;
        private void Awake()
        {
            dragHandler = DragHandler.Get(gameObject);
            dragHandler.onBeginDrag.AddListener(OnBeginDrag);
            dragHandler.onDrag.AddListener(OnDrag);
            dragHandler.onEndDrag.AddListener(OnEndDrag);
        }
        
        protected virtual void OnBeginDrag(Vector3 position, PointerEventData eventData)
        {
            startPosition = transform.position;
            originalParent = transform.parent;
            if (transform.TryGetComponent<RectTransform>(out var _rect))
            {
                transform.SetParent(transform.root.Find("UICanvas"));
            }
            else
            {
                transform.SetParent(transform.root);
            }
            
            GetComponent<RawImage>().raycastTarget = false;
        }

        protected virtual void OnDrag(Vector3 position,PointerEventData eventData)
        {
            if (transform.TryGetComponent<RectTransform>(out var _rect))
            {
                position = GameManager.Instance.m_Camera.WorldToScreenPoint(position);
                Vector2 uipos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.root.Find("UICanvas").transform as RectTransform, position,GameManager.Instance.m_UICamera, out uipos);
                _rect.anchoredPosition = uipos;
            }
            else
            {
                transform.position = position;

            }
        }

        protected virtual void OnEndDrag(Vector3 position, PointerEventData eventData)
        {
            GameObject destination = eventData.pointerCurrentRaycast.gameObject;
            if (destination != null && destination.CompareTag("GridCell"))
            {
                var parent = destination.transform.Find("SpawnPoint");
                if (parent == null)
                {
                    parent = destination.transform;
                }
                transform.SetParent(parent);
                transform.position = parent.position;
            }
            else
            {
                // Return to original position if not dropped on a grid cell
                transform.position = startPosition;
                transform.SetParent(originalParent);
            }
            
            GetComponent<RawImage>().raycastTarget = true;
        }
    }
}