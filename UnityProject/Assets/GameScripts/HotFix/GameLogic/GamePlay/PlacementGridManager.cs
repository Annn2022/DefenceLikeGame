using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameBase;
using GamePlay.framework;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TEngine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameLogic.GamePlay
{
    public class PlacementGridManager : ManagerBehaviour<PlacementGridManager>
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<int,List<PlacementGrid>> ListPlacementGrid_Dic = new Dictionary<int, List<PlacementGrid>>();

        private List<PlacementGrid> ListPlacementGrid_Empty = new List<PlacementGrid>();
        private List<Attacker>      AliveAttacker           = new List<Attacker>();

        private string[] attackerPool = new string[] { "Attacker_01", "Attacker_02", "Attacker_03", "Attacker_04", "Attacker_05" };
      
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
        public async UniTask RandomInit()
        {
            attackerCurPool.Add(attackerPool[0]);
            int indexAttackerId = Random.Range(0, attackerCurPool.Count);
            int index = Random.Range(0, ListPlacementGrid_Empty.Count);
            var attacker=  await GameModule.Resource.LoadGameObjectAsync(attackerCurPool[indexAttackerId]);
            var attackerComponet = attacker.GetComponent<Attacker>();

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
        
        public async UniTask RondomNewAttacker()
        {
            UpdateMonsterPool();
            if (ListPlacementGrid_Empty.IsNullOrEmpty())
            {
                return;
            }
            int indexAttackerId = Random.Range(0, attackerCurPool.Count);
            var attacker=  await GameModule.Resource.LoadGameObjectAsync(attackerCurPool[indexAttackerId]);
            var attackerComponet = attacker.GetComponent<Attacker>();
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
        public void CreateNewAttacker(uint id, PlacementGrid grid)
        {
            ClearGrid(grid);
            var attacker=  GameModule.Resource.LoadGameObject(attackerPool[id-1]);
            var attackerComponet = attacker.GetComponent<Attacker>();
            grid.DoPutDown(attackerComponet);
            AddAttacker(attackerComponet);
        }


        public void AddAttacker(Attacker attacker)
        {
            if (!AliveAttacker.Contains(attacker))
            {
                AliveAttacker.Add(attacker);

            }
        }
        
        public void RemoveAttacker(Attacker attacker)
        {
            ClearGrid(attacker.m_PlacementGrid);
            
            if (AliveAttacker.Contains(attacker))
            {
                AliveAttacker.Remove(attacker);
                Destroy(attacker.gameObject);
            }
        }

        public void ClearGrid(PlacementGrid grid)
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
        public void PlaceGrid(PlacementGrid grid,Attacker attacker)
        {
            grid.SetAttacker(attacker);
            ListPlacementGrid_Empty.Remove(grid);
            print("放置攻击者 当前格子空余数： "+ListPlacementGrid_Empty.Count);
        }

        public void NextTurn()
        {
            foreach (var attacker in AliveAttacker)
            {
                attacker.NextTurn();
            }

            RondomNewAttacker().Forget();
        }

      
    }
}