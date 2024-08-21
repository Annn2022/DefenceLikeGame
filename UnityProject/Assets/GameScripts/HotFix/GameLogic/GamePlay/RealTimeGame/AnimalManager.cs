using Cysharp.Threading.Tasks;
using GamePlay.framework;
using UnityEngine;

namespace GameLogic.GamePlay
{
    /// <summary>
    /// 生成动物的基本策略
    /// </summary>
    public class AnimalManager:ManagerBehaviour<AnimalManager>
    {
        //动物生成质量升级的回合数
        private int[] SpawnLevelUpTurn = new int[] { 0,10,20,30,40,50,60 };
        
        private Vector2[] SpawnLevels = new Vector2[]
        {
            new Vector2(1,1),
            new Vector2(1,2),
            new Vector2(1,3),
            new Vector2(1,4),
            new Vector2(1,5),
            new Vector2(1,6),
            new Vector2(1,7),
        };
        Vector2 spawnLevel;
        
        
        public void SpawnAniaml()
        {
            if (SpawnLevelUpTurn.Length != SpawnLevels.Length)
            {
                Debug.LogError("动物生成逻辑数据不匹配");
                return;
            }

            for (int i = SpawnLevelUpTurn.Length -1; i >= 0; i--)
            {
                if (GameManager.Instance.MainTurnNum >= SpawnLevelUpTurn[i])
                {
                    spawnLevel = SpawnLevels[i];
                    break;
                }
            }
            
            var id = (uint)Random.Range(spawnLevel.x, spawnLevel.y + 1 + (uint)GameManager.Instance.technologyLevel);
            ManagerLocator.Get<PlaceGridManager>().RondomNewAttacker(id);
        }
     }
    
}