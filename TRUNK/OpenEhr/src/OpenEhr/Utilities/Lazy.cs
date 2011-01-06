using System;
using OpenEhr.DesignByContract;
using System.Runtime.Serialization;
using System.Threading;


namespace OpenEhr.Utilities
{

    public delegate TResult Func<T, TResult>(T arg);
    public delegate TResult Func<TResult>();

    [Serializable]
    class Lazy<T> : ISerializable
    {
        LazyBlock lazyBlock;
        volatile object value;

        public Lazy() : this(null) { }

        public Lazy(Func<T> valueFactory)
        {
            this.lazyBlock = new LazyBlock(valueFactory);
        }

        #region ISerializable Members

        const string LazyValueAttribute = "LazyValue";

        protected Lazy(SerializationInfo info, StreamingContext context)
        {
            Check.Require(info != null, "info must not be null.");

            this.lazyBlock = new LazyBlock(null);
            this.value = new Boxed((T)info.GetValue(LazyValueAttribute, typeof(T)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Check.Require(info != null, "info must not be null.");
            info.AddValue(LazyValueAttribute, this.value);
        }

        #endregion

        private T LazyInitValue()
        {
            //This is one of those rare cases according to the documentation:
            //http://msdn.microsoft.com/en-us/library/4bw5ewxy%28VS.80%29.aspx

#pragma warning disable 420

            lazyBlock.Initialise(Boxed.CreateValueFunc, ref value);

#pragma warning restore 420

            Boxed boxedValue = value as Boxed;
            Check.Assert(boxedValue != null);
            return boxedValue.Unboxed;
        }

        public bool IsValueCreated
        { get { return value != null && value is Boxed; } }

        public T Value
        {
            get
            {
                Boxed boxed = value as Boxed;
                if (boxed != null)
                    return boxed.Unboxed;
                LazyExceptionHolder holder = value as LazyExceptionHolder;
                if (holder != null)
                    throw holder.exception;
                return LazyInitValue();
            }
        }

        private class Boxed
        {
            static Boxed()
            {
                createValueFunc = new Func<Func<T>, object>(Boxed.CreateValue);
            }

            internal Boxed(T unboxed)
            {
                Check.Require(unboxed != null, "unboxed must not be null.");
                this.unboxed = unboxed;
            }

            private T unboxed;
            internal T Unboxed
            {
                get { return this.unboxed; }
            }

            private static Func<Func<T>, object> createValueFunc;
            internal static Func<Func<T>, object> CreateValueFunc
            {
                get { return createValueFunc; }
            }


            internal static Boxed CreateValue(Func<T> valueFactory)
            {
                Boxed boxed;
                if (valueFactory != null)
                    boxed = new Boxed(valueFactory());
                else
                {
                    boxed = new Boxed(Activator.CreateInstance<T>());
                }

                Check.Ensure(boxed != null, "boxed must not be null.");

                return boxed;
            }

        }

        private struct LazyBlock
        {
            readonly Func<T> valueFactory;

            internal LazyBlock(Func<T> valueFactory)
            {
                this.valueFactory = valueFactory;
            }

            private static bool TrySetValue(object value, ref object target)
            {
                if (Interlocked.CompareExchange(ref target, value, null) == null)
                    return true;

                IDisposable disposable = value as IDisposable;
                if (disposable != null)
                    disposable.Dispose();

                return false;
            }

            internal object Initialise(Func<Func<T>, object> createValueFunc, ref object target)
            {
                object block = null;
                try
                {
                    block = createValueFunc(valueFactory);
                }
                catch (Exception ex)
                {
                    block = new LazyExceptionHolder(ex);
                    LazyBlock.TrySetValue(block, ref target);
                    throw;
                }

                if (!LazyBlock.TrySetValue(block, ref target) && target is LazyExceptionHolder)
                    throw (target as LazyExceptionHolder).exception;

                return target;
            }
        }

        private class LazyExceptionHolder
        {
            internal Exception exception;

            internal LazyExceptionHolder(Exception ex)
            {
                this.exception = ex;
            }
        }
    }
}
