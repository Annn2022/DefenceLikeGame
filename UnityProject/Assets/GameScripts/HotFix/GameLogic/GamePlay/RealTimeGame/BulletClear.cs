using System;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class BulletClear:MonoBehaviour
    {
        public bool ClearBullet;
        public bool ClearMonster;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Bullet bullet) && ClearBullet)
            {
                bullet.ClearSelf();
            }
            else if (col.TryGetComponent(out Monster monster) && ClearMonster)
            {
                monster.ClearSelf();
                GameManager.Instance.Hp--;
            }
            
        }
    }
}