using Cysharp.Threading.Tasks;
using GameLogic.Common.Particle;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class BulletExplore:Bullet
    {
        private float SizeRate
        {
            get
            {
                if (Level == 1)
                {
                    return 0.8f;

                }
                else if (Level == 2)
                {
                    return 1f;

                }else if (Level == 3)
                {
                    return 1.2f;

                }else if (Level == 4)
                {
                    return 1.4f;

                }else if (Level == 5)
                {
                    return 1.6f;

                }

                return 1f;

            }
        }

        public override void Init()
        {
            base.Init();
            transform.localScale = Vector3.one * SizeRate;

        }

        public override void AttackMonster(Monster monster, Vector2 position)
        {
            DoExplore().Forget();
            Destroy(gameObject);
        }

        private async UniTask DoExplore()
        {
            GameObject go = await ManagerLocator.Get<FactoryManager>().Get<EffectFactory>().CreateEffect(EffectType.Explore, transform.position);
            go.GetComponent<ExploreTrigger>().SetDamage(damage);
            go.transform.localScale = Vector3.one * SizeRate;
        }
    }
}