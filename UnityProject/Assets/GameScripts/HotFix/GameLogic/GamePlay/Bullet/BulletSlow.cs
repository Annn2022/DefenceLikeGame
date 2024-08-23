using UnityEngine;

namespace GameLogic.GamePlay
{
    public class BulletSlow:Bullet
    {
        private float slowDuration = 5f;

        private float speedRate
        {
            get
            {
                if (Level == 1)
                {
                    return 0.7f;

                }
                else if (Level == 2)
                {
                    return 0.6f;

                }else if (Level == 3)
                {
                    return 0.5f;

                }else if (Level == 4)
                {
                    return 0.4f;

                }else if (Level == 5)
                {
                    return 0.3f;

                }

                return 1f;

            }
        }
        public override void AttackMonster(Monster monster,Vector2 position)
        {
            base.AttackMonster(monster,position);
            //Destroy(transform.parent.gameObject);
            monster.AddSlowDown(speedRate,slowDuration);
        }
    }
}