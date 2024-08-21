using System;
using System.Collections.Generic;
using GameBase;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameLogic.GamePlay
{
    //管理怪物的生成和回收
    public class MonsterManager : ManagerBehaviour<MonsterManager>
    {
        /// <summary>
        /// 怪物的强度随时间变化 后续都以25秒为一个阶段
        /// </summary>
        private int[] MonsterStateWithTime = new int[3] { 10, 20,35};
       
        private int curTurn = 0;
        /// <summary>
        /// 怪物数量升级的轮数
        /// </summary>
        private int[] MonsterCountUpTurn = new int[3] { 3, 8, 13};
        
        /// <summary>
        /// 当前同时生成的怪物数量
        /// </summary>
        private int curMonsterCount = 1;

        /// <summary>
        /// 不同阶段下生怪的等级区间
        /// </summary>
        private Vector2[] MonsterLevels = new Vector2[12]
        {
            new Vector2(1, 1), 
            new Vector2(1, 2), 
            new Vector2(2, 3), 
            new Vector2(2, 4),
            new Vector2(3, 5),
            new Vector2(3, 6),
            new Vector2(4, 7),
            new Vector2(4, 8),
            new Vector2(5, 9),
            new Vector2(5, 10),
            new Vector2(6, 11),
            new Vector2(6, 12),
        };

        private Vector2 monsterlevel = Vector3.one;
        
        /// <summary>
        /// 怪物等级进阶的轮数
        /// </summary>
        private int[] MonsterLevelsUpTurn = new int[12] {2, 5,8,11,14,17,20,23,26,29,32,35};
        
        [SerializeField]
        private List<Transform> m_MonsterSpawnPoints = new List<Transform>();
        List<Transform> monsterSpawnPoint = new List<Transform>();
        
        [SerializeField]
        private List<Transform> m_MonsterSpawnPoints_Big = new List<Transform>();
        private List<Transform> monsterSpawnPoint_Big = new List<Transform>();

        float lastSpawnTime = 0;
        
        private int[] MonsterspawnIntervalChange = new int[1] {3};
        int spawnInterval = 4;

        private int[]  bigMonsterSpawnTurn = new int[9] {8,11,14,17,20,23,26,29,32};
        private uint[] bigMonsterSpawnId   = new uint[9] {8,9,9,10,10,11,11,12,12};
        private uint   bigId               = 0;
        
        float spawnMoreHpPercent = 0.1f;
        private void Awake()
        {
            monsterParent = new GameObject("Monsters").transform;
            monsterParent.SetParent(transform);
            monsterParent.localPosition = Vector3.zero;
            monsterParent.localRotation = Quaternion.identity;
            monsterParent.localScale = Vector3.one;
        }
        private Transform monsterParent;
        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.InGame)
                return;
            bool isNewTurn = false;
            
            if (GameManager.Instance.GamingTime < MonsterStateWithTime[0])
            {
                if (curTurn != 1)
                {
                    isNewTurn = true;
                    curTurn = 1;

                }
            }
            else if (GameManager.Instance.GamingTime < MonsterStateWithTime[1])
            {
                if (curTurn != 2)
                {
                    isNewTurn = true;
                    curTurn = 2;

                }
            }
            else
            {
                if (curTurn != ((int)GameManager.Instance.GamingTime - MonsterStateWithTime[1]) / 15 + 2)
                {
                    isNewTurn = true;
                    curTurn = ((int)GameManager.Instance.GamingTime - MonsterStateWithTime[1]) / 15 + 2;
                }
            }

            //怪物同时出现的数量
            curMonsterCount = 1;
            for (int i = 0; i < MonsterCountUpTurn.Length; i++)
            {
                if (curTurn >= MonsterCountUpTurn[i])
                {
                    curMonsterCount++;
                }
            }

            if (MonsterLevelsUpTurn.Length != MonsterLevels.Length)
            {
               Debug.LogError("MonsterLevelsUpTurn.Length != MonsterLevels.Length 数据不匹配"); 
               return;
            }
            for (int i = MonsterLevelsUpTurn.Length-1; i >=0; i--)
            {
                if (curTurn >=  MonsterLevelsUpTurn[i])
                {
                    monsterlevel = MonsterLevels[i];
                    break;
                }
            }
            
            //怪物生成间隔计算
            spawnInterval = 4;
            for (int i = MonsterspawnIntervalChange.Length-1; i >=0; i--)
            {
                if (curTurn >=  MonsterLevelsUpTurn[i])
                {
                    spawnInterval--;
                }
            }
            
            //是否为打怪生成回合
            if (isNewTurn)
            {
                for (int i = bigMonsterSpawnTurn.Length -1; i >=0; i--)
                {
                    if (curTurn == bigMonsterSpawnTurn[i])
                    {
                        bigId = bigMonsterSpawnId[i];
                    }
                }
            }
            

            if (GameManager.Instance.GamingTime -lastSpawnTime >= spawnInterval)
            {
                SpawnMonster();
                lastSpawnTime = GameManager.Instance.GamingTime;
            }
        }


        private void SpawnMonster()
        {
            monsterSpawnPoint.Clear();

            if (bigId > 0)
            {
                var index = UnityEngine.Random.Range(0, m_MonsterSpawnPoints_Big.Count);
                GameObject monster =  ManagerLocator.Get<FactoryManager>().Get<MonsterFactory>().CreateBigMonster(bigId);
                monster.transform.position = m_MonsterSpawnPoints_Big[index].position;
                monster.transform.SetParent(monsterParent,false);
                bigId = 0;
            }
            //var count = UnityEngine.Random.Range(1, curMonsterCount+1);
            var count = curMonsterCount;
            if (count >m_MonsterSpawnPoints.Count)
            {
                Debug.LogError("怪物随机数量过大");
            }
            for (int i = 0; i < count; i++)
            {
                int spawnPointIndex = UnityEngine.Random.Range(0, m_MonsterSpawnPoints.Count);
                if (!monsterSpawnPoint.Contains(m_MonsterSpawnPoints[spawnPointIndex]))
                {
                    monsterSpawnPoint.Add(m_MonsterSpawnPoints[spawnPointIndex]);
                }
                else
                {
                    i--;
                }
            }

            for (int i = 0; i < monsterSpawnPoint.Count; i++)
            {
                //有概率出现高等级怪物
                uint isMore = (uint)(Random.Range(0f,1f) < spawnMoreHpPercent?2:0);
                GameObject monster = ManagerLocator.Get<FactoryManager>().Get<MonsterFactory>().CreateNewMonster((uint)UnityEngine.Random.Range((int)monsterlevel.x, (int)monsterlevel.y+1)+isMore+(uint)GameManager.Instance.technologyLevel);
                monster.transform.position = monsterSpawnPoint[i].position;
                monster.transform.SetParent(monsterParent,false);
            }
        }
        
    }
}