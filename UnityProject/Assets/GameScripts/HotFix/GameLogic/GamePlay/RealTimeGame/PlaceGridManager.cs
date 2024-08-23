using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class PlaceGridManager :  ManagerBehaviour<PlaceGridManager>
    {
         [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<int,List<PlaceGrid>> ListPlacementGrid_Dic = new Dictionary<int, List<PlaceGrid>>();

        private List<PlaceGrid> ListPlacementGrid_Empty = new List<PlaceGrid>();
        private List<Animal_Attacker>  AliveAttacker           = new List<Animal_Attacker>();

        private string[] attackerPool = new string[] { "Animal_Sheep","Animal_Pig", "Animal_Cow", "Animal_Chicken"};
      
        //怪物池更新回合数
        private int[] attackerUpdateTurn = new[] { 1, 6, 11, 16, 26 };
        
        private List<string> attackerCurPool = new List<string>();

        protected override void Awake()
        {
            base.Awake();
            ListPlacementGrid_Empty.Clear();
            foreach (var list in ListPlacementGrid_Dic)
            {
                ListPlacementGrid_Empty.AddRange(list.Value);
                list.Value.ForEach(x => x.RowId = list.Key);
            }
        }
        
        /// <summary>
        /// 开局随机生成Attacker
        /// </summary>
        public void RandomInit()
        {
            attackerCurPool.Add(attackerPool[0]);
            int index = Random.Range(0, ListPlacementGrid_Empty.Count);
            var attacker=  ManagerLocator.Get<FactoryManager>().Get<AnimalFactory>().CreateAnimalAsType(0,1);
            var attackerComponet = attacker.GetComponent<Animal_Attacker>();

            ListPlacementGrid_Empty[index].DoPutDown(attackerComponet);
            AddAttacker(attackerComponet);
        }  
        
        public void UpdateMonsterPool()
        {
            for (int i = 0; i < attackerUpdateTurn.Length; i++)
            {
                if (attackerUpdateTurn[i]<= GameManager.Instance.MainTurnNum)
                {
                    if (!attackerCurPool.Contains(attackerPool[i]))
                    {
                        attackerCurPool.Add(attackerPool[i]);
                    }
                }
            }
        }
        
        public void RondomNewAttacker(uint id)
        {
           // UpdateMonsterPool();
            if (ListPlacementGrid_Empty.IsNullOrEmpty())
            {
                return;
            }
            var attacker =  ManagerLocator.Get<FactoryManager>().Get<AnimalFactory>().CreateAnimalRandom(id);
            var attackerComponet = attacker.GetComponent<Animal_Attacker>();
            int indexGrid = Random.Range(0, ListPlacementGrid_Empty.Count);
            if (ListPlacementGrid_Empty[indexGrid].m_Attacker!=null)
            {
                Debug.LogError("该格子已有攻击者: "+ListPlacementGrid_Empty[indexGrid].gameObject.name);
                return;
            }
            ListPlacementGrid_Empty[indexGrid].DoPutDown(attackerComponet);
            AddAttacker(attackerComponet);
        }
        
        /// <summary>
        /// 创建一个新的攻击者
        /// </summary>
        public void CreateNewAttacker(uint newId, PlaceGrid grid, int animalType)
        {
            ClearGrid(grid);
            var attacker=  ManagerLocator.Get<FactoryManager>().Get<AnimalFactory>().CreateAnimalAsType(animalType,newId);
            var attackerComponet = attacker.GetComponent<Animal_Attacker>();
            grid.DoPutDown(attackerComponet);
            AddAttacker(attackerComponet);
        }


        public void AddAttacker(Animal_Attacker attacker)
        {
            if (!AliveAttacker.Contains(attacker))
            {
                AliveAttacker.Add(attacker);

            }
        }
        
        public void RemoveAttacker(Animal_Attacker attacker)
        {
            ClearGrid(attacker.m_PlacementGrid);
            
            if (AliveAttacker.Contains(attacker))
            {
                AliveAttacker.Remove(attacker);
                Destroy(attacker.gameObject);
            }
        }

        public void ClearGrid(PlaceGrid grid)
        {
            grid.SetAttacker(null);
            if (!ListPlacementGrid_Empty.Contains(grid))
            {
                ListPlacementGrid_Empty.Add(grid);

            }
        }
        
        /// <summary>
        /// 放置攻击者
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="attacker"></param>
        public void PlaceGrid(PlaceGrid grid,Animal_Attacker attacker)
        {
            grid.SetAttacker(attacker);
            if (ListPlacementGrid_Empty.Contains(grid))
            {
                ListPlacementGrid_Empty.Remove(grid);

            }

            print("放置攻击者 当前格子空余数： "+ListPlacementGrid_Empty.Count);
        }
        
    }
}