using Framework.Contracts.Helper;
using System;

namespace Framework.Common.Helper
{
    public class Maybe<T> : IMaybe<T>
        where T : class
    {
        private readonly T _value;

        public Maybe(T value)
        {
            _value = value;
        }

        public bool HasValue => _value != default(T);

        public T Value => _value;

        public IMaybe<TConvert> Convert<TConvert>(Func<T, TConvert> converter) where TConvert : class
        {
            if (HasValue)
            {
                return new Maybe<TConvert>(converter(Value));
            }
            return new Maybe<TConvert>(null);
        }

        public T ExposeException(Func<Exception> exceptionCreator)
        {
            if (HasValue)
            {
                return Value;
            }
            throw exceptionCreator();
        }
    }
}
