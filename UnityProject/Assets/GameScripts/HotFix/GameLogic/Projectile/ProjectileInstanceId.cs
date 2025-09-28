using System;

namespace GameLogic.Game
{
    /// <summary>
    /// 召唤物实例ID生成器
    /// 确保每个召唤物实例拥有唯一的ID
    /// </summary>
    public static class ProjectileInstanceIdGenerator
    {
        private static long _lastId = 0;
    
        /// <summary>
        /// 生成新的唯一特效实例ID
        /// </summary>
        public static long GenerateNewId()
        {
            _lastId++;
            return _lastId;
        }

        public static void ClearCache()
        {
            _lastId = 0;
        }
    }

    /// <summary>
    /// 召唤物实例ID结构体
    /// 包装long类型ID，提供类型安全
    /// </summary>
    public struct ProjectileInstanceId : IEquatable<ProjectileInstanceId>
    {
        public long Id { get; }
    
        public ProjectileInstanceId(long id)
        {
            Id = id;
        }
    
        // 生成新的实例ID
        public static ProjectileInstanceId NewId()
        {
            return new ProjectileInstanceId(ProjectileInstanceIdGenerator.GenerateNewId());
        }
    
        // 相等性检查
        public bool Equals(ProjectileInstanceId other)
        {
            return Id == other.Id;
        }
    
        public override bool Equals(object obj)
        {
            return obj is ProjectileInstanceId other && Equals(other);
        }
    
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    
        public static bool operator ==(ProjectileInstanceId left, ProjectileInstanceId right)
        {
            return left.Equals(right);
        }
    
        public static bool operator !=(ProjectileInstanceId left, ProjectileInstanceId right)
        {
            return !left.Equals(right);
        }
    }

}