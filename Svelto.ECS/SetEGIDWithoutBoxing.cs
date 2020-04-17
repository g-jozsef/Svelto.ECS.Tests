using System;

namespace Svelto.ECS.Internal
{
    public delegate void SetEGIDWithoutBoxingActionCast<T>(ref T target, EGID egid) where T : struct, IEntityComponent;

    static class SetEGIDWithoutBoxing<T> where T : struct, IEntityComponent
    {
        public static readonly SetEGIDWithoutBoxingActionCast<T> SetIDWithoutBoxing = MakeSetter();

        public static void Warmup() { }

        static SetEGIDWithoutBoxingActionCast<T> MakeSetter()
        {
            if (ComponentBuilder<T>.HAS_EGID)
            {
                var method = typeof(Foo).GetMethod(nameof(Foo.SetEGIDImpl)).MakeGenericMethod(typeof(T));
                return (SetEGIDWithoutBoxingActionCast<T>) Delegate.CreateDelegate(
                    typeof(SetEGIDWithoutBoxingActionCast<T>), method);
            }

            return null;
        }

        static class Foo
        {    
            public static void SetEGIDImpl<U>(ref U target, EGID egid) where U : struct, INeedEGID
            {
                target.ID = egid;
            }
        }
    }
}