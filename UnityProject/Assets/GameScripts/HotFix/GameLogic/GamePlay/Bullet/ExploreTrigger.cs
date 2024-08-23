using System;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class ExploreTrigger : MonoBehaviour
    {
        private int damage;

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Monster monster))
            {
                monster.TakeDamage(damage);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Monster monster))
            {
                monster.TakeDamage(damage);
            }
        }
    }
   
}