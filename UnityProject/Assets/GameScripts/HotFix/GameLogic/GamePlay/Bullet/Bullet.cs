using System;
using Cysharp.Threading.Tasks;
using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using TMPro;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class Bullet : MonoBehaviour
    {
        public EnemyGrid m_EnemyGrid;
        
        protected int damage;
        public int Damage
        {
            get => damage;
            set
            {
                damage = value;
                if (m_Txt != null)
                {
                    m_Txt.text = damage.ToString();

                }
            }
        }

        /// <summary>
        /// 子弹等级 依据创建动物生成
        /// </summary>
        public int Level;

        public float speed = 2;
        public TMP_Text m_Txt;

        public virtual void Init()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        private void Update()
        {
            Move();
            //RotateSelf();
        }

        private void Move()
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
        }

        private void RotateSelf()
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * 15);
        }

        public virtual void AttackMonster(Monster monster,Vector2 position)
        {
            ManagerLocator.Get<FactoryManager>().Get<EffectFactory>().CreateEffect(EffectType.Hit, position).Forget();
            Destroy(transform.gameObject);
        }
        
        public void ClearSelf()
        {
            Destroy(transform.gameObject);
        }
    }
}