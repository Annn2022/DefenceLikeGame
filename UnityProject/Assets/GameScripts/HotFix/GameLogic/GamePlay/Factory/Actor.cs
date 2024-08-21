using TEngine;

namespace GameLogic.GamePlay.Factory
{
    public class Actor:ObjectBase
    {
        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象池时触发。</param>
        protected override void Release(bool isShutdown) {}

        /// <summary>
        /// 创建Actor对象。
        /// </summary>
        /// <param name="actorName">对象名称。</param>
        /// <param name="target">对象持有实例。</param>
        /// <returns></returns>
        public static Actor Create(string name, object target)
        {
            var actor = MemoryPool.Acquire<Actor>();
            actor.Initialize(name, target);
            return actor;
        }
    }
}