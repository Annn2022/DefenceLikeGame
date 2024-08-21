using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay.Factory
{
    public class BulletFactory:IFactory
    {
        private string[] bulletPathPool = new string[] { "AnimalBullet_01"};
        
        public GameObject CreateBullet(uint id)
        {
            GameObject go;
            
            if (id <= 3)
            {
                //生成🐏
                go = GameModule.Resource.LoadGameObject(bulletPathPool[0]);
            }
            else if (id <= 6)
            {
                //生成🐖🐖🐖🐖
                go = GameModule.Resource.LoadGameObject(bulletPathPool[0]);
            }
            else if (id <= 9)
            {
                //生成🐂
                go = GameModule.Resource.LoadGameObject(bulletPathPool[0]);
            }
            else
            {
                //生成🐓
                go = GameModule.Resource.LoadGameObject(bulletPathPool[0]);

            }
            go.GetComponentInChildren<Bullet>().Damage = (int)Mathf.Pow(2f,id);
            
            return go;
        }

        public void Init()
        {
            
        }
    }
}