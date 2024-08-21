using System;
using Cysharp.Threading.Tasks;
using GamePlay.framework;
using UnityEngine;

namespace GameLogic.GamePlay
{
    public class PlacementGrid:MonoBehaviour
    {
        public Attacker m_Attacker;

        /// <summary>
        /// 格子在第几排
        /// </summary>
        public int RowId;

        public void DoPutDown(Attacker attacker)
        {
            if (m_Attacker != null)
            {
                if (m_Attacker.CheckSynthesize(attacker.ID))
                {
                    Debug.Log("合成成功");
                    var targetId = m_Attacker.ID + 1;
                    ManagerLocator.Get<PlacementGridManager>().RemoveAttacker(attacker);
                    ManagerLocator.Get<PlacementGridManager>().RemoveAttacker(m_Attacker);
                    ManagerLocator.Get<PlacementGridManager>().CreateNewAttacker(targetId, this);
                }
                else
                {
                    ReplaceAttacker(attacker);
                }
            }
            else
            {
                PlaceAttacker(attacker);
            }
        }
        
        public void PlaceAttacker(Attacker attacker)
        {
            m_Attacker = attacker;
            if (attacker.m_PlacementGrid != null)
            {
                ManagerLocator.Get<PlacementGridManager>().ClearGrid(attacker.m_PlacementGrid);

            }
            SetAttackerGrid(attacker,this);
        }
        
        public void ReplaceAttacker(Attacker attacker)
        {
            Debug.Log($"交换位置  {m_Attacker.ID}  {attacker.ID}");
            var targetParent = attacker.m_PlacementGrid;
            var targetAttacker = m_Attacker;
            SetAttackerGrid(attacker, this);
            SetAttackerGrid(targetAttacker, targetParent);

        }
        
        public void SetAttacker(Attacker attacker)
        {
            m_Attacker = attacker;
            if (attacker != null)
            {
                attacker.m_PlacementGrid = this;

            }
        }

        private void SetAttackerGrid(Attacker attacker,PlacementGrid placement)
        {
            
            attacker.transform.SetParent(placement.transform);
            var rect = attacker.transform.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;
            rect.localScale = Vector3.one;
            
            ManagerLocator.Get<PlacementGridManager>().PlaceGrid(placement,attacker);
        }
    }
}