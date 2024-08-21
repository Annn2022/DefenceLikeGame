using System.Collections.Generic;
using System.Linq;
using GameBase;
using GamePlay.framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class EnemyGridManager:ManagerBehaviour<EnemyGridManager>
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        public Dictionary<int,List<EnemyGrid>> EnemyGrid_Dic = new Dictionary<int, List<EnemyGrid>>();
        
        private string[] monsterPool = new string[]{"Monster_01","Monster_02","Monster_03","Monster_04","Monster_05"};
        //怪物池更新回合数
        private int[] monserPoolUpdateTurn = new[] { 1, 6, 11, 16, 26 };
        
        //当前的怪物池
        private List<string> monsterCurPool = new List<string>();
        public void NextTurn()
        {
            UpdateMonsterPool();
            if (GameManager.Instance.MainTurnNum % 2 == 0)
            {
                MoveTheBullets();
            }
            else
            {
                MoveTheMonster();
                //随机生成怪物
                int rowId = Random.Range(1, EnemyGrid_Dic.Count+1);
                string monsterName = monsterCurPool[Random.Range(0, monsterCurPool.Count)];
                EnemyGrid_Dic[rowId].First().SpawnMonster(monsterName);
            }


        }

        /// <summary>
        /// 移动场上的所有子弹
        /// </summary>
        private void MoveTheBullets()
        {
            foreach (var gridList in EnemyGrid_Dic)
            {
                var list = gridList.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ListBullet.Count > 0 && i >0)
                    {
                        List<Bullet> temp = list[i].ListBullet;
                        for (int j = temp.Count -1; j >= 0; j--)
                        {
                            list[i - 1].MoveBullet(temp[j]);

                        }
                    }
                    
                    else if (i <= 0)
                    {
                        //在最后则直接销毁
                        list[i].RemoveAllBulletInstence();

                    }
                }
            }
            
            
        }
        
        /// <summary>
        /// 移动场上的所有怪物
        /// </summary>
        private void MoveTheMonster()
        {
            foreach (var gridList in EnemyGrid_Dic)
            {
                var list = gridList.Value;
                for (int i = list.Count-1; i >= 0; i--)
                {
                    if (list[i].m_Monster != null  && i < list.Count - 1)
                    {
                        list[i + 1].MoveMonster(list[i].m_Monster);
                    }
                    
                    else if (i>= list.Count - 1)
                    {
                        //游戏失败
                        

                    }
                }
            }
        }

        public void UpdateMonsterPool()
        {
            for (int i = 0; i < monserPoolUpdateTurn.Length; i++)
            {
                if (monserPoolUpdateTurn[i]<= GameManager.Instance.MainTurnNum)
                {
                    if (!monsterCurPool.Contains(monsterPool[i]))
                    {
                        monsterCurPool.Add(monsterPool[i]);
                    }
                }
            }
        }
    }
  
}