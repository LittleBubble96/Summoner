using GameBase;

namespace GameLogic.Game
{
    public class GameSystemBase : ILogicSys
    {
        public virtual bool OnInit()
        {
            return true;
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnRoleLogout()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }

        public virtual void OnApplicationPause(bool pause)
        {
        }
    }
}