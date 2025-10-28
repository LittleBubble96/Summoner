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
        
        public static CommonArgs CreateThreeArgs<T,T2,T3>(T args , T2 args2 , T3 args3)
        {
            CommonThreeArgs<T,T2,T3> threeArgs = ReferencePool.Acquire<CommonThreeArgs<T,T2,T3>>();
            threeArgs.Arg1 = args;
            threeArgs.Arg2 = args2;
            threeArgs.Arg3 = args3;
            return threeArgs;
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
    
    public class CommonThreeArgs<T1, T2,T3> : CommonTwoArgs<T1,T2>
    {
        public T3 Arg3 { get; set; }

        public override void Clear()
        {
            base.Clear();
            Arg3 = default;
        }
    }
}