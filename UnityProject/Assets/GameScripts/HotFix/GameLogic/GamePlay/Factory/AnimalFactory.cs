using Cysharp.Threading.Tasks;
using GameBase;
using TEngine;
using TMPro;
using UnityEngine;

namespace GameLogic.GamePlay.Factory
{
    public class AnimalFactory:IFactory
    {
        private string[] attackerPool = new string[] { "Animal_Sheep","Animal_Pig","Animal_Cow","Animal_Chicken"};
        private Color[] colorPool  = new Color[] { Color.green, Color.cyan, Color.magenta, Color.yellow, Color.red,  };
        
        public async UniTask<GameObject> CreateAnimalAsync(uint id)
        {
            GameObject go;
            
            if (id <= 3)
            {
                //生成🐏
                go = await GameModule.Resource.LoadGameObjectAsync(attackerPool[0]);
            }
            else if (id <= 6)
            {
                //生成🐖🐖🐖🐖
                go = await GameModule.Resource.LoadGameObjectAsync(attackerPool[1]);
            }
            else if (id <= 9)
            {
                //生成🐂
                go = await GameModule.Resource.LoadGameObjectAsync(attackerPool[2]);
            }
            else
            {
                //生成🐓
                go = await GameModule.Resource.LoadGameObjectAsync(attackerPool[3]);

            }
            go.GetComponent<Animal_Attacker>().ID = id;
            
            return go;
        } 
        
        public GameObject CreateAnimal(uint id)
        {
            GameObject go;
            
            if (id <= 3)
            {
                //生成🐏
                go = GameModule.Resource.LoadGameObject(attackerPool[0]);
            }
            else if (id <= 6)
            {
                //生成🐖🐖🐖🐖
                go = GameModule.Resource.LoadGameObject(attackerPool[1]);
            }
            else if (id <= 9)
            {
                //生成🐂
                go = GameModule.Resource.LoadGameObject(attackerPool[2]);
            }
            else
            {
                //生成🐓
                go = GameModule.Resource.LoadGameObject(attackerPool[3]);
        
            }
            go.GetComponent<Animal_Attacker>().ID = id;
            
            return go;
        }
        
        //随机一个种类
        public GameObject CreateAnimalRandom(uint id)
        {
            GameObject go;

            var animalType = Random.Range(0, 3);
            go = GameModule.Resource.LoadGameObject(attackerPool[animalType]);
        
          
            go.GetComponent<Animal_Attacker>().ID = id;
            go.GetComponent<Animal_Attacker>().animalType = animalType;
            id = (uint)Mathf.Clamp(id, 1, 5);

            go.GetComponent<SpriteRenderer>().color = colorPool[id-1];
            return go;
        } 
        
        public GameObject CreateAnimalAsType(int animalType,uint id)
        {
            GameObject go;
            
            go = GameModule.Resource.LoadGameObject(attackerPool[animalType]);
            
            go.GetComponent<Animal_Attacker>().ID = id;
            go.GetComponent<Animal_Attacker>().animalType = animalType;
            id = (uint)Mathf.Clamp(id, 1, 5);
            go.GetComponent<SpriteRenderer>().color = colorPool[id-1];
            return go;
        }

        public void Init()
        {
        }
    }
}