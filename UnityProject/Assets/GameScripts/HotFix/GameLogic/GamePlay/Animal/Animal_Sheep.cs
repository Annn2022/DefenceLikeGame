using Cysharp.Threading.Tasks;

namespace GameLogic.GamePlay
{
    public class Animal_Sheep:Animal_Attacker
    {
        public int oneAttackCount
        {
            get
            {
                return (int)ID;
            }
        }

        protected override void SpawnBullet()
        {
            int count = oneAttackCount;
            UniTask.Create(async () =>
            {
                while (count > 0)
                {
                    await UniTask.Delay(300);
                    base.SpawnBullet();
                    count--;
                }
            });
        }
    }
}