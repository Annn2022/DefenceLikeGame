using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.framework
{
    public interface IGameService
    {
    }

    public class ManagerBehaviour<T> : SerializedMonoBehaviour, IGameService
    {
        protected virtual void Awake()
        {
            ManagerLocator.Register<T>(this);
        }

        protected virtual void OnDestroy()
        {
            ManagerLocator.Unregister<T>(this);
        }
    }

}

