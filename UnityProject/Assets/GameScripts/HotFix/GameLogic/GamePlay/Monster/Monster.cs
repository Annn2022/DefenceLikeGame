using System;
using System.Threading;
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

        public int hp;
        public int HP
        {
            get => hp;
            set
            {
                hp = value;
                m_Txt.text = hp.ToString();
                if (CoinCount == 0)
                {
                    CoinCount = hp/8;
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

        private void OnDestroy()
        {
            source_Slow.Cancel();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Bullet bullet))
            {
                BulletAttack(bullet);
                bullet.AttackMonster(this,col.ClosestPoint((Vector2)transform.position));
            }
        }

        


        private CancellationTokenSource source_Slow = new CancellationTokenSource();
        /// <summary>
        /// 添加限时的减速
        /// </summary>
        /// <param name="speedRate"></param>
        /// <param name="duration"></param>
        public void AddSlowDown(float speedRate, float duration)
        {
            source_Slow.Cancel();
            UniTask.Create(async () =>
            {
                var spd = speed;
                speed *= speedRate;
                GetComponent<SpriteRenderer>().color = Color.cyan;
                await UniTask.Delay(TimeSpan.FromSeconds(duration)).AttachExternalCancellation(source_Slow.Token);
                GetComponent<SpriteRenderer>().color = Color.white;
                speed = spd;

            }).AttachExternalCancellation(source_Slow.Token);
        }
    }
}