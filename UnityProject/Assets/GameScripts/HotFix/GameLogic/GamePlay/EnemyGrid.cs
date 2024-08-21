using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic.GamePlay
{
    /// <summary>
    /// 怪物前进的地块
    /// </summary>
    public class EnemyGrid : MonoBehaviour
    {
        /// <summary>
        /// 一个格子上可能存在多个格子
        /// </summary>
        public List<Bullet> ListBullet = new List<Bullet>();

        /// <summary>
        /// 一个格子上最多仅有一个monster
        /// </summary>
        public Monster m_Monster;
        
        public async UniTask SpawnBullet(string bulletPath)
        {
            var bulletInstence=  await GameModule.Resource.LoadGameObjectAsync(bulletPath);
            var bullet = bulletInstence.GetComponent<Bullet>();
            MoveBullet(bullet);
        }

        /// <summary>
        /// 移动到当前格子 如果有怪物则攻击并销毁自身
        /// </summary>
        /// <param name="bullet"></param>
        public void MoveBullet(Bullet bullet)
        {
            if (bullet.m_EnemyGrid)
            {
                bullet.m_EnemyGrid.RemoveBullet(bullet);
            }
            
            bullet.transform.SetParent(transform);
            var rect = bullet.transform.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;
            rect.localScale = Vector3.one;
            
            if (m_Monster != null)
            {
                //需要攻击怪物
                m_Monster.BulletAttack(bullet);
                RemoveBulletInstence(bullet);
            }
            else
            {
                AddBullet(bullet);

            }
        }

        private void RemoveBullet(Bullet bullet)
        {
            if (ListBullet.Contains(bullet))
            {
                ListBullet.Remove(bullet);
            }
        }

        public void RemoveBulletInstence(Bullet bullet)
        {
            if (ListBullet.Contains(bullet))
            {
                ListBullet.Remove(bullet);
            }
            Destroy(bullet.gameObject);
        }
        /// <summary>
        /// 直接销毁当前格子所有子弹
        /// </summary>
        public void RemoveAllBulletInstence()
        {
            foreach (var bullet in ListBullet)
            {
                Destroy(bullet.gameObject);
            }
            
            ListBullet.Clear();
        }

        private void AddBullet(Bullet bullet)
        {
            if (!ListBullet.Contains(bullet))
            {
                ListBullet.Add(bullet);
                bullet.m_EnemyGrid = this;
                UpdateLayout();
            }
        }

        public async UniTask SpawnMonster(string monsterName)
        {
            var monsterInstence=  await GameModule.Resource.LoadGameObjectAsync(monsterName);
            var monster = monsterInstence.GetComponent<Monster>();
            m_Monster = monster;
            MoveMonster(monster);
        }
        
        public void MoveMonster(Monster monster)
        {
            if (monster.m_EnemyGrid)
            {
                monster.m_EnemyGrid.RemoveMonster();
            }
            
            monster.transform.SetParent(transform);
            var rect = monster.transform.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;
            rect.localScale = Vector3.one;

            m_Monster = monster;
            monster.m_EnemyGrid = this;
            if (ListBullet.Count > 0)
            {
                for (int i = ListBullet.Count - 1; i >= 0; i--)
                {
                    if (m_Monster != null)
                    {
                        m_Monster.BulletAttack(ListBullet[i]);
                    }
                }
                RemoveAllBulletInstence();

            }
        }
        
        public void RemoveMonster()
        {
            m_Monster = null;
        }


        #region 子弹的排列
        public RectTransform[] elements; // UI元素的数组
        public Vector2         cellSize = new Vector2(64, 64); // 每个格子的大小
        public Vector2         spacing  = new Vector2(0, 0); // 每个格子之间的间距

        public void UpdateLayout()
        {
            elements = new RectTransform[ListBullet.Count];
            for (int i = 0; i < ListBullet.Count; i++)
            {
                elements[i] = ListBullet[i].transform as RectTransform;
            }
            int elementCount = elements.Length;

            if (elementCount == 1)
            {
                // 1个元素，居中显示
                elements[0].anchoredPosition = Vector2.zero;
            }
            else if (elementCount == 2)
            {
                // 2个元素，横向排列
                elements[0].anchoredPosition = new Vector2(-cellSize.x / 2 - spacing.x / 2, 0);
                elements[1].anchoredPosition = new Vector2(cellSize.x / 2 + spacing.x / 2, 0);
            }
            else if (elementCount == 3)
            {
                // 三个元素，成三角形
                elements[0].anchoredPosition = new Vector2(-cellSize.x / 2 - spacing.x / 2, cellSize.y/2 + spacing.y/2);
                elements[1].anchoredPosition = new Vector2(cellSize.x / 2 + spacing.x / 2, cellSize.y/2 + spacing.y/2);
                elements[2].anchoredPosition = new Vector2(0, -cellSize.y/2 - spacing.y/2);
            }
            else if (elementCount == 4)
            {
                // 四个元素，2x2排列
                elements[0].anchoredPosition = new Vector2(-cellSize.x / 2 - spacing.x / 2, cellSize.y/2 + spacing.y/2);
                elements[1].anchoredPosition = new Vector2(cellSize.x / 2 + spacing.x / 2, cellSize.y/2 + spacing.y/2);
                elements[2].anchoredPosition = new Vector2(-cellSize.x / 2 - spacing.x / 2, -cellSize.y/2 - spacing.y/2);
                elements[3].anchoredPosition = new Vector2(cellSize.x / 2 + spacing.x / 2, -cellSize.y/2 - spacing.y/2);
            }
            else
            {
                Debug.LogWarning("Unsupported number of elements");
            }
        }
        

        #endregion
    }
}