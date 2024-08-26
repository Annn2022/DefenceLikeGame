using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameLogic.GamePlay
{
    public class Animal_Sheep:Animal_Attacker
    {
        private CancellationTokenSource source_Shoot = new CancellationTokenSource();

        public int oneAttackCount
        {
            get
            {
                return (int)ID;
            }
        }

        private void OnDestroy()
        {
            source_Shoot.Cancel();
        }

        protected override void SpawnBullet()
        {
            int count = oneAttackCount;
            UniTask.Create(async () =>
            {
                while (count > 0)
                {
                    await UniTask.Delay(300).AttachExternalCancellation(source_Shoot.Token);
                    base.SpawnBullet();
                    count--;
                }
            });
        }
    }
}