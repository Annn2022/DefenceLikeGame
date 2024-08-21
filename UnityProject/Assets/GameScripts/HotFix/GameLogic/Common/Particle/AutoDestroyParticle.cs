using GameLogic.GamePlay.Factory;
using GamePlay.framework;
using UnityEngine;

namespace GameLogic.Common.Particle
{
    public class AutoDestroyParticle : MonoBehaviour
    {
        private ParticleSystem particleSystem;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();

            // 如果没有ParticleSystem组件，打印错误信息
            if (particleSystem == null)
            {
                Debug.LogError("No ParticleSystem component found on the game object.");
            }
        }

        void Update()
        {
            // 检查粒子系统是否已经停止播放且没有剩余的粒子
            if (particleSystem != null && !particleSystem.IsAlive(true))
            {
                Destroy(gameObject);
            }
        }
    }
}