using System;
using GameFramework;

namespace GameLogic.Game
{
    public class CommonArgs : IReference ,IDisposable
    {
        public static CommonArgs CreateOneArgs<T>(T args)
        {
            CommonOneArgs<T> oneArgs = ReferencePool.Acquire<CommonOneArgs<T>>();
            oneArgs.Arg1 = args;
            return oneArgs;
        }
        
        public static CommonArgs CreateTwoArgs<T,T2>(T args , T2 args2)
        {
            CommonTwoArgs<T,T2> twoArgs = ReferencePool.Acquire<CommonTwoArgs<T,T2>>();
            twoArgs.Arg1 = args;
            twoArgs.Arg2 = args2;
            return twoArgs;
        }

        public virtual void Clear()
        {
            
        }

        public void Dispose()
        {
            ReferencePool.Release(this);
        }
    }

    public class CommonOneArgs<T> : CommonArgs
    {
        public T Arg1 { get; set; }

        public override void Clear()
        {
            base.Clear();
            Arg1 = default;
        }
    }

    public class CommonTwoArgs<T1, T2> : CommonOneArgs<T1>
    {
        public T2 Arg2 { get; set; }

        public override void Clear()
        {
            base.Clear();
            Arg2 = default;
        }
    }
}