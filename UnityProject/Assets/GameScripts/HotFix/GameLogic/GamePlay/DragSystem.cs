using System;
using GameBase;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic.GamePlay
{
    public class DragSystem : SingletonBehaviour<DragSystem>
    {
        
        private bool    isDragging = false;
        private Camera  mainCamera;
        private Vector3 offset;
        private DragHandler_SceneObject handler;

        private PointerData pointerData;
        // 目前只考虑点击的第一个
        RaycastHit2D[] hits = new RaycastHit2D[1];
          void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
#if UNITY_EDITOR
            // 检测鼠标按下事件
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                int num = Physics2D.RaycastNonAlloc(pos, Vector3.forward, hits, 1000, LayerMask.GetMask("Dragable"));
                if (num > 0)
                {
                    if (hits[0].transform.TryGetComponent<DragHandler_SceneObject>(out handler))
                    {
                        var target = handler.gameObject;
                        isDragging = true;
                        offset = handler.transform.position - GetMouseWorldPos();
                        pointerData = new PointerData
                        {
                            target = target,
                            offset = offset,
                        };
                        handler.onBeginDrag.Invoke(hits[0].point, pointerData);
                    }
                }
            }

            // 当鼠标抬起时停止拖动
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                if (handler == null)
                {
                    return;
                }
                Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                int num = Physics2D.RaycastNonAlloc(pos, Vector3.forward, hits, 1000, LayerMask.GetMask("Dragable"));
                if (num > 0)
                {
                    var target = hits[0].transform.gameObject;
                    offset = handler.transform.position - GetMouseWorldPos();
                    pointerData = new PointerData
                    {
                        target = target,
                        offset = offset,
                    };
                    handler.onEndDrag.Invoke(hits[0].point, pointerData);

                }
                else
                {
                    pointerData = new PointerData
                    {
                        target = null,
                        offset = Vector3.zero,
                    };
                    handler.onEndDrag.Invoke(Vector3.zero, pointerData);

                }

                handler = null;
            }

            // 如果正在拖动，更新物体位置
            if (isDragging)
            {
                transform.position = GetMouseWorldPos();
                handler?.onDrag.Invoke(GetMouseWorldPos(), pointerData);

            }
#endif

#if PLATFORM_WEBGL || PLATFORM_IOS|| PLATFORM_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = mainCamera.ScreenToWorldPoint(touch.position);
                RaycastHit hit;
                isDragging = true;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        int num = Physics2D.RaycastNonAlloc(pos,Vector3.forward, hits, 1000, LayerMask.GetMask("Dragable"));
                        if (num > 0)
                        {
                            if (hits[0].transform.TryGetComponent<DragHandler_SceneObject>(out handler))
                            {
                                var target = handler.gameObject;
                                isDragging = true;
                                offset = handler.transform.position - GetTouchWorldPos(touch.position);
                                pointerData = new PointerData
                                {
                                    target = target,
                                    offset = offset,
                                };
                                handler.onBeginDrag.Invoke(hits[0].point, pointerData);
                            }
                        }

                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            transform.position = GetTouchWorldPos(touch.position);
                            handler.onDrag.Invoke(GetTouchWorldPos(touch.position), pointerData);
                        }

                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isDragging = false;
                        handler.onEndDrag.Invoke(GetMouseWorldPos(), pointerData);

                        break;
                }

            }
#endif
        }
        
    
        // 获取鼠标在世界空间中的位置
        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z; // 获取物体的z坐标
            return mainCamera.ScreenToWorldPoint(mousePoint);
        }

        private Vector3 GetTouchWorldPos(Vector2 touchPosition)
        {
            Vector3 touchPoint = new Vector3(touchPosition.x, touchPosition.y,
                mainCamera.WorldToScreenPoint(transform.position).z);
            return mainCamera.ScreenToWorldPoint(touchPoint);
        }
        
    }
    

    public class OnDragEvent : UnityEvent<Vector3, PointerData>
    {

    }

    public class PointerData
    {
      public GameObject target;
      public Vector3    offset;
    }
    
}