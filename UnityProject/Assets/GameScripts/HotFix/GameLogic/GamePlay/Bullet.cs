using System;
using TMPro;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class Bullet : MonoBehaviour
    {
        public EnemyGrid m_EnemyGrid;
        
        private int damage;
        public int Damage
        {
            get => damage;
            set
            {
                damage = value;
                m_Txt.text = damage.ToString();
            }
        }

        public float speed = 2;
        public TMP_Text m_Txt;

        private void Update()
        {
            Move();
            RotateSelf();
        }

        private void Move()
        {
            transform.parent.position += Vector3.up * Time.deltaTime * speed;
        }

        private void RotateSelf()
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * 15);
        }

        public void AttackMonster(Monster monster)
        {
            Destroy(transform.parent.gameObject);
        }
        
        public void ClearSelf()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}