using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.framework
{
    public static class ManagerLocator
    {

        private static Dictionary<Type, IGameService> s_services;
        private static Dictionary<Type, IGameService> Services
        {
            get
            {
                return s_services ??= new Dictionary<Type, IGameService>();
            }
        }

        public static void Register<T>(IGameService service)
        {
            if (Services.ContainsKey(typeof(T)))
            {
                Debug.LogError("KEY,重复");
                return;
            }
            Services.Add(typeof(T), service);
        }

        public static void Unregister<T>(IGameService service)
        {
            var type = service.GetType();
            if (Services.ContainsKey(type) && Services[type] == service)
            {
                Services.Remove(type);
            }
        }

        public static T Get<T>() where T : class, IGameService
        {
            var type = typeof(T);
            if (Services.ContainsKey(type))
            {
                return Services[type] as T;
            }
            return null;
        }

        public static IGameService Get(Type type)
        {
            if (Services.ContainsKey(type))
            {
                return Services[type];
            }
            return null;
        }
    }
}

