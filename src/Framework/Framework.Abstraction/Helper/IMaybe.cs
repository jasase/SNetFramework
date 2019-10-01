using System;

namespace Framework.Abstraction.Helper
{
    public interface IMaybe<out T>
      where T : class
    {
        bool HasValue { get; }

        T Value { get; }

        IMaybe<TConvert> Convert<TConvert>(Func<T, TConvert> converter)
            where TConvert : class;

        T ExposeException(Func<Exception> exceptionCreator);
    }
}
