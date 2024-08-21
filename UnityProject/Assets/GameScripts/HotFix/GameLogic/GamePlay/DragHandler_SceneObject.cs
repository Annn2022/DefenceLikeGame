using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameLogic.GamePlay
{
    public class DragHandler_SceneObject : MonoBehaviour
    {

        public OnDragEvent _onDrag;
        public OnDragEvent _onBeginDrag;
        public OnDragEvent _onEndDrag;

        protected bool isDragging = false;
        
        private Vector3   startPosition;
        private Transform originalParent;

        public OnDragEvent onDrag
        {
            get
            {
                if (_onDrag == null)
                {
                    _onDrag = new OnDragEvent();
                }

                return _onDrag;
            }
        }

        public OnDragEvent onBeginDrag
        {
            get
            {
                if (_onBeginDrag == null)
                {
                    _onBeginDrag = new OnDragEvent();
                }

                return _onBeginDrag;
            }
        }

        public OnDragEvent onEndDrag
        {
            get
            {
                if (_onEndDrag == null)
                {
                    _onEndDrag = new OnDragEvent();
                }

                return _onEndDrag;
            }
        }
        
        private void OnEnable()
        {
            onDrag.AddListener(OnDrag);
            onBeginDrag.AddListener(OnBeginDrag);
            onEndDrag.AddListener(OnEndDrag);
            gameObject.layer = LayerMask.NameToLayer("Dragable");
        }
        
        protected virtual void OnBeginDrag(Vector3 position, PointerData pointerData)
        {
            isDragging = true;
            startPosition = transform.position;
            originalParent = transform.parent;
            if (transform.TryGetComponent<RectTransform>(out var _rect))
            {
                transform.SetParent(transform.root.Find("UICanvas"));
            }
            else
            {
                if (transform.TryGetComponent(out Collider2D col))
                {
                    col.enabled = false;
                }
                transform.SetParent(transform.root);
            }
        }
        
        protected virtual void OnDrag(Vector3 position, PointerData pointerData)
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
        
        protected virtual void OnEndDrag(Vector3 position, PointerData pointerData)
        {
            isDragging = false;
            
            GameObject destination = pointerData.target;
            if (transform.TryGetComponent(out Collider2D col))
            {
                col.enabled = true;
            }
            
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
        }



    }
}