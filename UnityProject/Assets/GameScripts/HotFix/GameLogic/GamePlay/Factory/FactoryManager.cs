using System;
using System.Collections.Generic;
using GamePlay.framework;
using UnityEngine;

namespace GameLogic.GamePlay.Factory
{
    public class FactoryManager : ManagerBehaviour<FactoryManager>
    {
        private static Dictionary<Type, IFactory> factories = new Dictionary<Type, IFactory>();

        protected void Start()
        {
            Register<AnimalFactory>(new AnimalFactory());
            Register<BulletFactory>(new BulletFactory());
            Register<MonsterFactory>(new MonsterFactory());
            Register<EffectFactory>(new EffectFactory());
        }
        
        public void Register<T>(IFactory factory)
        {
            if (factories.ContainsKey(typeof(T)))
            {
                Debug.LogError("KEY,重复");
                return;
            }
            factories.Add(typeof(T), factory);
        }
        
        public T Get<T>() where T : class, IFactory
        {
            var type = typeof(T);
            if (factories.ContainsKey(type))
            {
                return factories[type] as T;
            }
            return null;
        }
    }
}