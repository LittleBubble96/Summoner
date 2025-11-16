using UnityEngine;

namespace GameLogic.Game
{
    public struct AreaParams
    {
        public AreaType AreaType { get; private set; }

        public Vector3 Origin { get; private set; }

        // 球形/扇形共用参数
        public float Radius { get; private set; }
        // 盒形参数
        public Vector3 BoxSize { get; private set; }
        // 扇形专属参数
        public float SectorAngle { get; private set; } // 角度（0-360）
        public Vector3 Direction { get; private set; } // 朝向

        public static AreaParams New(AreaType areaType)
        {
            AreaParams areaParams = new AreaParams
            {
                AreaType = areaType
            };
            return areaParams;
        }
        
        // 链式配置方法（简化参数设置）
        public AreaParams SetOrigin(Vector3 origin)
        {
            this.Origin = origin;
            return this;
        }

        public AreaParams SetRadius(float radius)
        {
            this.Radius = radius;
            return this;
        }

        public AreaParams SetBoxSize(Vector3 size)
        {
            BoxSize = size;
            return this;
        }

        public AreaParams SetSectorAngle(float angle)
        {
            SectorAngle = angle;
            return this;
        }

        public AreaParams SetDirection(Vector3 dir)
        {
            Direction = dir;
            return this;
        }
    }
}