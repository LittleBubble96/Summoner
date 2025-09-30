using System;

namespace GameLogic.Game
{
    /// <summary>
    /// 特效实例ID生成器
    /// 确保每个特效实例拥有唯一的ID
    /// </summary>
    public static class ActorInstanceIdGenerator
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
    }

    /// <summary>
    /// 特效实例ID结构体
    /// 包装long类型ID，提供类型安全
    /// </summary>
    public struct ActorInstanceId : IEquatable<ActorInstanceId>
    {
        public long Id { get; }
    
        public ActorInstanceId(long id)
        {
            Id = id;
        }
    
        // 生成新的实例ID
        public static ActorInstanceId NewId()
        {
            return new ActorInstanceId(ActorInstanceIdGenerator.GenerateNewId());
        }
    
        // 相等性检查
        public bool Equals(ActorInstanceId other)
        {
            return Id == other.Id;
        }
    
        public override bool Equals(object obj)
        {
            return obj is ActorInstanceId other && Equals(other);
        }
    
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    
        public static bool operator ==(ActorInstanceId left, ActorInstanceId right)
        {
            return left.Equals(right);
        }
    
        public static bool operator !=(ActorInstanceId left, ActorInstanceId right)
        {
            return !left.Equals(right);
        }
    }

}