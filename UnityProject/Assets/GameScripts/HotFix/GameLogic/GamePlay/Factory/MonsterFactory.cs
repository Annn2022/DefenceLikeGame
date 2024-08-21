using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay.Factory
{
    public class MonsterFactory:IFactory
    {
        private static string[] monsterPathPool = new string[] { "Skeleton_01"};
        private static string[] monsterBigPathPool = new string[] { "Skeleton_c_01"};

        public GameObject CreateNewMonster(uint id)
        {
            GameObject go = null;
            if (id == 0)
            {
                go = GameModule.Resource.LoadGameObject(monsterPathPool[0]);
                
            }
            else
            {
                go = GameModule.Resource.LoadGameObject(monsterPathPool[0]);
            }
            
            go.name = $"Monster_{id}";
            go.GetComponent<Monster>().HP = (int)Mathf.Pow(2f, id+1);
            
            return go;
        }  
        
        public GameObject CreateBigMonster(uint id)
        {
            GameObject go = null;
            if (id == 0)
            {
                go = GameModule.Resource.LoadGameObject(monsterBigPathPool[0]);
                
            }
            else
            {
                go = GameModule.Resource.LoadGameObject(monsterBigPathPool[0]);
            }
            
            go.name = $"Monster_Big_{id}";
            go.GetComponent<Monster>().HP = (int)Mathf.Pow(2f, id+1);
            
            return go;
        }

        public void Init()
        {
            
        }
    }
}