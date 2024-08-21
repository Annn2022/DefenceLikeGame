using System;
using Cysharp.Threading.Tasks;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using TMPro;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class Monster:MonoBehaviour
    {
        public EnemyGrid m_EnemyGrid;

        public int      hp;
        
        public int HP
        {
            get => hp;
            set
            {
                hp = value;
                m_Txt.text = hp.ToString();
                if (CoinCount == 0)
                {
                    CoinCount = hp;
                }
            }
        }
        
        public int CoinCount;

        public TMP_Text m_Txt;
        public float    speed = 0.5f;


        private void Start()
        {
            m_Txt.text = hp.ToString();
        }

        private void Update()
        {
            Move();
        }

        public bool isDead
        {
            get => hp <= 0;
        }
        
        public void BulletAttack(Bullet bullet)
        {
            TakeDamage(bullet.Damage);
            ManagerLocator.Get<FactoryManager>().Get<EffectFactory>().CreateEffect(EffectType.Hit, transform.position).Forget();
        }
        
        private void Move()
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (isDead)
            {
                m_EnemyGrid?.RemoveMonster();
                GameManager.Instance.CointCount += CoinCount;
                Destroy(this.gameObject);
            }
        }

        public void ClearSelf()
        {
            Destroy(this.gameObject);

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Bullet bullet))
            {
                BulletAttack(bullet);
                bullet.AttackMonster(this);
            }
        }
    }
}